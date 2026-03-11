using System.Collections.Generic;
using BE;

namespace DAL.Interfaces
{
    /// <summary>
    /// Define el contrato para la persistencia de datos multiidioma.
    /// </summary>
    public interface IIdiomaDAL
    {
        #region "Lectura"

        List<IdiomaBE> ObtenerIdiomas();
        Dictionary<string, string> ObtenerTraducciones(string nombreIdioma);
        List<string> ListarTagsExistentes();

        #endregion

        #region "Escritura (ABM)"

        void GuardarEtiqueta(string nombreTag);
        void GuardarTraduccion(int idIdioma, string nombreTag, string texto);

        /// <summary>
        /// Elimina físicamente una traducción para un idioma específico.
        /// </summary>
        void EliminarTraduccion(int idIdioma, string nombreTag);

        /// <summary>
        /// Elimina una etiqueta de la maestra (y por cascada sus traducciones).
        /// </summary>
        void EliminarEtiqueta(string nombreTag);

        #endregion
    }
}