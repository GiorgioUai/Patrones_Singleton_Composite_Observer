using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BE;

namespace DAL
{
    public class PermisoDAL : DAO, IPermisoDAL
    {
        public void GuardarPermisoSimple(PermisoBE pPermiso)
        {
            using (SqlConnection cn = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Permiso (Nombre, EsFamilia) VALUES (@nombre, 0); SELECT SCOPE_IDENTITY();";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@nombre", pPermiso.Nombre);
                    cn.Open();
                    pPermiso.Id = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public List<PermisoBE> ListarSimples()
        {
            List<PermisoBE> lista = new List<PermisoBE>();

            using (SqlConnection cn = new SqlConnection(_connectionString))
            {
                string query = "SELECT Id, Nombre FROM Permiso WHERE EsFamilia = 0";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new PermisoBE
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Nombre = reader["Nombre"].ToString()
                            });
                        }
                    }
                }
            }
            return lista;
        }

        public void EliminarPermiso(PermisoBE pPermiso)
        {
            using (SqlConnection cn = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Permiso WHERE Id = @id";
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@id", pPermiso.Id);
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // IMPLEMENTACIÓN PARA RESOLVER EL ERROR EN USUARIODAO
        public List<ComponenteBE> ObtenerPermisosUsuario(int usuarioId)
        {
            List<ComponenteBE> lista = new List<ComponenteBE>();

            using (SqlConnection cn = new SqlConnection(_connectionString))
            {
                // Obtenemos los componentes raíz asignados al usuario
                string query = @"SELECT p.Id, p.Nombre, p.EsFamilia 
                                 FROM Permiso p 
                                 INNER JOIN Usuarios_Permisos up ON p.Id = up.Id_Permiso 
                                 WHERE up.Id_Usuario = @id";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@id", usuarioId);
                    cn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ComponenteBE componente;
                            int id = Convert.ToInt32(reader["Id"]);
                            string nombre = reader["Nombre"].ToString();
                            bool esFamilia = Convert.ToBoolean(reader["EsFamilia"]);

                            if (esFamilia)
                            {
                                componente = new CompuestoBE { Id = id, Nombre = nombre };
                                // Llamada recursiva para cargar sus hijos
                                LlenarHijos(componente);
                            }
                            else
                            {
                                componente = new PermisoBE { Id = id, Nombre = nombre };
                            }

                            lista.Add(componente);
                        }
                    }
                }
            }
            return lista;
        }

        // Método recursivo para navegar la jerarquía Permiso_Permiso
        private void LlenarHijos(ComponenteBE padre)
        {
            using (SqlConnection cn = new SqlConnection(_connectionString))
            {
                string query = @"SELECT p.Id, p.Nombre, p.EsFamilia 
                                 FROM Permiso p 
                                 INNER JOIN Permiso_Permiso pp ON p.Id = pp.Id_Hijo 
                                 WHERE pp.Id_Padre = @idPadre";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@idPadre", padre.Id);
                    cn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ComponenteBE hijo;
                            int id = Convert.ToInt32(reader["Id"]);
                            string nombre = reader["Nombre"].ToString();
                            bool esFamilia = Convert.ToBoolean(reader["EsFamilia"]);

                            if (esFamilia)
                            {
                                hijo = new CompuestoBE { Id = id, Nombre = nombre };
                                // Si el hijo es compuesto, seguimos bajando niveles
                                LlenarHijos(hijo);
                            }
                            else
                            {
                                hijo = new PermisoBE { Id = id, Nombre = nombre };
                            }

                            padre.AgregarHijo(hijo);
                        }
                    }
                }
            }
        }
    }
}