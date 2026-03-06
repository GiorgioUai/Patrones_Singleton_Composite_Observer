using System;

namespace BE.Interfaces

{
    #region "Contrato de Multiidioma"

    /// <summary>
    /// Define el contrato para los componentes que deben reaccionar ante un cambio de idioma en la aplicación.
    /// Permite la actualización dinámica de textos en la interfaz de usuario.
    /// </summary>
    public interface IIdiomaObserver
    {
        /// <summary>
        /// Método invocado para notificar que el idioma del sistema ha sido modificado.
        /// </summary>
        void ActualizarIdioma();
    }

    #endregion
}