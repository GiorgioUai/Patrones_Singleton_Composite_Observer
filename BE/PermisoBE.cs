using System;
using System.Collections.Generic;

namespace BE
{
    /// <summary>
    /// Representa un permiso simple (Hoja) en la estructura de seguridad del patrón Composite.
    /// Al ser un componente terminal, no permite la gestión de hijos.
    /// </summary>
    public class PermisoBE : ComponenteBE
    {
        #region "Atributos / Propiedades"

        /// <summary>
        /// Identificador de tipo para el componente.
        /// </summary>
        public string Tipo => "Permiso";

        #endregion

        #region "Implementación Composite"

        /// <summary>
        /// No soportado en componentes de tipo Hoja.
        /// </summary>
        public override void AgregarHijo(ComponenteBE hijo)
        {
            throw new NotSupportedException("Un permiso simple no puede contener hijos.");
        }

        /// <summary>
        /// No soportado en componentes de tipo Hoja.
        /// </summary>
        public override void QuitarHijo(ComponenteBE hijo)
        {
            throw new NotSupportedException("Un permiso simple no posee hijos para quitar.");
        }

        /// <summary>
        /// Retorna una colección vacía constante para optimizar el rendimiento.
        /// </summary>
        /// <returns>Array vacío de ComponenteBE.</returns>
        public override IReadOnlyCollection<ComponenteBE> ObtenerHijos()
        {
            return Array.Empty<ComponenteBE>();
        }

        /// <summary>
        /// Valida si el permiso actual coincide con el nombre buscado.
        /// </summary>
        /// <param name="permisoBuscado">Nombre del permiso a validar.</param>
        /// <returns>True si coinciden, de lo contrario False.</returns>
        public override bool ValidarPermisos(string permisoBuscado)
        {
            if (string.IsNullOrEmpty(permisoBuscado)) return false;
            return this.Nombre.Equals(permisoBuscado, StringComparison.OrdinalIgnoreCase);
        }

        #endregion

        #region "Gestión de Estados"

        /// <summary>
        /// Restablece el estado del componente a 'SinCambio' tras una persistencia exitosa en la base de datos.
        /// </summary>
        public override void LimpiarEstados()
        {
            this.Estado = EstadoEntidad.SinCambio;
        }

        #endregion
    }
}