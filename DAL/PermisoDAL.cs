using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BE;
using DAL.Interfaces;

namespace DAL
{
    /// <summary>
    /// Gestiona la persistencia de componentes de seguridad (Permisos y Roles) en la base de datos SQL.
    /// Implementa el patrón Data Access Object para manejar la estructura Composite.
    /// </summary>
    public class PermisoDAL : DAO, IPermisoDAL
    {
        #region "Métodos de Escritura (Write)"

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

        public void GuardarEstructuraRol(CompuestoBE pRol)
        {
            using (SqlConnection cn = new SqlConnection(_connectionString))
            {
                cn.Open();
                SqlTransaction txn = cn.BeginTransaction();
                try
                {
                    foreach (var hijo in pRol.ObtenerEstructuraCompleta())
                    {
                        if (hijo.Estado == ComponenteBE.EstadoEntidad.Agregado)
                        {
                            // Ajustado a IdPadre / IdHijo según convención de nombres sin guion
                            string query = "INSERT INTO Permiso_Permiso (IdPadre, IdHijo) VALUES (@padre, @hijo)";
                            using (SqlCommand cmd = new SqlCommand(query, cn, txn))
                            {
                                cmd.Parameters.AddWithValue("@padre", pRol.Id);
                                cmd.Parameters.AddWithValue("@hijo", hijo.Id);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        else if (hijo.Estado == ComponenteBE.EstadoEntidad.Eliminado)
                        {
                            string query = "DELETE FROM Permiso_Permiso WHERE IdPadre = @padre AND IdHijo = @hijo";
                            using (SqlCommand cmd = new SqlCommand(query, cn, txn))
                            {
                                cmd.Parameters.AddWithValue("@padre", pRol.Id);
                                cmd.Parameters.AddWithValue("@hijo", hijo.Id);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    txn.Commit();
                }
                catch (Exception)
                {
                    txn.Rollback();
                    throw;
                }
            }
        }

        #endregion

        #region "Métodos de Lectura (Read)"

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

        public List<ComponenteBE> ListarTodo()
        {
            List<ComponenteBE> lista = new List<ComponenteBE>();
            using (SqlConnection cn = new SqlConnection(_connectionString))
            {
                string query = "SELECT Id, Nombre, EsFamilia FROM Permiso";
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ComponenteBE c;
                            bool esFamilia = Convert.ToBoolean(reader["EsFamilia"]);
                            if (esFamilia)
                                c = new CompuestoBE { Id = (int)reader["Id"], Nombre = reader["Nombre"].ToString() };
                            else
                                c = new PermisoBE { Id = (int)reader["Id"], Nombre = reader["Nombre"].ToString() };

                            lista.Add(c);
                        }
                    }
                }
            }
            return lista;
        }

        public List<ComponenteBE> ObtenerPermisosUsuario(int usuarioId)
        {
            List<ComponenteBE> lista = new List<ComponenteBE>();

            using (SqlConnection cn = new SqlConnection(_connectionString))
            {
                // CORRECCIÓN: Tabla 'Usuario_Permiso' y columnas 'IdPermiso', 'IdUsuario'
                string query = @"SELECT p.Id, p.Nombre, p.EsFamilia 
                                 FROM Permiso p 
                                 INNER JOIN Usuario_Permiso up ON p.Id = up.IdPermiso 
                                 WHERE up.IdUsuario = @id";

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

        private void LlenarHijos(ComponenteBE padre)
        {
            using (SqlConnection cn = new SqlConnection(_connectionString))
            {
                // CORRECCIÓN: Nombres de columnas según tu esquema Composite (IdPadre / IdHijo)
                string query = @"SELECT p.Id, p.Nombre, p.EsFamilia 
                                 FROM Permiso p 
                                 INNER JOIN Permiso_Permiso pp ON p.Id = pp.IdHijo 
                                 WHERE pp.IdPadre = @idPadre";

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

        #endregion
    }
}