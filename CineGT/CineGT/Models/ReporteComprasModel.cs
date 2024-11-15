namespace CineGT.Models
{
    public class ReporteComprasModel
    {
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public List<ReporteCompraInfo> ReporteCompras { get; set; }
    }

    public class ReporteCompraInfo
    {
        public string NumeroFactura { get; set; }
        public decimal MontoTotal { get; set; }
        public DateTime FechaCompra { get; set; }
        public int IdSala { get; set; }
        public decimal PrecioUnitario { get; set; }
        public DateTime FechaInicioFuncion { get; set; }
        public DateTime FechaFinFuncion { get; set; }
        public int AsientosOcupados { get; set; }
        public string EstadoFuncion { get; set; }
        public string NombrePelicula { get; set; }
        public string DescripcionPelicula { get; set; }
        public string Clasificacion { get; set; }
        public string TipoSala { get; set; }
        public int CapacidadSala { get; set; }
    }
}
