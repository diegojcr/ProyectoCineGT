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
                    

                    // Crear un DataTable para enviar la lista de asientos al procedimiento almacenado
                    DataTable asientosDataTable = new DataTable();
                    asientosDataTable.Columns.Add("FILA", typeof(int));
                    asientosDataTable.Columns.Add("COLUMNA", typeof(string));

                    // Llenar el DataTable con los datos de los asientos seleccionados
                    foreach (var asiento in model.AsientosSeleccionados)
                    {
                        asientosDataTable.Rows.Add(asiento.fila, asiento.columna);
                     }

                    //Prueba mandar tabla a sql
                    var parametros = new SqlParameter("@lst", SqlDbType.Structured);
                    parametros.Value = asientosDataTable;
                    parametros.TypeName = "dbo.ASIENTODATO";

                    SqlCommand command = new SqlCommand("SP_VENTABOLETOS", connection);
                    
                        

                        // Agregar el parámetro de tipo tabla
                        //SqlParameter tvpParam = command.Parameters.AddWithValue("@lst", asientosDataTable);
                        //tvpParam.SqlDbType = SqlDbType.Structured;
                        //tvpParam.TypeName = "ASIENTODATO"; // Nombre del tipo de tabla en SQL Server

                        

                        // Parámetros adicionales del procedimiento almacenado
                        command.Parameters.Add(parametros);
                        command.Parameters.AddWithValue("@NO_BOLETOS", model.NumeroBoletos);
                        command.Parameters.AddWithValue("@ID_CLIENTE", model.ClienteId);
                        command.Parameters.AddWithValue("@PELICULA", model.Pelicula);
                        command.Parameters.AddWithValue("@ID_VENDEDOR", model.VendedorId);
                        command.Parameters.AddWithValue("@FECHA_FUNCION", model.FechaFuncion);
                        command.Parameters.AddWithValue("@ID_SALA", model.Sala);
                        command.Parameters.AddWithValue("@PAGO", model.TipoPago);
                    command.CommandType = CommandType.StoredProcedure;
                    // Parámetro de salida para la factura
                    SqlParameter facturaOutput = new SqlParameter("@FACTURA", SqlDbType.VarChar, 200)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(facturaOutput);

                    connection.Open();
                    // Ejecutar el procedimiento almacenado
                    command.ExecuteNonQuery();

                        // Obtener el valor del parámetro de salida
                        string numeroFactura = facturaOutput.Value.ToString();
                        TempData["Success"] = $"Compra realizada con éxito. Factura No: {numeroFactura}";
                    ViewBag.MensajeFactura = TempData["Success"];
                }

                return View("ReservarAsientos",model);
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

        [HttpGet]
        public IActionResult ReservarAsientosUsuario()
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
        public IActionResult ProcesarCompraBoletosUsuarios()
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
        public IActionResult ProcesarCompraBoletosUsuarios(Boleto model, string AsientosSeleccionadosJson)
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


                    // Crear un DataTable para enviar la lista de asientos al procedimiento almacenado
                    DataTable asientosDataTable = new DataTable();
                    asientosDataTable.Columns.Add("FILA", typeof(int));
                    asientosDataTable.Columns.Add("COLUMNA", typeof(string));

                    // Llenar el DataTable con los datos de los asientos seleccionados
                    foreach (var asiento in model.AsientosSeleccionados)
                    {
                        asientosDataTable.Rows.Add(asiento.fila, asiento.columna);
                    }

                    //Prueba mandar tabla a sql
                    var parametros = new SqlParameter("@lst", SqlDbType.Structured);
                    parametros.Value = asientosDataTable;
                    parametros.TypeName = "dbo.ASIENTODATO";

                    SqlCommand command = new SqlCommand("SP_VENTABOLETOS", connection);



                    // Agregar el parámetro de tipo tabla
                    //SqlParameter tvpParam = command.Parameters.AddWithValue("@lst", asientosDataTable);
                    //tvpParam.SqlDbType = SqlDbType.Structured;
                    //tvpParam.TypeName = "ASIENTODATO"; // Nombre del tipo de tabla en SQL Server



                    // Parámetros adicionales del procedimiento almacenado
                    command.Parameters.Add(parametros);
                    command.Parameters.AddWithValue("@NO_BOLETOS", model.NumeroBoletos);
                    command.Parameters.AddWithValue("@ID_CLIENTE", model.ClienteId);
                    command.Parameters.AddWithValue("@PELICULA", model.Pelicula);
                    command.Parameters.AddWithValue("@ID_VENDEDOR", model.VendedorId);
                    command.Parameters.AddWithValue("@FECHA_FUNCION", model.FechaFuncion);
                    command.Parameters.AddWithValue("@ID_SALA", model.Sala);
                    command.Parameters.AddWithValue("@PAGO", model.TipoPago);
                    command.CommandType = CommandType.StoredProcedure;
                    // Parámetro de salida para la factura
                    SqlParameter facturaOutput = new SqlParameter("@FACTURA", SqlDbType.VarChar, 200)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(facturaOutput);

                    connection.Open();
                    // Ejecutar el procedimiento almacenado
                    command.ExecuteNonQuery();

                    // Obtener el valor del parámetro de salida
                    string numeroFactura = facturaOutput.Value.ToString();
                    TempData["Success"] = $"Compra realizada con éxito. Factura No: {numeroFactura}";
                    ViewBag.MensajeFactura = TempData["Success"];
                }

                return View("ReservarAsientosUsuario", model);
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
