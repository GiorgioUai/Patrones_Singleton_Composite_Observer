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
        #region "Escritura"
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

                    foreach (var hijo in pRol.ObtenerEstructuraCompleta())
                    {
                        string q = "";
                        if (hijo.Estado == ComponenteBE.EstadoEntidad.Agregado) q = "INSERT INTO Permiso_Permiso (IdPadre, IdHijo) VALUES (@padre, @hijo)";
                        else if (hijo.Estado == ComponenteBE.EstadoEntidad.Eliminado) q = "DELETE FROM Permiso_Permiso WHERE IdPadre = @padre AND IdHijo = @hijo";

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
                catch { txn.Rollback(); throw; }
            }
        }
        #endregion

        #region "Lectura"
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
                        while (reader.Read()) lista.Add(new PermisoBE { Id = (int)reader["Id"], Nombre = reader["Nombre"].ToString() });
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
                            bool esF = Convert.ToBoolean(reader["EsFamilia"]);
                            lista.Add(esF ? (ComponenteBE)new CompuestoBE { Id = (int)reader["Id"], Nombre = reader["Nombre"].ToString() } :
                                            (ComponenteBE)new PermisoBE { Id = (int)reader["Id"], Nombre = reader["Nombre"].ToString() });
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
                string query = @"SELECT p.Id, p.Nombre, p.EsFamilia FROM Permiso p INNER JOIN Usuario_Permiso up ON p.Id = up.IdPermiso WHERE up.IdUsuario = @id";
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@id", usuarioId);
                    cn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bool esF = Convert.ToBoolean(reader["EsFamilia"]);
                            ComponenteBE c = esF ? (ComponenteBE)new CompuestoBE { Id = (int)reader["Id"], Nombre = reader["Nombre"].ToString() } :
                                                   (ComponenteBE)new PermisoBE { Id = (int)reader["Id"], Nombre = reader["Nombre"].ToString() };
                            if (esF) LlenarHijos(c, cn); // Pasamos la conexión
                            lista.Add(c);
                        }
                    }
                }
            }
            return lista;
        }

        public void LlenarHijos(ComponenteBE padre, SqlConnection cnExistente = null)
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
                string query = @"SELECT p.Id, p.Nombre, p.EsFamilia FROM Permiso p INNER JOIN Permiso_Permiso pp ON p.Id = pp.IdHijo WHERE pp.IdPadre = @idPadre";
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@idPadre", padre.Id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        var hijos = new List<ComponenteBE>();
                        while (reader.Read())
                        {
                            bool esF = Convert.ToBoolean(reader["EsFamilia"]);
                            ComponenteBE hijo = esF ? (ComponenteBE)new CompuestoBE { Id = (int)reader["Id"], Nombre = reader["Nombre"].ToString() } :
                                                      (ComponenteBE)new PermisoBE { Id = (int)reader["Id"], Nombre = reader["Nombre"].ToString() };
                            hijos.Add(hijo);
                        }
                        reader.Close(); // Cerramos reader antes de la recursividad

                        foreach (var h in hijos)
                        {
                            if (h is CompuestoBE) LlenarHijos(h, cn);
                            padre.AgregarHijo(h);
                        }
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