namespace CineGT.Models
{

    public class ReporteTransaccionesConFechasViewModel
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public List<ReporteTransaccionesPorFecha> ReporteTransacciones { get; set; }
    }
    public class ReporteTransaccionesPorFecha
    {
        public DateTime FechaTransaccion { get; set; }
        public string DescripcionTransaccion { get; set; }
        public int IdUsuarioTransaccion { get; set; }
        public int? IdCompra { get; set; }
        public string NumeroFactura { get; set; }
        public decimal? MontoTotal { get; set; }
        public int? IdCliente { get; set; }
        public string EstadoCompra { get; set; }
        public int? TotalBoletos { get; set; }
        public DateTime? FechaFuncion { get; set; }
        public string NombrePelicula { get; set; }
        public decimal? PrecioFuncion { get; set; }
    }
}
