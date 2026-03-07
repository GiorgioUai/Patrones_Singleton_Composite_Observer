using System;
using System.Collections.Generic;
using BE;
using BE.Interfaces;
using DAL;
using DAL.Interfaces;
using System.Linq;

namespace BL
{
    /// <summary>
    /// Gestiona el idioma actual de la aplicación y el diccionario de traducciones.
    /// Implementa Singleton y actúa como Sujeto en el patrón Observer.
    /// </summary>
    public class IdiomaManagerBL
    {
        #region "Atributos / Variables Privadas"

        private static IdiomaManagerBL _instance;
        private static readonly object _lock = new object();
        private readonly IIdiomaDAL _idiomaDAL;
        private readonly List<IIdiomaObserver> _observadores = new List<IIdiomaObserver>();
        private Dictionary<string, string> _traducciones = new Dictionary<string, string>();

        #endregion

        #region "Propiedades"

        /// <summary>
        /// Almacena el objeto completo del idioma actual (contiene Id, Nombre y Sigla).
        /// </summary>
        public IdiomaBE IdiomaActual { get; private set; }

        #endregion

        #region "Constructor y Singleton"

        private IdiomaManagerBL()
        {
            _idiomaDAL = new IdiomaDAL();

            // Inicializamos con un objeto IdiomaBE por defecto (ES = ID 1)
            IdiomaActual = new IdiomaBE { Id = 1, Nombre = "ES" };

            CargarTraducciones();
        }

        public static IdiomaManagerBL GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new IdiomaManagerBL();
                    }
                }
            }
            return _instance;
        }

        #endregion

        #region "Gestión de Observadores (Patrón Observer)"

        public void Suscribir(IIdiomaObserver observador)
        {
            if (!_observadores.Contains(observador))
                _observadores.Add(observador);
        }

        public void Desuscribir(IIdiomaObserver observador)
        {
            if (_observadores.Contains(observador))
                _observadores.Remove(observador);
        }

        private void Notificar()
        {
            foreach (var observador in _observadores)
            {
                observador.ActualizarIdioma();
            }
        }

        #endregion

        #region "Métodos Públicos y Lógica de Datos"

        /// <summary>
        /// Cambia el idioma actual basándose en el objeto de entidad IdiomaBE.
        /// </summary>
        public void CambiarIdioma(IdiomaBE nuevoIdioma)
        {
            if (nuevoIdioma != null)
            {
                IdiomaActual = nuevoIdioma;
                CargarTraducciones();
                Notificar();
            }
        }

        /// <summary>
        /// Cambia el idioma actual basándose en su nombre/código (ej: "ES").
        /// Busca el ID correspondiente en la DAL para mantener la integridad del objeto.
        /// </summary>
        public void CambiarIdioma(string nombreIdioma)
        {
            // Buscamos el objeto completo para no perder el ID
            var idiomasDisponibles = ObtenerIdiomas();
            var idiomaEncontrado = idiomasDisponibles.FirstOrDefault(i => i.Nombre.Equals(nombreIdioma, StringComparison.OrdinalIgnoreCase));

            if (idiomaEncontrado != null)
            {
                CambiarIdioma(idiomaEncontrado);
            }
            else
            {
                // Fallback por seguridad si no se encuentra en la DB
                IdiomaActual = new IdiomaBE { Id = 1, Nombre = nombreIdioma };
                CargarTraducciones();
                Notificar();
            }
        }

        public List<IdiomaBE> ObtenerIdiomas()
        {
            return _idiomaDAL.ObtenerIdiomas();
        }

        public string ObtenerTexto(string tag)
        {
            if (_traducciones != null && _traducciones.ContainsKey(tag))
            {
                return _traducciones[tag];
            }
            return $"[{tag}]";
        }

        private void CargarTraducciones()
        {
            // Ahora pasamos el Nombre (sigla) del objeto IdiomaActual
            _traducciones = _idiomaDAL.ObtenerTraducciones(IdiomaActual.Nombre);
        }

        #endregion
    }
}