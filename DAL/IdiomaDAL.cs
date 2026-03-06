using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BE; // Crucial para reconocer la entidad IdiomaBE [cite: 2026-03-06]

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
                // Agregamos el Id a la consulta para persistencia [cite: 2026-03-06]
                string query = "SELECT Id, Nombre FROM Idioma";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Creamos el objeto de entidad y lo agregamos a la lista [cite: 2026-03-06]
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
                string query = @"SELECT e.Nombre as Tag, t.Texto 
                                 FROM Traduccion t
                                 INNER JOIN Etiqueta e ON t.Id_Etiqueta = e.Id
                                 INNER JOIN Idioma i ON t.Id_Idioma = i.Id
                                 WHERE i.Nombre = @idioma";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@idioma", nombreIdioma);
                    cn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            traducciones.Add(
                                reader["Tag"].ToString(),
                                reader["Texto"].ToString()
                            );
                        }
                    }
                }
            }
            return traducciones;
        }

        #endregion
    }
}