namespace CineGT.Models
{
    public class LogSesionesModel
    {
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public List<LogSesionInfo> LogSesiones { get; set; }
    }

    public class LogSesionInfo
    {
        public int IdLogSesion { get; set; }
        public string DescripcionLog { get; set; }
        public DateTime FechaLog { get; set; }
        public int IdUsuarioLog { get; set; }
        public int IdPelicula { get; set; }
        public string NombrePelicula { get; set; }
        public string DuracionPelicula { get; set; }
        public string ClasificacionPelicula { get; set; }
        public string DescripcionPelicula { get; set; }
        public int IdFuncion { get; set; }
        public DateTime FechaInicioFuncion { get; set; }
        public DateTime FechaFinFuncion { get; set; }
        public decimal PrecioFuncion { get; set; }
        public int SalaFuncion { get; set; }
        public int EstadoFuncion { get; set; }
        public int DisponibilidadFuncion { get; set; }
        public int CapacidadSala { get; set; }
    }
}
