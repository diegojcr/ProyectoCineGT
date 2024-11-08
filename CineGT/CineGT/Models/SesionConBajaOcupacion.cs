namespace CineGT.Models
{
    public class SesionConBajaOcupacion
    {
        public int Id_Funcion { get; set; }
        public DateTime Fecha_Inicio_Funcion { get; set; }
        public DateTime Fecha_Fin_Funcion { get; set; }
        public decimal Precio_Funcion { get; set; }
        public int Id_Sala { get; set; }
        public int Disponibilidad { get; set; }
        public int Asientos_Ocupados { get; set; }
        public decimal Porcentaje_Ocupacion { get; set; }
    }
}
