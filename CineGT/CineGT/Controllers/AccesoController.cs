using CineGT.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace CineGT.Controllers
{
    public class AccesoController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Registrar()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Registrar(Usuario oUsuario)
        {
            string cadena = "Data Source=DIEGO\\SQLEXPRESS;Initial Catalog=PROYECTO_CINEGT;Integrated Security=True";
            
            if(oUsuario.Clave != oUsuario.ConfirmarClave)
            {
                ViewData["Mensaje"] = "Las contraseñas no coinciden";
                return View();
            }
            try
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_NUEVOUSUARIO", cn))
                    {
                        cmd.Parameters.AddWithValue("Usuario", oUsuario.User);
                        cmd.Parameters.AddWithValue("Numero", oUsuario.Numero);
                        cmd.Parameters.AddWithValue("Correo", oUsuario.Correo);
                        cmd.Parameters.AddWithValue("Password", oUsuario.Clave);
                        cmd.Parameters.AddWithValue("Nombre", oUsuario.Nombre);
                        cmd.Parameters.AddWithValue("Apellido", oUsuario.Apellido);
                        cmd.Parameters.AddWithValue("TIPO_USUARIO", oUsuario.TipoUsuario);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cn.Open();

                        cmd.ExecuteNonQuery();
                    }                    
                }
                ViewData["Mensaje"] = "Usuario registrado correctamente";
                return RedirectToAction("Login", "Acceso");
            }
            catch (SqlException ex)
            {
                foreach(SqlError error in ex.Errors)
                {
                    ModelState.AddModelError("", $"Numero de error: {error.Number} - {error.Message}");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Ocurrio un error: {ex.Message}");
            }
            return View();

            
        }

        [HttpPost]
        public IActionResult Login(Usuario oUsuario)
        {
            string user = oUsuario.User;
            string clave = oUsuario.Clave;
            string connectionString = $"Server=DIEGO\\SQLEXPRESS;Database=PROYECTO_CINEGT;User Id={user};Password={clave};";
            string cadena = "Data Source=DIEGO\\SQLEXPRESS;Initial Catalog=PROYECTO_CINEGT;Integrated Security=True";
            if (oUsuario.IdTipoUsuario == true)
            {
                try
                {
                    using (SqlConnection cn = new SqlConnection(connectionString))
                    {
                        cn.Open();
                        HttpContext.Session.SetString("Usuario", user);
                        return RedirectToAction("Index", "Home");
                    }
                }
                catch (SqlException)
                {
                    ViewData["Mensaje"] = "Usuario o contraseña incorrectos.";
                    return View();
                }
            } else
            {
                //clave = ConvertirSha256(clave);

                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    SqlCommand sqlCommand = new SqlCommand("sp_ValidarUsuario", cn);
                    sqlCommand.Parameters.AddWithValue("Usuario", user);
                    sqlCommand.Parameters.AddWithValue("contra", clave);
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    cn.Open();

                    oUsuario.IdUsuario = Convert.ToInt32(sqlCommand.ExecuteScalar().ToString());
                }
                if(oUsuario.IdUsuario != 0)
                {
                    HttpContext.Session.SetString("Usuario", user);
                    return RedirectToAction("IndexUsuario", "Home");
                }
                else
                {
                    ViewData["Mensaje"] = "Usuario no encontrado";
                    return View();
                }
            }

        }

        public static string ConvertirSha256(string texto)
        {
            StringBuilder sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));

                foreach (byte b in result)
                {
                    sb.Append(b.ToString("x2"));
                }
            }
            return sb.ToString();
        }

    }
}
