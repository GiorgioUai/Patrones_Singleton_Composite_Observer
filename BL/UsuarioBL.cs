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

        public bool LogIn(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password)) return false;

            string passwordHasheado = GenerarHash(password);
            var usuario = _usuarioDAO.ValidarAcceso(email, passwordHasheado);

            if (usuario != null)
            {
                SesionManagerBL.GetInstance().LogIn(usuario);
                return true;
            }
            return false;
        }

        public List<UsuarioBE> ListarTodos()
        {
            return _usuarioDAO.ListarTodos();
        }

        /// <summary>
        /// Hidrata el objeto usuario con sus Roles y Permisos asignados.
        /// Aplica Lazy Loading para optimizar el rendimiento de la aplicación.
        /// </summary>
        public void CargarSeguridad(UsuarioBE pUsuario)
        {
            if (pUsuario == null) throw new ArgumentNullException(nameof(pUsuario));
            _usuarioDAO.ObtenerSeguridadUsuario(pUsuario);
        }

        /// <summary>
        /// Gestiona el registro de nuevos usuarios aplicando políticas de seguridad (Hashing).
        /// </summary>
        public bool Registrar(UsuarioBE nuevoUsuario, string password)
        {
            string passwordHasheado = GenerarHash(password);

            // Se asigna por defecto el Rol base definido en la arquitectura del sistema
            const int idRolBase = 1;

            return _usuarioDAO.Registrar(nuevoUsuario, passwordHasheado, idRolBase);
        }

        /// <summary>
        /// Solicita la persistencia de los cambios realizados en el árbol de permisos del usuario.
        /// </summary>
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