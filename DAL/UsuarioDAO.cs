using BE;
using System;
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
                // Mantenemos la consulta limpia sin el campo 'Rol'
                string query = "SELECT Id, Nombre, Apellido, Email FROM Usuarios WHERE Email = @email AND Password = @password";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@password", passwordHash);

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
                                    Email = reader["Email"].ToString()
                                };
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error al validar el acceso en la base de datos.", ex);
                    }
                }
            }

            // PASO A PASO: Si el usuario existe, completamos su seguridad
            if (usuarioEncontrado != null)
            {
                CargarSeguridadUsuario(usuarioEncontrado);
            }

            return usuarioEncontrado;
        }

        // Método privado para no ensuciar la lógica de ValidarAcceso
        private void CargarSeguridadUsuario(UsuarioBE pUsuario)
        {
            try
            {
                // Instanciamos la DAL de permisos (o la inyectamos si prefieres)
                PermisoDAL permisoDAL = new PermisoDAL();

                // Obtenemos todos los componentes (Simples y Compuestos)
                var componentes = permisoDAL.ObtenerPermisosUsuario(pUsuario.Id);

                foreach (var comp in componentes)
                {
                    pUsuario.AgregarPermiso(comp);
                }
            }
            catch (Exception ex)
            {
                // Si falla la carga de permisos, el usuario no debería poder operar
                throw new Exception("Error al cargar la configuración de seguridad del usuario.", ex);
            }
        }
    }
}