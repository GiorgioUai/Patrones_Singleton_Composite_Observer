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
                // Consulta a la tabla Idioma (PascalCase según image_3f4cfc)
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
                string query = @"SELECT e.Nombre as Tag, t.Texto FROM Traduccion t INNER JOIN Etiqueta e ON t.IdEtiqueta = e.Id INNER JOIN Idioma i ON t.IdIdioma = i.Id WHERE i.Nombre = @idioma";

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

                                // Validación de integridad para evitar duplicados en el diccionario
                                if (!traducciones.ContainsKey(tag))
                                {
                                    traducciones.Add(tag, texto);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Lógica para debug técnico
                        throw new Exception("Error en DAL al recuperar traducciones para el idioma: " + nombreIdioma, ex);
                    }
                }
            }
            return traducciones;
        }

        #endregion
    }
}