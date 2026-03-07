using DAL;
using BE;
using System;
using System.Security.Cryptography;
using System.Text;
using DAL.Interfaces;

namespace BL
{
    public class UsuarioBL
    {
        #region "Atributos / Constructor"

        private readonly IUsuarioDAO _usuarioDAO;

        public UsuarioBL()
        {
            _usuarioDAO = new UsuarioDAO();
        }

        public UsuarioBL(IUsuarioDAO usuarioDAO)
        {
            _usuarioDAO = usuarioDAO;
        }

        #endregion

        #region "Métodos Públicos"

        public bool LogIn(string email, string password)
        {
            // Aplicamos el Hash a la contraseña que viene de la UI
            string passwordHasheado = GenerarHash(password);

            // Usamos la interfaz para validar con el Hash
            var usuario = _usuarioDAO.ValidarAcceso(email, passwordHasheado);

            if (usuario != null)
            {
                SesionManagerBL.GetInstance().LogIn(usuario);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Registra un nuevo usuario, cifra su clave y le asigna el Rol base (ID 1).
        /// </summary>
        public bool Registrar(UsuarioBE nuevoUsuario, string password)
        {
            // 1. Cifrado de password profesional [cite: 2026-01-28]
            string passwordHasheado = GenerarHash(password);

            // 2. Definimos el Rol Base (ID 1 por defecto en el sistema) [cite: 2026-03-05]
            int idRolBase = 1;

            // 3. Persistencia mediante DAO
            return _usuarioDAO.Registrar(nuevoUsuario, passwordHasheado, idRolBase);
        }

        #endregion

        #region "Métodos Privados"

        private string GenerarHash(string texto)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(texto));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        #endregion
    }
}