using CineGT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CineGT.Controllers
{
    public class ReservaController : Controller
    {
        private string cadena = "Data Source=DIEGO\\SQLEXPRESS;Initial Catalog=PROYECTO_CINEGT;Integrated Security=True";
        [HttpGet]
        public IActionResult ReservarAsientos()
        {
            List<SelectListItem> peliculas = new List<SelectListItem>();
            string cadena = "Data Source=DIEGO\\SQLEXPRESS;Initial Catalog=PROYECTO_CINEGT;Integrated Security=True";

            using (SqlConnection con = new SqlConnection(cadena))
            {
                string query = "select Nombre from Pelicula";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        peliculas.Add(new SelectListItem
                        {
                            Value = reader["Nombre"].ToString(),
                            Text = reader["Nombre"].ToString()
                        });
                    }
                }
            }
            ViewBag.Pelicula = peliculas;
            return View();
        }
        [HttpGet]
        public IActionResult ProcesarCompraBoletos()
        {
            List<SelectListItem> peliculas = new List<SelectListItem>();
            string cadena = "Data Source=DIEGO\\SQLEXPRESS;Initial Catalog=PROYECTO_CINEGT;Integrated Security=True";

            using (SqlConnection con = new SqlConnection(cadena))
            {
                string query = "select Nombre from Pelicula";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        peliculas.Add(new SelectListItem
                        {
                            Value = reader["Nombre"].ToString(),
                            Text = reader["Nombre"].ToString()
                        });
                    }
                }
            }
            ViewBag.Pelicula = peliculas;
            return View();
        }

        [HttpPost]
        public IActionResult ProcesarCompraBoletos(Boleto model, string AsientosSeleccionadosJson)
        {

            if (!string.IsNullOrEmpty(AsientosSeleccionadosJson))
            {
                // Deserializar el JSON de los asientos seleccionados
                model.AsientosSeleccionados = JsonSerializer.Deserialize<List<AsientoSeleccionado>>(AsientosSeleccionadosJson);
            }

            string cadena = "Data Source=DIEGO\\SQLEXPRESS;Initial Catalog=PROYECTO_CINEGT;Integrated Security=True";
            try
            {
                using (SqlConnection connection = new SqlConnection(cadena))
                {
                    connection.Open();

                    foreach (var asiento in model.AsientosSeleccionados)
                    {
                        using (SqlCommand command = new SqlCommand("SP_COMPRABOLETOS", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            // Parámetros del procedimiento almacenado
                            command.Parameters.AddWithValue("@NO_BOLETOS", model.NumeroBoletos);
                            command.Parameters.AddWithValue("@PELICULA", model.Pelicula);
                            command.Parameters.AddWithValue("@FILA", asiento.Fila);
                            command.Parameters.AddWithValue("@COLUMNA", Convert.ToInt32(asiento.Columna));
                            command.Parameters.AddWithValue("@VENDEDOR", model.VendedorId);
                            command.Parameters.AddWithValue("@FECHA", model.FechaFuncion);
                            command.Parameters.AddWithValue("@SALA", model.Sala);
                            command.Parameters.AddWithValue("@PAGO", model.TipoPago);
                            command.Parameters.AddWithValue("@CLIENTE", model.ClienteId);
                            command.Parameters.AddWithValue("@ESTADO", model.EstadoCompra);
                            command.Parameters.AddWithValue("@FECHA_COMPRA", DateTime.Now);

                            command.ExecuteNonQuery();
                        }
                    }
                }

                TempData["Success"] = "Compra realizada con éxito";
                return RedirectToAction("ReservarAsientos", "Reserva");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error en la compra: " + ex.Message);
            }
        

        //Recarga la lista de peliculas si hay un error
        List<SelectListItem> peliculas = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(cadena))
            {
                string query = "select Nombre from Pelicula";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        peliculas.Add(new SelectListItem
                        {
                            Value = reader["Nombre"].ToString(),
                            Text = reader["Nombre"].ToString()
                        });
                    }
                }
            }
            ViewBag.Pelicula = peliculas;
            return RedirectToAction("ReservarAsientos", "Reserva");
        }
    }
}
