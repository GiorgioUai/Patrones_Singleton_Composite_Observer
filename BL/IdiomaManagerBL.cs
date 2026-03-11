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

        public void CambiarIdioma(IdiomaBE nuevoIdioma)
        {
            if (nuevoIdioma != null)
            {
                IdiomaActual = nuevoIdioma;
                CargarTraducciones();
                Notificar();
            }
        }

        public void CambiarIdioma(string nombreIdioma)
        {
            var idiomasDisponibles = ObtenerIdiomas();
            var idiomaEncontrado = idiomasDisponibles.FirstOrDefault(i => i.Nombre.Equals(nombreIdioma, StringComparison.OrdinalIgnoreCase));

            if (idiomaEncontrado != null)
            {
                CambiarIdioma(idiomaEncontrado);
            }
            else
            {
                IdiomaActual = new IdiomaBE { Id = 1, Nombre = nombreIdioma };
                CargarTraducciones();
                Notificar();
            }
        }

        public List<IdiomaBE> ObtenerIdiomas()
        {
            return _idiomaDAL.ObtenerIdiomas();
        }

        /// <summary>
        /// Retorna el texto traducido para el idioma ACTUAL de la sesión.
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
        /// Retorna el texto de un tag para un idioma específico sin cambiar la sesión actual.
        /// Útil para el ABM de traducciones.
        /// </summary>
        public string ObtenerTextoPorIdioma(string tag, string nombreIdioma)
        {
            var diccTmp = _idiomaDAL.ObtenerTraducciones(nombreIdioma);
            if (diccTmp != null && diccTmp.ContainsKey(tag))
            {
                return diccTmp[tag];
            }
            return "";
        }

        private void CargarTraducciones()
        {
            _traducciones = _idiomaDAL.ObtenerTraducciones(IdiomaActual.Nombre);
        }

        #endregion

        #region "Lógica para el ABM de Idiomas"

        public void GuardarEtiqueta(string nombreTag)
        {
            _idiomaDAL.GuardarEtiqueta(nombreTag);
        }

        public void GuardarTraduccion(int idIdioma, string nombreTag, string texto)
        {
            _idiomaDAL.GuardarTraduccion(idIdioma, nombreTag, texto);

            if (idIdioma == IdiomaActual.Id)
            {
                CargarTraducciones();
                Notificar();
            }
        }

        public List<string> ListarTagsExistentes()
        {
            return _idiomaDAL.ListarTagsExistentes();
        }

        /// <summary>
        /// Elimina una traducción específica para un idioma.
        /// </summary>
        public void EliminarTraduccion(int idIdioma, string nombreTag)
        {
            _idiomaDAL.EliminarTraduccion(idIdioma, nombreTag);

            if (idIdioma == IdiomaActual.Id)
            {
                CargarTraducciones();
                Notificar();
            }
        }

        /// <summary>
        /// Elimina la etiqueta de la maestra y todas sus traducciones.
        /// </summary>
        public void EliminarEtiqueta(string nombreTag)
        {
            _idiomaDAL.EliminarEtiqueta(nombreTag);

            // Siempre recargamos y notificamos porque el tag desaparece de todos los idiomas
            CargarTraducciones();
            Notificar();
        }

        #endregion
    }
}