using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using CineGT.Models;

namespace CineGT.Controllers
{
    public class DeclinarController : Controller
    {
        public string cadena = "Data Source=DIEGO\\SQLEXPRESS;Initial Catalog=PROYECTO_CINEGT;Integrated Security=True";
        public IActionResult DeclinarVenta()
        {
            return View(new Declinar());
        }
      
        [HttpPost]
        public ActionResult DeclinarVenta(Declinar model)
        {
            
                try
                {
                    using (SqlConnection connection = new SqlConnection(cadena))
                    {
                        using (SqlCommand command = new SqlCommand("DECLINAR_VENTA", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@FACTURA", model.NumeroFactura);

                            connection.Open();
                            command.ExecuteNonQuery();

                            // Si no hubo error, mostramos un mensaje de éxito
                            model.Mensaje = "Venta declinada exitosamente.";
                        }
                    }
                }
                catch (SqlException ex)
                {
                    // Capturamos errores de SQL Server y mostramos el mensaje
                    if (ex.Number == 50000) // Error de RAISERROR
                    {
                        model.Mensaje = "Error al declinar venta: " + ex.Message;
                    }
                    else
                    {
                        model.Mensaje = "Ocurrió un error al procesar la solicitud: " + ex.Message;
                    }
                }
                catch (Exception ex)
                {
                    model.Mensaje = "Ocurrió un error inesperado: " + ex.Message;
                }
            
            

            return View(model);
        }
    }
}
