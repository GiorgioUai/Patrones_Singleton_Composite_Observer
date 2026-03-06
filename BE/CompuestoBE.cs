using System;
using System.Collections.Generic;
using System.Linq;

namespace BE
{
    /// <summary>
    /// Representa un componente complejo (Rol) dentro de la estructura de seguridad.
    /// </summary>
    public class CompuestoBE : ComponenteBE
    {
        #region "Atributos y Propiedades"

        private List<ComponenteBE> _hijos;
        public string Tipo => "Rol";

        #endregion

        #region "Constructor"

        public CompuestoBE()
        {
            _hijos = new List<ComponenteBE>();
        }

        #endregion

        #region "Implementación Composite"

        public override void AgregarHijo(ComponenteBE hijo)
        {
            var existente = _hijos.FirstOrDefault(x => x.Id == hijo.Id);
            if (existente != null)
            {
                if (existente.Estado == ComponenteBE.EstadoEntidad.Eliminado)
                    existente.Estado = ComponenteBE.EstadoEntidad.SinCambio;
            }
            else
            {
                hijo.Estado = ComponenteBE.EstadoEntidad.Agregado;
                _hijos.Add(hijo);
            }
        }

        public override void QuitarHijo(ComponenteBE hijo)
        {
            var item = _hijos.FirstOrDefault(x => x.Id == hijo.Id);
            if (item != null)
            {
                if (item.Estado == ComponenteBE.EstadoEntidad.Agregado)
                {
                    _hijos.Remove(item);
                }
                else
                {
                    item.Estado = ComponenteBE.EstadoEntidad.Eliminado;
                }
            }
        }

        public override IReadOnlyCollection<ComponenteBE> ObtenerHijos()
        {
            return _hijos.Where(x => x.Estado != ComponenteBE.EstadoEntidad.Eliminado)
                         .ToList()
                         .AsReadOnly();
        }

        public IEnumerable<ComponenteBE> ObtenerEstructuraCompleta()
        {
            return _hijos;
        }

        public override bool ValidarPermisos(string permisoBuscado)
        {
            foreach (var hijo in _hijos.Where(x => x.Estado != ComponenteBE.EstadoEntidad.Eliminado))
            {
                if (hijo.Nombre.Equals(permisoBuscado, StringComparison.OrdinalIgnoreCase))
                    return true;
                if (hijo.ValidarPermisos(permisoBuscado))
                    return true;
            }
            return false;
        }

        #endregion

        #region "Gestión de Estados"

        /// <summary>
        /// Sincroniza la estructura en memoria RAM tras el guardado en la base de datos.
        /// </summary>
        public override void LimpiarEstados()
        {
            this.Estado = EstadoEntidad.SinCambio;
            _hijos.RemoveAll(x => x.Estado == EstadoEntidad.Eliminado);
            foreach (var hijo in _hijos)
            {
                hijo.LimpiarEstados();
            }
        }

        #endregion
    }
}