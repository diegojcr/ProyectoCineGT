using CineGT.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace CineGT.Controllers
{
    public class PeliculaController : Controller
    {
       

        [HttpGet]
        public IActionResult Crear()
        {
            ViewBag.Clasificacion = new List<string>
            {
                "A" , "B" ,"B15" ,"R"
            };
            return View();
        }

        [HttpPost]
        public IActionResult Crear(Pelicula pelicula)
        {
            string cadena = "Data Source=DIEGO\\SQLEXPRESS;Initial Catalog=PROYECTO_CINEGT;Integrated Security=True";

            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    SqlCommand cmd = new SqlCommand("SP_CREARPELICULA", cn);
                    cmd.Parameters.AddWithValue("NOMBRE", pelicula.Nombre);
                    cmd.Parameters.AddWithValue("DURACION", pelicula.Duracion);
                    cmd.Parameters.AddWithValue("CLASIFICACION", pelicula.Clasificacion);
                    cmd.Parameters.AddWithValue("DESCRIPCION", pelicula.Descripcion);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cn.Open();

                    cmd.ExecuteNonQuery();
                    return RedirectToAction("Crear", "Pelicula");
                }
            }
            catch (SqlException)
            {
                ViewData["CrearPMensaje"] = "Error al crear pelicula";
                return View();
            }
            
        }
    }
}
