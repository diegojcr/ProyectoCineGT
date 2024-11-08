using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using CineGT.Models;

namespace CineGT.Controllers
{
    public class DeclinarController : Controller
    {
        public IActionResult DeclinarVenta()
        {
            return View();
        }
        [HttpPost]
        public IActionResult DeclinarVenta(Declinar model)
        {
            string cadena = "Data Source=DIEGO\\SQLEXPRESS;Initial Catalog=PROYECTO_CINEGT;Integrated Security=True";

            if (ModelState.IsValid)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(cadena))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand("DECLINAR_VENTA", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@FACTURA", model.NumeroFactura);

                            command.ExecuteNonQuery();
                        }
                    }
                    model.Mensaje = "Venta declinada exitosamente.";
                }
                catch (SqlException ex)
                {
                    model.Mensaje = "Error al declinar la venta: " + ex.Message;
                }
            }
            return View();
        }
    }
}
