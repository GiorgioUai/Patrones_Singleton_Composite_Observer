using System;
using System.Collections.Generic;
using BE;

namespace BL
{
    /// <summary>
    /// Gestiona la sesión activa del usuario y notifica cambios de seguridad a los observadores.
    /// Implementa el patrón Singleton para garantizar una única sesión en la aplicación.
    /// </summary>
    public sealed class SesionManagerBL
    {
        #region "Atributos / Variables Privadas"

        private static SesionManagerBL _instance;
        private static readonly object _lock = new object();

        /// <summary>
        /// Lista de observadores (formularios) que deben reaccionar ante cambios de sesión.
        /// </summary>
        private readonly List<ISesionObserver> _observadores = new List<ISesionObserver>();

        #endregion

        #region "Propiedades"

        /// <summary>
        /// Almacena el usuario que se encuentra actualmente autenticado en el sistema.
        /// </summary>
        public UsuarioBE _Usuario { get; private set; } = null;

        #endregion

        #region "Constructor y Singleton"

        private SesionManagerBL() { }

        /// <summary>
        /// Obtiene la instancia única del gestor de sesión utilizando Double-Check Locking.
        /// </summary>
        public static SesionManagerBL GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new SesionManagerBL();
                    }
                }
            }
            return _instance;
        }

        #endregion

        #region "Gestión de Observadores (Patrón Observer)"

        /// <summary>
        /// Registra un nuevo componente para recibir notificaciones de inicio o cierre de sesión.
        /// </summary>
        public void Suscribir(ISesionObserver observador)
        {
            if (!_observadores.Contains(observador))
            {
                _observadores.Add(observador);
            }
        }

        /// <summary>
        /// Elimina un componente de la lista de notificaciones.
        /// </summary>
        public void Desuscribir(ISesionObserver observador)
        {
            if (_observadores.Contains(observador))
            {
                _observadores.Remove(observador);
            }
        }

        /// <summary>
        /// Notifica a todos los observadores registrados que el estado de la sesión ha cambiado.
        /// </summary>
        private void Notificar()
        {
            foreach (var observador in _observadores)
            {
                observador.ActualizarSesion();
            }
        }

        #endregion

        #region "Métodos de Sesión"

        /// <summary>
        /// Establece la sesión para el usuario indicado y dispara la notificación a la UI.
        /// </summary>
        public void LogIn(UsuarioBE usuario)
        {
            if (usuario != null)
            {
                _Usuario = usuario;
                Notificar();
            }
        }

        /// <summary>
        /// Finaliza la sesión actual y dispara la notificación para limpiar la interfaz.
        /// </summary>
        public void LogOut()
        {
            _Usuario = null;
            Notificar();
        }

        #endregion
    }
}