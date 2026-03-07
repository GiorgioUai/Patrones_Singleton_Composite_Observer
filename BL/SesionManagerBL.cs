using System;
using System.Collections.Generic;
using BE;
using BE.Interfaces;

namespace BL
{
    /// <summary>
    /// Gestiona la sesión activa del usuario y notifica cambios de seguridad a los observadores.
    /// Implementa el patrón Singleton y valida permisos mediante el patrón Composite.
    /// </summary>
    public sealed class SesionManagerBL
    {
        #region "Atributos / Variables Privadas"

        private static SesionManagerBL _instance;
        private static readonly object _lock = new object();
        private readonly List<ISesionObserver> _observadores = new List<ISesionObserver>();

        #endregion

        #region "Propiedades"

        /// <summary>
        /// Almacena el usuario autenticado con su respectivo árbol de Roles y Permisos.
        /// </summary>
        public UsuarioBE _Usuario { get; private set; } = null;

        #endregion

        #region "Constructor y Singleton"

        private SesionManagerBL() { }

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

        public void Suscribir(ISesionObserver observador)
        {
            if (!_observadores.Contains(observador))
                _observadores.Add(observador);
        }

        public void Desuscribir(ISesionObserver observador)
        {
            if (_observadores.Contains(observador))
                _observadores.Remove(observador);
        }

        private void Notificar()
        {
            foreach (var observador in _observadores)
            {
                observador.ActualizarSesion();
            }
        }

        #endregion

        #region "Métodos de Sesión y Seguridad"

        /// <summary>
        /// Establece la sesión activa y dispara las notificaciones de seguridad.
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
        /// Finaliza la sesión actual y limpia las referencias del usuario.
        /// </summary>
        public void LogOut()
        {
            _Usuario = null;
            Notificar();
        }

        /// <summary>
        /// Determina si el usuario actual posee un permiso específico recorriendo su jerarquía de Roles.
        /// Implementa la búsqueda recursiva propia del patrón Composite.
        /// </summary>
        /// <param name="nombrePermiso">Nombre o código del Permiso a buscar.</param>
        /// <returns>True si tiene el permiso concedido directamente o heredado por un Rol.</returns>
        public bool TienePermiso(string nombrePermiso)
        {
            if (_Usuario == null || _Usuario.Permisos == null) return false;

            foreach (var componente in _Usuario.Permisos)
            {
                if (ValidarRecursivo(componente, nombrePermiso)) return true;
            }

            return false;
        }

        /// <summary>
        /// Método auxiliar para navegar recursivamente el árbol de componentes de seguridad.
        /// </summary>
        private bool ValidarRecursivo(ComponenteBE componente, string nombrePermiso)
        {
            // Caso base: El componente actual es el Permiso buscado
            if (componente.Nombre.Equals(nombrePermiso, StringComparison.OrdinalIgnoreCase))
                return true;

            // Si es un Rol (Composite), buscamos dentro de sus hijos
            if (componente is CompuestoBE compuesto)
            {
                foreach (var hijo in compuesto.ObtenerHijos())
                {
                    if (ValidarRecursivo(hijo, nombrePermiso)) return true;
                }
            }

            return false;
        }

        #endregion
    }
}