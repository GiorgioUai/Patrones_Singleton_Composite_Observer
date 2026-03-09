using BE;
using DAL.Interfaces;
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
                // Sincronizado con tabla Usuarios y columnas normalizadas
                string query = "SELECT Id, Nombre, Apellido, Email, IdIdioma FROM Usuarios WHERE Email = @email AND Password = @pass";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@pass", passwordHash);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                usuarioEncontrado = new UsuarioBE
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    Nombre = reader["Nombre"].ToString(),
                                    Apellido = reader["Apellido"].ToString(),
                                    Email = reader["Email"].ToString(),
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

            if (usuarioEncontrado != null)
            {
                CargarSeguridadUsuario(usuarioEncontrado);
            }

            return usuarioEncontrado;
        }

        /// <summary>
        /// Inserta un usuario y su relación con el Rol Base en una transacción atómica.
        /// </summary>
        public bool Registrar(UsuarioBE pUsuario, string pPasswordHash, int pIdRolBase)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    // 1. Insertar el Usuario y obtener su ID generado                    
                    string queryUser = @"INSERT INTO Usuarios (Nombre, Apellido, Email, Password, IdIdioma) 
                                       VALUES (@nom, @ape, @email, @pass, @idioma); 
                                       SELECT SCOPE_IDENTITY();";

                    SqlCommand cmdUser = new SqlCommand(queryUser, connection, transaction);
                    cmdUser.Parameters.AddWithValue("@nom", pUsuario.Nombre);
                    cmdUser.Parameters.AddWithValue("@ape", pUsuario.Apellido);
                    cmdUser.Parameters.AddWithValue("@email", pUsuario.Email);
                    cmdUser.Parameters.AddWithValue("@pass", pPasswordHash);
                    cmdUser.Parameters.AddWithValue("@idioma", pUsuario.IdIdioma);

                    int nuevoIdUsuario = Convert.ToInt32(cmdUser.ExecuteScalar());

                    // 2. Asignar el Rol Base en la tabla intermedia (Sin guion bajo en tabla)
                    string queryRol = "INSERT INTO Usuario_Permiso (IdUsuario, IdPermiso) VALUES (@idU, @idR)";
                    SqlCommand cmdRol = new SqlCommand(queryRol, connection, transaction);
                    cmdRol.Parameters.AddWithValue("@idU", nuevoIdUsuario);
                    cmdRol.Parameters.AddWithValue("@idR", pIdRolBase);

                    cmdRol.ExecuteNonQuery();

                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    string errorDetallado = ex.Message;
                    if (ex.InnerException != null) errorDetallado += " -> " + ex.InnerException.Message;

                    throw new Exception("Error técnico SQL: " + errorDetallado, ex);
                }
            }
        }

        #endregion

        #region "Métodos Privados de Soporte (Seguridad)"

        private void CargarSeguridadUsuario(UsuarioBE pUsuario)
        {
            try
            {
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