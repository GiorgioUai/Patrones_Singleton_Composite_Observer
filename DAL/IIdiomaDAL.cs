using System.Collections.Generic;
using BE; // Crucial para que la interfaz reconozca la entidad IdiomaBE [cite: 2026-03-06]

namespace DAL
{
    /// <summary>
    /// Define el contrato para la persistencia de datos multiidioma.
    /// </summary>
    public interface IIdiomaDAL
    {
        /// <summary>
        /// Obtiene la lista completa de entidades de idioma disponibles.
        /// </summary>
        List<IdiomaBE> ObtenerIdiomas();

        /// <summary>
        /// Obtiene el diccionario de etiquetas y traducciones para un idioma específico.
        /// </summary>
        Dictionary<string, string> ObtenerTraducciones(string nombreIdioma);
    }
}