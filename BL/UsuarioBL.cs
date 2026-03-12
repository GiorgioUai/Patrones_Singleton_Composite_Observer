using DAL;
using BE;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using DAL.Interfaces;

namespace BL
{
    public class UsuarioBL
    {
        #region "Atributos y Constructor"

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

        #region "Lógica de Negocio de Usuarios"

        /// <summary>
        /// Valida las credenciales y gestiona el inicio de sesión.
        /// </summary>
        /// <returns>El usuario encontrado. La UI debe validar el flag DebeCambiarPassword.</returns>
        public UsuarioBE LogIn(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password)) return null;

            string passwordHasheado = GenerarHash(password);
            var usuario = _usuarioDAO.ValidarAcceso(email, passwordHasheado);

            // SEGURIDAD: Solo iniciamos sesión formal si NO debe cambiar password.
            if (usuario != null && !usuario.DebeCambiarPassword)
            {
                SesionManagerBL.GetInstance().LogIn(usuario);
            }

            return usuario; // Retornamos el usuario para que la UI actúe según el flag.
        }

        /// <summary>
        /// Realiza el cambio de contraseña solicitado por el usuario.
        /// </summary>
        public bool CambiarPassword(UsuarioBE pUsuario, string nuevaPassword)
        {
            if (pUsuario == null || string.IsNullOrWhiteSpace(nuevaPassword)) return false;

            string passwordHasheado = GenerarHash(nuevaPassword);
            bool exito = _usuarioDAO.ActualizarPassword(pUsuario.Id, passwordHasheado);

            if (exito) pUsuario.DebeCambiarPassword = false;
            return exito;
        }

        /// <summary>
        /// Fuerza el cambio de contraseña (Blanqueo) para un usuario.
        /// </summary>
        public bool ForzarCambioPassword(int idUsuario)
        {
            return _usuarioDAO.ForzarCambioPassword(idUsuario);
        }

        public List<UsuarioBE> ListarTodos()
        {
            return _usuarioDAO.ListarTodos();
        }

        public void CargarSeguridad(UsuarioBE pUsuario)
        {
            if (pUsuario == null) throw new ArgumentNullException(nameof(pUsuario));
            _usuarioDAO.ObtenerSeguridadUsuario(pUsuario);
        }

        public bool Registrar(UsuarioBE nuevoUsuario, string password)
        {
            string passwordHasheado = GenerarHash(password);
            const int idRolBase = 1;

            // QA: Por defecto, los nuevos usuarios podrían ser obligados a cambiar clave
            // dependiendo de la política comercial/seguridad.
            return _usuarioDAO.Registrar(nuevoUsuario, passwordHasheado, idRolBase);
        }

        public bool GuardarPermisos(UsuarioBE pUsuario)
        {
            if (pUsuario == null) throw new ArgumentNullException(nameof(pUsuario));
            return _usuarioDAO.GuardarPermisos(pUsuario);
        }

        #endregion

        #region "Seguridad Criptográfica"

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