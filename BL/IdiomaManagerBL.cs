using System;
using System.Collections.Generic;
using BE;
using BE.Interfaces;
using DAL;
using DAL.Interfaces;

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
        /// Almacena el código del idioma actual (ej: "ES", "EN").
        /// </summary>
        public string IdiomaActual { get; private set; }

        #endregion

        #region "Constructor y Singleton"

        private IdiomaManagerBL()
        {
            _idiomaDAL = new IdiomaDAL();
            // Idioma por defecto al iniciar la aplicación
            IdiomaActual = "ES";
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
                CambiarIdioma(nuevoIdioma.Nombre); // Reutilizamos la lógica del string
            }
        }

        /// <summary>
        /// Cambia el idioma actual basándose en su nombre/código (ej: "ES").
        /// </summary>
        public void CambiarIdioma(string nombreIdioma)
        {
            IdiomaActual = nombreIdioma;
            CargarTraducciones();
            Notificar();
        }

        /// <summary>
        /// Retorna la lista de todos los idiomas disponibles en la base de datos.
        /// </summary>
        public List<IdiomaBE> ObtenerIdiomas()
        {
            return _idiomaDAL.ObtenerIdiomas();
        }

        /// <summary>
        /// Retorna la traducción correspondiente a un Tag específico.
        /// </summary>
        public string ObtenerTexto(string tag)
        {
            if (_traducciones != null && _traducciones.ContainsKey(tag))
            {
                return _traducciones[tag];
            }
            return $"[{tag}]";
        }

        /// <summary>
        /// Actualiza el diccionario de traducciones desde la DAL.
        /// </summary>
        private void CargarTraducciones()
        {
            _traducciones = _idiomaDAL.ObtenerTraducciones(IdiomaActual);
        }

        #endregion
    }
}