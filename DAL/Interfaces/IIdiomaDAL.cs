using System.Collections.Generic;
using BE;

namespace DAL.Interfaces
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