namespace CineGT.Models
{
    public class Funcion
    {
        public DateTime FechaInicio { get; set; }
        public string HoraInicio { get; set; }
        public string FechaFin {  get; set; }
        public string HoraFin { get; set; }
        public double Precio { get; set; }
        public int Id_Sala { get; set; }
        public string Id_Pelicula { get; set; }
        public int Id_Estado_F {  get; set; }
        public string FilePath { get; set; }  // Ruta del archivo CSV
        public bool IgnoreErrors { get; set; }  // Ignorar errores (si es 1 se ignoran)
        public string MensajeError { get; set; }  // Mensaje de error
    }



}
