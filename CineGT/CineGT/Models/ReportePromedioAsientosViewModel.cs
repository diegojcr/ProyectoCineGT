namespace CineGT.Models
{
    public class ReportePromedioAsientosViewModel
    {
        public int Mes { get; set; }
        public int Año { get; set; }
        public int Cantidad_Sesiones { get; set; }
        public decimal Promedio_Asientos_Ocupados { get; set; }
    }

    public class ReportePromedioAsientosConSalaViewModel
    {
        public int Id_Sala { get; set; }
        public List<ReportePromedioAsientosViewModel> ReportePromedioAsientos { get; set; }
    }
}
