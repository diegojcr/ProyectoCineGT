﻿using CineGT.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace CineGT.Controllers
{
    public class ReportesController : Controller
    {

        public static string cadena = "Data Source=DIEGO\\SQLEXPRESS;Initial Catalog=PROYECTO_CINEGT;Integrated Security=True";
        [HttpGet]
        public IActionResult ObtenerFuncionesConAsientosOcupados()
        {
            // Inicializar una lista vacía para la primera carga de la vista
            var modelo = new ObtenerFuncionesConAsientosOcupados
            {
                Fecha_Inicio_Funcion = DateTime.Today,
                Fecha_Fin_Funcion = DateTime.Today,
                ReporteFunciones = new List<ObtenerFuncionesConAsientosOcupados>()
            };

            return View(modelo);
        }
        [HttpPost]
        public ActionResult ObtenerFuncionesConAsientosOcupados(ObtenerFuncionesConAsientosOcupados modelo)
        {
            // Validar fechas
            if (modelo.Fecha_Inicio_Funcion > modelo.Fecha_Fin_Funcion)
            {
                ModelState.AddModelError("", "La fecha de inicio no puede ser mayor que la fecha de fin.");
                return View(modelo);
            }

            modelo.ReporteFunciones = new List<ObtenerFuncionesConAsientosOcupados>();

            try
            {
                using (SqlConnection connection = new SqlConnection(cadena))
                {
                    using (SqlCommand command = new SqlCommand("ObtenerFuncionesConAsientosOcupados", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@FechaInicio", modelo.Fecha_Inicio_Funcion);
                        command.Parameters.AddWithValue("@FechaFin", modelo.Fecha_Fin_Funcion);

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var reporte = new ObtenerFuncionesConAsientosOcupados
                                {
                                    Id_Funcion = Convert.ToInt32(reader["Id_Funcion"]),
                                    Fecha_Inicio_Funcion = Convert.ToDateTime(reader["Fecha_Inicio_Funcion"]),
                                    Fecha_Fin_Funcion = Convert.ToDateTime(reader["Fecha_Fin_Funcion"]),
                                    Precio_Funcion = Convert.ToDecimal(reader["Precio_Funcion"]),
                                    Id_Sala = Convert.ToInt32(reader["Id_Sala"]),
                                    Disponibilidad = Convert.ToInt32(reader["Disponibilidad"]),
                                    Nombre_Pelicula = reader["Nombre_Pelicula"].ToString(),
                                    Clasificacion_Pelicula = reader["Clasificacion_Pelicula"].ToString(),
                                    Asientos_Ocupados = Convert.ToInt32(reader["Asientos_Ocupados"])
                                };
                                modelo.ReporteFunciones.Add(reporte);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al obtener el reporte: {ex.Message}");
            }

            return View(modelo);
        }

        [HttpGet]
        public ActionResult ObtenerPromedioAsientosOcupadosYSesionesPorMes()
        {
            // Inicializar el modelo con lista vacía
            var modelo = new ReportePromedioAsientosConSalaViewModel
            {
                Id_Sala = 0,
                ReportePromedioAsientos = new List<ReportePromedioAsientosViewModel>()
            };
            return View(modelo);
        }

        [HttpPost]
        public ActionResult ObtenerPromedioAsientosOcupadosYSesionesPorMes(ReportePromedioAsientosConSalaViewModel modelo)
        {
            // Validar que el Id_Sala sea mayor que 0
            if (modelo.Id_Sala <= 0)
            {
                ModelState.AddModelError("", "El ID de la sala debe ser un número positivo.");
                return View(modelo);
            }

            modelo.ReportePromedioAsientos = new List<ReportePromedioAsientosViewModel>();

            try
            {
                using (SqlConnection connection = new SqlConnection(cadena))
                {
                    using (SqlCommand command = new SqlCommand("ObtenerPromedioAsientosOcupadosYSesionesPorMes", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id_Sala", modelo.Id_Sala);

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var reporte = new ReportePromedioAsientosViewModel
                                {
                                    Mes = Convert.ToInt32(reader["Mes"]),
                                    Año = Convert.ToInt32(reader["Año"]),
                                    Cantidad_Sesiones = Convert.ToInt32(reader["Cantidad_Sesiones"]),
                                    Promedio_Asientos_Ocupados = Convert.ToDecimal(reader["Promedio_Asientos_Ocupados"])
                                };
                                modelo.ReportePromedioAsientos.Add(reporte);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al obtener el reporte: {ex.Message}");
            }

            return View(modelo);
        }

        [HttpGet]
        public ActionResult ObtenerSesionesConAsientosOcupadosPorDebajoDelPorcentaje()
        {
            // Inicializar el modelo con lista vacía
            var modelo = new ReporteOcupacionConPorcentajeViewModel
            {
                PorcentajeLimite = 0,
                ReporteOcupacion = new List<ReporteOcupacionViewModel>()
            };
            return View(modelo);
        }

        [HttpPost]
        public ActionResult ObtenerSesionesConAsientosOcupadosPorDebajoDelPorcentaje(ReporteOcupacionConPorcentajeViewModel modelo)
        {
            // Validar que el porcentaje límite sea mayor o igual a 0 y menor o igual a 100
            if (modelo.PorcentajeLimite < 0 || modelo.PorcentajeLimite > 100)
            {
                ModelState.AddModelError("", "El porcentaje límite debe estar entre 0 y 100.");
                return View(modelo);
            }

            modelo.ReporteOcupacion = new List<ReporteOcupacionViewModel>();

            try
            {
                using (SqlConnection connection = new SqlConnection(cadena))
                {
                    using (SqlCommand command = new SqlCommand("ObtenerSesionesConAsientosOcupadosPorDebajoDelPorcentaje", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@PorcentajeLimite", modelo.PorcentajeLimite);

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var reporte = new ReporteOcupacionViewModel
                                {
                                    Id_Funcion = Convert.ToInt32(reader["Id_Funcion"]),
                                    Fecha_Inicio_Funcion = Convert.ToDateTime(reader["Fecha_Inicio_Funcion"]),
                                    Fecha_Fin_Funcion = Convert.ToDateTime(reader["Fecha_Fin_Funcion"]),
                                    Precio_Funcion = Convert.ToDecimal(reader["Precio_Funcion"]),
                                    Id_Sala = Convert.ToInt32(reader["Id_Sala"]),
                                    Disponibilidad = Convert.ToInt32(reader["Disponibilidad"]),
                                    Asientos_Ocupados = Convert.ToInt32(reader["Asientos_Ocupados"]),
                                    Porcentaje_Ocupacion = Convert.ToDecimal(reader["Porcentaje_Ocupacion"])
                                };
                                modelo.ReporteOcupacion.Add(reporte);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al obtener el reporte: {ex.Message}");
            }

            return View(modelo);
        }

        [HttpGet]
        public ActionResult ObtenerTop5PeliculasPorPromedioAsientosVendidos()
        {
            List<Top5PeliculasViewModel> modelo = new List<Top5PeliculasViewModel>();

            try
            {
                using (SqlConnection connection = new SqlConnection(cadena))
                {
                    using (SqlCommand command = new SqlCommand("ObtenerTop5PeliculasPorPromedioAsientosVendidos", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var reporte = new Top5PeliculasViewModel
                                {
                                    Id_Pelicula = Convert.ToInt32(reader["Id_Pelicula"]),
                                    Nombre_Pelicula = reader["Nombre_Pelicula"].ToString(),
                                    Promedio_Asientos_Vendidos = Convert.ToDecimal(reader["Promedio_Asientos_Vendidos"])
                                };
                                modelo.Add(reporte);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al obtener el reporte: {ex.Message}");
            }

            return View(modelo);
        }

    }
}
