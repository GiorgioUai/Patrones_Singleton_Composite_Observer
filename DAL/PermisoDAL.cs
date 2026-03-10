using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BE;
using DAL.Interfaces;

namespace DAL
{
    public class PermisoDAL : DAO, IPermisoDAL
    {
        #region "Métodos de Escritura y Persistencia"

        /// <summary>
        /// Persiste un permiso simple (Patente) en la base de datos.
        /// </summary>
        public void GuardarPermisoSimple(PermisoBE pPermiso)
        {
            using (SqlConnection cn = new SqlConnection(_connectionString))
            {
                string query = pPermiso.Id == 0 ?
                    "INSERT INTO Permiso (Nombre, EsFamilia) VALUES (@nombre, 0); SELECT SCOPE_IDENTITY();" :
                    "UPDATE Permiso SET Nombre = @nombre WHERE Id = @id";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@nombre", pPermiso.Nombre);
                    if (pPermiso.Id > 0) cmd.Parameters.AddWithValue("@id", pPermiso.Id);

                    cn.Open();
                    if (pPermiso.Id == 0) pPermiso.Id = Convert.ToInt32(cmd.ExecuteScalar());
                    else cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Elimina físicamente un componente de la base de datos.
        /// </summary>
        public void EliminarPermiso(ComponenteBE pComponente)
        {
            using (SqlConnection cn = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Permiso WHERE Id = @id";
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@id", pComponente.Id);
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Guarda la estructura jerárquica de un Rol (Composite) utilizando una transacción atómica.
        /// </summary>
        public void GuardarEstructuraRol(CompuestoBE pRol)
        {
            using (SqlConnection cn = new SqlConnection(_connectionString))
            {
                cn.Open();
                SqlTransaction txn = cn.BeginTransaction();
                try
                {
                    string queryRol = pRol.Id == 0 ?
                        "INSERT INTO Permiso (Nombre, EsFamilia) VALUES (@nombre, 1); SELECT SCOPE_IDENTITY();" :
                        "UPDATE Permiso SET Nombre = @nombre WHERE Id = @id";

                    using (SqlCommand cmd = new SqlCommand(queryRol, cn, txn))
                    {
                        cmd.Parameters.AddWithValue("@nombre", pRol.Nombre);
                        if (pRol.Id > 0) cmd.Parameters.AddWithValue("@id", pRol.Id);

                        if (pRol.Id == 0) pRol.Id = Convert.ToInt32(cmd.ExecuteScalar());
                        else cmd.ExecuteNonQuery();
                    }

                    // Sincronización de relaciones de jerarquía
                    foreach (var hijo in pRol.ObtenerHijos())
                    {
                        string q = "";
                        if (hijo.Estado == ComponenteBE.EstadoEntidad.Agregado)
                            q = "INSERT INTO Permiso_Permiso (IdPadre, IdHijo) VALUES (@padre, @hijo)";
                        else if (hijo.Estado == ComponenteBE.EstadoEntidad.Eliminado)
                            q = "DELETE FROM Permiso_Permiso WHERE IdPadre = @padre AND IdHijo = @hijo";

                        if (!string.IsNullOrEmpty(q))
                        {
                            using (SqlCommand cmd = new SqlCommand(q, cn, txn))
                            {
                                cmd.Parameters.AddWithValue("@padre", pRol.Id);
                                cmd.Parameters.AddWithValue("@hijo", hijo.Id);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    txn.Commit();
                }
                catch
                {
                    txn.Rollback();
                    throw new Exception("Error al persistir la estructura jerárquica del Rol.");
                }
            }
        }

        #endregion

        #region "Métodos de Lectura e Hidratación"

        /// <summary>
        /// Lista todos los permisos simples (hojas) que no poseen estructura de Rol.
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
                            lista.Add(new PermisoBE { Id = (int)reader["Id"], Nombre = reader["Nombre"].ToString() });
                    }
                }
            }
            return lista;
        }

        /// <summary>
        /// Lista todos los componentes hidratando sus estructuras recursivas para su uso en el catálogo.
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
                            bool esF = Convert.ToBoolean(reader["EsFamilia"]);
                            ComponenteBE c = esF ?
                                (ComponenteBE)new CompuestoBE { Id = (int)reader["Id"], Nombre = reader["Nombre"].ToString() } :
                                (ComponenteBE)new PermisoBE { Id = (int)reader["Id"], Nombre = reader["Nombre"].ToString() };

                            lista.Add(c);
                        }
                    }
                    // HIDRATACIÓN CRÍTICA: Llenamos los hijos de los roles detectados para el catálogo
                    foreach (var componente in lista)
                    {
                        if (componente is CompuestoBE) LlenarHijos(componente, cn);
                    }
                }
            }
            return lista;
        }

        /// <summary>
        /// Recupera la configuración de seguridad completa asignada a un usuario específico.
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
                            bool esF = Convert.ToBoolean(reader["EsFamilia"]);
                            ComponenteBE c = esF ?
                                (ComponenteBE)new CompuestoBE { Id = (int)reader["Id"], Nombre = reader["Nombre"].ToString() } :
                                (ComponenteBE)new PermisoBE { Id = (int)reader["Id"], Nombre = reader["Nombre"].ToString() };

                            if (esF) LlenarHijos(c, cn);
                            lista.Add(c);
                        }
                    }
                }
            }
            return lista;
        }

        #endregion

        #region "Lógica Composite (Recursividad)"

        /// <summary>
        /// Sobrecarga pública para iniciar la hidratación recursiva de un componente.
        /// </summary>
        public void LlenarHijos(ComponenteBE padre)
        {
            using (SqlConnection cn = new SqlConnection(_connectionString))
            {
                cn.Open();
                LlenarHijos(padre, cn);
            }
        }

        /// <summary>
        /// Ejecuta la carga recursiva de la estructura de permisos navegando por la tabla de relaciones.
        /// </summary>
        public void LlenarHijos(ComponenteBE padre, SqlConnection cnExistente)
        {
            bool cerrarConexion = false;
            SqlConnection cn = cnExistente;
            if (cn == null)
            {
                cn = new SqlConnection(_connectionString);
                cn.Open();
                cerrarConexion = true;
            }

            try
            {
                string query = @"SELECT p.Id, p.Nombre, p.EsFamilia 
                                 FROM Permiso p 
                                 INNER JOIN Permiso_Permiso pp ON p.Id = pp.IdHijo 
                                 WHERE pp.IdPadre = @idPadre";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@idPadre", padre.Id);

                    var hijosEnMemoria = new List<ComponenteBE>();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bool esF = Convert.ToBoolean(reader["EsFamilia"]);
                            ComponenteBE hijo = esF ?
                                (ComponenteBE)new CompuestoBE { Id = (int)reader["Id"], Nombre = reader["Nombre"].ToString() } :
                                (ComponenteBE)new PermisoBE { Id = (int)reader["Id"], Nombre = reader["Nombre"].ToString() };

                            hijosEnMemoria.Add(hijo);
                        }
                    }

                    foreach (var h in hijosEnMemoria)
                    {
                        if (h is CompuestoBE) LlenarHijos(h, cn);
                        padre.AgregarHijo(h);
                    }
                }
            }
            finally
            {
                if (cerrarConexion && cn.State == ConnectionState.Open) cn.Close();
            }
        }

        #endregion
    }
}