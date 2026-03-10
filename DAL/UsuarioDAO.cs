using BE;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class UsuarioDAO : DAO, IUsuarioDAO
    {
        #region "Lectura y Validación de Seguridad"

        public UsuarioBE ValidarAcceso(string email, string passwordHash)
        {
            UsuarioBE usuarioEncontrado = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
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

                        if (usuarioEncontrado != null)
                        {
                            // Al iniciar sesión es mandatorio hidratar el árbol de seguridad completo
                            CargarSeguridadUsuario(usuarioEncontrado, connection);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error técnico al validar credenciales de acceso.", ex);
                    }
                }
            }
            return usuarioEncontrado;
        }

        public List<UsuarioBE> ListarTodos()
        {
            List<UsuarioBE> lista = new List<UsuarioBE>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT Id, Nombre, Apellido, Email, IdIdioma FROM Usuarios";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lista.Add(new UsuarioBE
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    Nombre = reader["Nombre"].ToString(),
                                    Apellido = reader["Apellido"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    IdIdioma = reader["IdIdioma"] != DBNull.Value ? Convert.ToInt32(reader["IdIdioma"]) : 1
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("No se pudo recuperar la nómina de usuarios.", ex);
                    }
                }
            }
            return lista;
        }

        public void ObtenerSeguridadUsuario(UsuarioBE pUsuario)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    CargarSeguridadUsuario(pUsuario, connection);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al hidratar la estructura de seguridad del usuario.", ex);
                }
            }
        }

        private void CargarSeguridadUsuario(UsuarioBE pUsuario, SqlConnection connection)
        {
            try
            {
                PermisoDAL permisoDAL = new PermisoDAL();
                // Consultamos permisos directos y roles asignados (EsFamilia en BD)
                string query = @"SELECT p.Id, p.Nombre, p.EsFamilia 
                                 FROM Permiso p 
                                 INNER JOIN Usuario_Permiso up ON p.Id = up.IdPermiso 
                                 WHERE up.IdUsuario = @id";

                var componentes = new List<ComponenteBE>();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@id", pUsuario.Id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bool esF = Convert.ToBoolean(reader["EsFamilia"]);
                            ComponenteBE c = esF ?
                                (ComponenteBE)new CompuestoBE { Id = (int)reader["Id"], Nombre = reader["Nombre"].ToString() } :
                                (ComponenteBE)new PermisoBE { Id = (int)reader["Id"], Nombre = reader["Nombre"].ToString() };

                            componentes.Add(c);
                        }
                    }
                }

                foreach (var comp in componentes)
                {
                    // Si es un Rol (Compuesto), cargamos recursivamente sus hijos
                    if (comp is CompuestoBE)
                    {
                        permisoDAL.LlenarHijos(comp, connection);
                    }
                    pUsuario.AgregarPermiso(comp);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo en la reconstrucción del árbol Composite del usuario.", ex);
            }
        }

        #endregion

        #region "Escritura y Persistencia Atómica"

        public bool Registrar(UsuarioBE pUsuario, string pPasswordHash, int pIdRolBase)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
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
                    throw new Exception("No se pudo completar el registro del nuevo usuario.", ex);
                }
            }
        }

        public bool GuardarPermisos(UsuarioBE pUsuario)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    // 1. Limpieza de asignaciones previas
                    string queryDelete = "DELETE FROM Usuario_Permiso WHERE IdUsuario = @id";
                    using (SqlCommand cmdDel = new SqlCommand(queryDelete, connection, transaction))
                    {
                        cmdDel.Parameters.AddWithValue("@id", pUsuario.Id);
                        cmdDel.ExecuteNonQuery();
                    }

                    // 2. Inserción de la nueva estructura de seguridad
                    // Optimizamos reutilizando el objeto Command y sus parámetros
                    string queryInsert = "INSERT INTO Usuario_Permiso (IdUsuario, IdPermiso) VALUES (@idU, @idP)";
                    using (SqlCommand cmdIns = new SqlCommand(queryInsert, connection, transaction))
                    {
                        cmdIns.Parameters.Add("@idU", SqlDbType.Int);
                        cmdIns.Parameters.Add("@idP", SqlDbType.Int);
                        cmdIns.Parameters["@idU"].Value = pUsuario.Id;

                        foreach (var componente in pUsuario.Permisos)
                        {
                            cmdIns.Parameters["@idP"].Value = componente.Id;
                            cmdIns.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Error al persistir la nueva configuración de seguridad del usuario.", ex);
                }
            }
        }

        #endregion
    }
}