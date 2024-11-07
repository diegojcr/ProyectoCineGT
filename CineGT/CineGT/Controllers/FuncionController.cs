using CineGT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.SqlClient;

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
            string cadena = "Data Source=DIEGO\\SQLEXPRESS;Initial Catalog=CineGT;Integrated Security=True";

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
            string cadena = "Data Source=DIEGO\\SQLEXPRESS;Initial Catalog=CineGT;Integrated Security=True";
            
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
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    con.Open();
                    cmd.ExecuteNonQuery();
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
        public IActionResult CargarSesiones(IFormFile formFile, bool revertirSiHayErrores)
        {
            if (formFile == null || formFile.Length == 0)
            {
                ModelState.AddModelError("","El archivo CSV es requerido.");
                return View();
            }
            return View();
            List<Funcion> nuevasFunciones = new List<Funcion>();
            List<string> errores = new List<string>();

            string cadena = "Data Source=DIEGO\\SQLEXPRESS;Initial Catalog=CineGT;Integrated Security=True";

            using(var reader = new StreamReader(formFile.OpenReadStream()))
            {
                string linea;
                while ((linea = reader.ReadLine()) != null) {
                    var datos = linea.Split(',');

                    try
                    {
                        var funcion = new Funcion
                        {
                            FechaInicio = DateTime.Parse(datos[0]),
                            Precio = double.Parse(datos[1]),
                            Id_Sala = int.Parse(datos[2]),
                            Id_Pelicula = datos[3],
                            Id_Estado_F = int.Parse(datos[4])
                        };
                    }catch (Exception)
                    {

                    }
                }
            }
        }
    }
}
