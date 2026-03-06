using System;

namespace BE.Interfaces

{
    #region "Contrato de Suscripción"

    /// <summary>
    /// Define el contrato para los componentes que actúan como observadores de la sesión de usuario.
    /// Esta interfaz permite que cualquier formulario reaccione ante eventos de Login y Logout.
    /// </summary>
    public interface ISesionObserver
    {
        /// <summary>
        /// Método invocado por el sujeto (SesionManager) para notificar cambios en el estado de la sesión.
        /// </summary>
        void ActualizarSesion();
    }

    #endregion
}