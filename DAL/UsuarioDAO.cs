using BE;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    /// <summary>
    /// Clase de Acceso a Datos para la entidad Usuario.
    /// Implementa la persistencia y carga de seguridad (Composite).
    /// </summary>
    public class UsuarioDAO : DAO, IUsuarioDAO
    {
        #region "Métodos Públicos"

        public UsuarioBE ValidarAcceso(string email, string passwordHash)
        {
            UsuarioBE usuarioEncontrado = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {                
                string query = "SELECT Id, Nombre, Apellido, Email, IdIdioma FROM Usuarios WHERE Email = @email AND Password = @pass";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@email", email);
                    // Sincronizamos el nombre del parámetro con la query (@pass)
                    command.Parameters.AddWithValue("@pass", passwordHash);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Instanciamos la entidad de la capa BE
                                usuarioEncontrado = new UsuarioBE
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    Nombre = reader["Nombre"].ToString(),
                                    Apellido = reader["Apellido"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    // Leemos el idioma preferido (Default 1 si es NULL)
                                    IdIdioma = reader["IdIdioma"] != DBNull.Value ? Convert.ToInt32(reader["IdIdioma"]) : 1
                                };
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error crítico en DAL: No se pudo validar el acceso.", ex);
                    }
                }
            }

            // Si el usuario es válido, disparamos la carga de sus Roles/Patentes
            if (usuarioEncontrado != null)
            {
                CargarSeguridadUsuario(usuarioEncontrado);
            }

            return usuarioEncontrado;
        }

        #endregion

        #region "Métodos Privados de Soporte (Seguridad)"

        /// <summary>
        /// Carga de forma recursiva la estructura de permisos del usuario.
        /// </summary>
        private void CargarSeguridadUsuario(UsuarioBE pUsuario)
        {
            try
            {
                // Usamos la DAL de permisos para obtener la estructura Composite
                PermisoDAL permisoDAL = new PermisoDAL();
                var componentes = permisoDAL.ObtenerPermisosUsuario(pUsuario.Id);

                foreach (var comp in componentes)
                {
                    pUsuario.AgregarPermiso(comp);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cargar la estructura de Roles del usuario.", ex);
            }
        }

        #endregion
    }
}