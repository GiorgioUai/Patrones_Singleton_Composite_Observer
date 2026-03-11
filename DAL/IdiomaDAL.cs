using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BE;
using DAL.Interfaces;

namespace DAL
{
    /// <summary>
    /// Gestiona la persistencia y recuperación de etiquetas y traducciones multiidioma.
    /// Implementa el contrato IIdiomaDAL para soportar inyección de dependencias.
    /// </summary>
    public class IdiomaDAL : DAO, IIdiomaDAL
    {
        #region "Lectura de Configuración de Idiomas"

        /// <summary>
        /// Obtiene todos los idiomas configurados en el sistema mapeados a la entidad IdiomaBE.
        /// </summary>
        /// <returns>Lista de objetos IdiomaBE.</returns>
        public List<IdiomaBE> ObtenerIdiomas()
        {
            List<IdiomaBE> idiomas = new List<IdiomaBE>();

            using (SqlConnection cn = new SqlConnection(_connectionString))
            {
                string query = "SELECT Id, Nombre FROM Idioma";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            idiomas.Add(new IdiomaBE
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Nombre = reader["Nombre"].ToString()
                            });
                        }
                    }
                }
            }
            return idiomas;
        }

        #endregion

        #region "Recuperación de Traducciones"

        /// <summary>
        /// Recupera el diccionario completo de traducciones para un idioma específico.
        /// </summary>
        /// <param name="nombreIdioma">Nombre del idioma (ej. 'ES', 'EN').</param>
        /// <returns>Diccionario con Tag y su Texto traducido.</returns>
        public Dictionary<string, string> ObtenerTraducciones(string nombreIdioma)
        {
            Dictionary<string, string> traducciones = new Dictionary<string, string>();

            using (SqlConnection cn = new SqlConnection(_connectionString))
            {
                string query = @"SELECT e.Nombre as Tag, t.Texto FROM Traduccion t 
                                INNER JOIN Etiqueta e ON t.IdEtiqueta = e.Id 
                                INNER JOIN Idioma i ON t.IdIdioma = i.Id 
                                WHERE i.Nombre = @idioma";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@idioma", nombreIdioma);

                    try
                    {
                        cn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string tag = reader["Tag"].ToString();
                                string texto = reader["Texto"].ToString();

                                if (!traducciones.ContainsKey(tag))
                                {
                                    traducciones.Add(tag, texto);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error en DAL al recuperar traducciones para el idioma: " + nombreIdioma, ex);
                    }
                }
            }
            return traducciones;
        }

        #endregion

        #region "Lógica para el ABM de Idiomas"

        /// <summary>
        /// Obtiene la lista de todos los nombres de etiquetas registrados en el sistema.
        /// </summary>
        public List<string> ListarTagsExistentes()
        {
            List<string> tags = new List<string>();
            using (SqlConnection cn = new SqlConnection(_connectionString))
            {
                string query = "SELECT Nombre FROM Etiqueta ORDER BY Nombre ASC";
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tags.Add(reader["Nombre"].ToString());
                        }
                    }
                }
            }
            return tags;
        }

        /// <summary>
        /// Registra una nueva etiqueta en la base de datos si no existe.
        /// </summary>
        public void GuardarEtiqueta(string nombreTag)
        {
            using (SqlConnection cn = new SqlConnection(_connectionString))
            {
                string query = @"IF NOT EXISTS (SELECT 1 FROM Etiqueta WHERE Nombre = @tag)
                                 INSERT INTO Etiqueta (Nombre) VALUES (@tag)";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@tag", nombreTag);
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Registra o actualiza el texto de una traducción para un idioma y etiqueta específicos.
        /// </summary>
        public void GuardarTraduccion(int idIdioma, string nombreTag, string texto)
        {
            using (SqlConnection cn = new SqlConnection(_connectionString))
            {
                string query = @"
                    DECLARE @idEtiqueta INT = (SELECT Id FROM Etiqueta WHERE Nombre = @tag);
                    
                    IF EXISTS (SELECT 1 FROM Traduccion WHERE IdEtiqueta = @idEtiqueta AND IdIdioma = @idIdio)
                    BEGIN
                        UPDATE Traduccion SET Texto = @txt 
                        WHERE IdEtiqueta = @idEtiqueta AND IdIdioma = @idIdio
                    END
                    ELSE
                    BEGIN
                        INSERT INTO Traduccion (IdEtiqueta, IdIdioma, Texto) 
                        VALUES (@idEtiqueta, @idIdio, @txt)
                    END";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@tag", nombreTag);
                    cmd.Parameters.AddWithValue("@idIdio", idIdioma);
                    cmd.Parameters.AddWithValue("@txt", texto);
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Elimina físicamente una traducción para un idioma específico.
        /// </summary>
        public void EliminarTraduccion(int idIdioma, string nombreTag)
        {
            using (SqlConnection cn = new SqlConnection(_connectionString))
            {
                string query = @"DELETE FROM Traduccion 
                                 WHERE IdIdioma = @idIdio 
                                 AND IdEtiqueta = (SELECT Id FROM Etiqueta WHERE Nombre = @tag)";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@idIdio", idIdioma);
                    cmd.Parameters.AddWithValue("@tag", nombreTag);
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Elimina una etiqueta de la maestra y todas sus traducciones asociadas.
        /// </summary>
        public void EliminarEtiqueta(string nombreTag)
        {
            using (SqlConnection cn = new SqlConnection(_connectionString))
            {
                // Primero eliminamos las traducciones asociadas para evitar error de FK
                // Luego eliminamos la etiqueta de la tabla maestra
                string query = @"
                    DECLARE @idEtiqueta INT = (SELECT Id FROM Etiqueta WHERE Nombre = @tag);
                    DELETE FROM Traduccion WHERE IdEtiqueta = @idEtiqueta;
                    DELETE FROM Etiqueta WHERE Id = @idEtiqueta;";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@tag", nombreTag);
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        #endregion
    }
}