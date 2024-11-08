namespace CineGT.Models
{
    public class Boleto
    {
        public int NumeroBoletos { get; set; }
        public string Pelicula { get; set; }
        public DateTime FechaFuncion { get; set; }
        public int Sala { get; set; }
        public int TipoPago { get; set; }
        public int ClienteId { get; set; }
        public int VendedorId { get; set; }
        public int EstadoCompra { get; set; } // Estado de la compra
        public List<AsientoSeleccionado> AsientosSeleccionados { get; set; }
    }

    public class AsientoSeleccionado
    {
        public string Fila { get; set; }
        public string Columna { get; set; }
    }
}
