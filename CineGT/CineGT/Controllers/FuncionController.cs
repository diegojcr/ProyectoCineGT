using CineGT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace CineGT.Controllers
{
    public class FuncionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Crear()
        {
            List<SelectListItem> peliculas = new List<SelectListItem>();
            string cadena = "Data Source=DIEGO\\SQLEXPRESS;Initial Catalog=PROYECTO_CINEGT;Integrated Security=True";

            using (SqlConnection con = new SqlConnection(cadena))
            {
                string query = "select Nombre from Pelicula";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                using(SqlDataReader reader = cmd.ExecuteReader())
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
        public IActionResult Crear(Funcion funcion)
        {
            string cadena = "Data Source=DIEGO\\SQLEXPRESS;Initial Catalog=PROYECTO_CINEGT;Integrated Security=True";
            
            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    SqlCommand cmd = new SqlCommand("SP_NUEVAFUNCION", con);
                    cmd.Parameters.AddWithValue("fechainicio", funcion.FechaInicio);
                    cmd.Parameters.AddWithValue("precio", funcion.Precio);
                    cmd.Parameters.AddWithValue("sala", funcion.Id_Sala);
                    cmd.Parameters.AddWithValue("pelicula", funcion.Id_Pelicula);
                    cmd.Parameters.AddWithValue("estado", funcion.Id_Estado_F);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    cmd.ExecuteNonQuery();
                    return RedirectToAction("Crear", "Funcion");
                }
            }catch(SqlException ex)
            {
                foreach (SqlError error in ex.Errors)
                {
                    ModelState.AddModelError("", $"Numero de error: {error.Number} - {error.Message}");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Ocurrio un error: {ex.Message}");
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
            return View(funcion);
        }

        [HttpPost]
        public IActionResult CargarSesiones(IFormFile fileUpload, bool opcionCarga)
        {
            string cadena = "Data Source=DIEGO\\SQLEXPRESS;Initial Catalog=PROYECTO_CINEGT;Integrated Security=True";
            int bit = 0;
            if(opcionCarga == true)
            {
                bit = 1;
            }
            if (fileUpload != null && fileUpload.Length > 0)
            {
                var filePath = Path.GetTempFileName(); // Genera un archivo temporal

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    fileUpload.CopyTo(stream);
                }

                string mensajeError = string.Empty;

                using (var connection = new SqlConnection(cadena))
                {
                    connection.Open();
                    using (var command = new SqlCommand("spImportarSesionDesdeCSV", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@FilePath", filePath);
                        command.Parameters.AddWithValue("@IgnoreErrors", bit);
                        command.Parameters.Add("@mensajeError", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;

                        command.ExecuteNonQuery();

                        mensajeError = command.Parameters["@mensajeError"].Value.ToString();
                    }
                }

                // Borra el archivo temporal
                System.IO.File.Delete(filePath);

                return View();
            }

            return View();
        }

    }
}
