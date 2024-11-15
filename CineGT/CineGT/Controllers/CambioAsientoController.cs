using CineGT.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace CineGT.Controllers
{
    public class CambioAsientoController : Controller
    {
        public static string cadena = "Data Source=DIEGO\\SQLEXPRESS;Initial Catalog=PROYECTO_CINEGT;Integrated Security=True";

        


        // Acción para mostrar la vista de cambio de asiento
        public ActionResult CambiarAsiento()
        {
            var modelo = new CambioAsientoViewModel();
            return View(modelo);
        }

        [HttpPost]
        public ActionResult CambiarAsiento(CambioAsientoViewModel modelo, string actionType)
        {
            if (actionType == "Buscar")
            {
                // Buscar los boletos asociados con la factura
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_VERBOLETOSCLIENTE", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FACTURA", modelo.NumeroFactura);

                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            List<AsientoInfo> asientos = new List<AsientoInfo>();
                            while (reader.Read())
                            {
                                var asiento = new AsientoInfo
                                {
                                    NombrePelicula = reader["Nombre"].ToString(),
                                    FechaInicio = Convert.ToDateTime(reader["Fecha_Inicio"]),
                                    Fila = Convert.ToInt32(reader["Fila"]),
                                    Columna = reader["Columna"].ToString(),
                                    IdSala = Convert.ToInt32(reader["Id_Sala"])
                                };
                                asientos.Add(asiento);
                            }

                            if (asientos.Count > 0)
                            {
                                modelo.Asientos = asientos;
                            }
                            else
                            {
                                ViewBag.Error = "No se encontraron boletos para la factura ingresada.";
                            }
                        }
                    }
                }
            }
            else if (actionType == "Cambiar")
            {
                // Lógica para actualizar el asiento en la base de datos
                try
                {
                    using (SqlConnection con = new SqlConnection(cadena))
                    {
                        using (SqlCommand cmd = new SqlCommand("SP_CAMBIARASIENTO", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@FACTURA", modelo.NumeroFactura);
                            cmd.Parameters.AddWithValue("@FILA_ANTERIOR", modelo.FilaAnterior);
                            cmd.Parameters.AddWithValue("@COLUMNA_ANTERIOR", modelo.ColumnaAnterior);
                            cmd.Parameters.AddWithValue("@PELICULA_ANTERIOR", modelo.PeliculaAnterior);
                            cmd.Parameters.AddWithValue("@SALA_ANTERIOR", modelo.SalaAnterior);
                            cmd.Parameters.AddWithValue("@FECHA_ANTERIOR", modelo.FechaAnterior);
                            cmd.Parameters.AddWithValue("@FILA_NUEVA", modelo.FilaNueva);
                            cmd.Parameters.AddWithValue("@COLUMNA_NUEVA", modelo.ColumnaNueva);
                            cmd.Parameters.AddWithValue("@PELICULA", modelo.Pelicula);
                            cmd.Parameters.AddWithValue("@ID_SALA", modelo.IdSala);
                            cmd.Parameters.AddWithValue("@FECHA_INICIO", modelo.FechaInicio);

                            con.Open();
                            cmd.ExecuteNonQuery();
                            ViewBag.Message = "Asiento cambiado exitosamente.";
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Error al cambiar el asiento: " + ex.Message;
                }
            }
            return View(modelo);
        }
    }


}
