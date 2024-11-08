namespace CineGT.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string User {  get; set; }
        public string Clave { get; set; }
        public string ConfirmarClave { get; set; }
        public int Numero { get; set; }
        public string Correo { get; set; }
        public string Nombre { get; set; } 
        public string Apellido { get; set; }
        public bool IdTipoUsuario { get; set; }
    }
}
