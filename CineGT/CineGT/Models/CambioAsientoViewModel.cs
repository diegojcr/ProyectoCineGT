namespace CineGT.Models
{
    public class CambioAsientoViewModel
    {
        public string NumeroFactura { get; set; }
        public List<AsientoInfo> Asientos { get; set; }
        public bool BoletoEncontrado { get; set; }

        // Datos del asiento a cambiar
        public int FilaAnterior { get; set; }
        public string ColumnaAnterior { get; set; }
        public string PeliculaAnterior { get; set; }
        public int SalaAnterior { get; set; }
        public DateTime FechaAnterior { get; set; }

        public int FilaNueva { get; set; }
        public string ColumnaNueva { get; set; }
        public string Pelicula { get; set; }
        public int IdSala { get; set; }
        public DateTime FechaInicio { get; set; }
    }

    public class AsientoInfo
    {
        public string NombrePelicula { get; set; }
        public DateTime FechaInicio { get; set; }
        public int Fila { get; set; }
        public string Columna { get; set; }
        public int IdSala { get; set; }
    }
}
