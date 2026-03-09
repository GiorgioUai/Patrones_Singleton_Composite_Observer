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

                        // Si el usuario existe, cargamos sus permisos usando la misma conexión abierta
                        if (usuarioEncontrado != null)
                        {
                            CargarSeguridadUsuario(usuarioEncontrado, connection);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error crítico en el proceso de Login.", ex);
                    }
                }
            }

            return usuarioEncontrado;
        }

        private void CargarSeguridadUsuario(UsuarioBE pUsuario, SqlConnection connection)
        {
            try
            {
                // Usamos la lógica de PermisoDAL inyectando la conexión compartida
                PermisoDAL permisoDAL = new PermisoDAL();

                // Obtenemos los roles/permisos directos del usuario
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
                            ComponenteBE c = esF ? (ComponenteBE)new CompuestoBE { Id = (int)reader["Id"], Nombre = reader["Nombre"].ToString() } :
                                                   (ComponenteBE)new PermisoBE { Id = (int)reader["Id"], Nombre = reader["Nombre"].ToString() };
                            componentes.Add(c);
                        }
                    }
                }

                // Disparamos la recursividad para cada componente compuesto
                foreach (var comp in componentes)
                {
                    if (comp is CompuestoBE)
                    {
                        permisoDAL.LlenarHijos(comp, connection);
                    }
                    pUsuario.AgregarPermiso(comp);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al reconstruir el árbol de seguridad del usuario.", ex);
            }
        }

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
                    throw new Exception("Error técnico en el registro de usuario.", ex);
                }
            }
        }
    }
}