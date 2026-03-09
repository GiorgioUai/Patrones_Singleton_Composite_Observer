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
        #region "Métodos de Escritura (Alta y Baja)"

        /// <summary>
        /// Inserta un nuevo permiso simple (Patente) en la base de datos con EsFamilia = 0.
        /// </summary>
        public void GuardarPermisoSimple(PermisoBE pPermiso)
        {
            using (SqlConnection cn = new SqlConnection(_connectionString))
            {
                // Sincronizado con columna EsFamilia
                string query = "INSERT INTO Permiso (Nombre, EsFamilia) VALUES (@nombre, 0); SELECT SCOPE_IDENTITY();";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@nombre", pPermiso.Nombre);
                    cn.Open();
                    pPermiso.Id = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        /// <summary>
        /// Elimina un permiso de la tabla base. 
        /// Nota: La integridad referencial en SQL debe manejar las relaciones en tablas intermedias.
        /// </summary>
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

        /// <summary>
        /// Persiste las relaciones entre Roles y sus hijos (Permisos o Roles) en Permiso_Permiso.
        /// </summary>
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

        #region "Métodos de Lectura y Carga Composite"

        /// <summary>
        /// Lista únicamente los permisos simples (Hoja).
        /// </summary>
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

        /// <summary>
        /// Obtiene la lista completa de componentes sin cargar su jerarquía.
        /// </summary>
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
                            bool esFamilia = Convert.ToBoolean(reader["EsFamilia"]);
                            ComponenteBE c = esFamilia ?
                                (ComponenteBE)new CompuestoBE { Id = (int)reader["Id"], Nombre = reader["Nombre"].ToString() } :
                                (ComponenteBE)new PermisoBE { Id = (int)reader["Id"], Nombre = reader["Nombre"].ToString() };

                            lista.Add(c);
                        }
                    }
                }
            }
            return lista;
        }

        /// <summary>
        /// Recupera los componentes asignados a un usuario y dispara la carga recursiva de hijos.
        /// </summary>
        public List<ComponenteBE> ObtenerPermisosUsuario(int usuarioId)
        {
            List<ComponenteBE> lista = new List<ComponenteBE>();
            using (SqlConnection cn = new SqlConnection(_connectionString))
            {
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
                            int id = Convert.ToInt32(reader["Id"]);
                            string nombre = reader["Nombre"].ToString();
                            bool esFamilia = Convert.ToBoolean(reader["EsFamilia"]);

                            ComponenteBE componente = esFamilia ?
                                (ComponenteBE)new CompuestoBE { Id = id, Nombre = nombre } :
                                (ComponenteBE)new PermisoBE { Id = id, Nombre = nombre };

                            if (esFamilia) LlenarHijos(componente);

                            lista.Add(componente);
                        }
                    }
                }
            }
            return lista;
        }

        /// <summary>
        /// Método recursivo para reconstruir el árbol de Roles y Permisos.
        /// </summary>
        private void LlenarHijos(ComponenteBE padre)
        {
            using (SqlConnection cn = new SqlConnection(_connectionString))
            {
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
                            int id = Convert.ToInt32(reader["Id"]);
                            string nombre = reader["Nombre"].ToString();
                            bool esFamilia = Convert.ToBoolean(reader["EsFamilia"]);

                            ComponenteBE hijo = esFamilia ?
                                (ComponenteBE)new CompuestoBE { Id = id, Nombre = nombre } :
                                (ComponenteBE)new PermisoBE { Id = id, Nombre = nombre };

                            if (esFamilia) LlenarHijos(hijo);

                            padre.AgregarHijo(hijo);
                        }
                    }
                }
            }
        }

        #endregion
    }
}