using System;
using System.Collections.Generic;

namespace BE
{
    /// <summary>
    /// Clase base abstracta que define el contrato para todos los componentes 
    /// de la estructura de seguridad (Composite).
    /// </summary>
    public abstract class ComponenteBE
    {
        #region "Enums"

        /// <summary>
        /// Representa los posibles estados de persistencia de una entidad en memoria RAM.
        /// </summary>
        public enum EstadoEntidad
        {
            SinCambio,
            Agregado,
            Eliminado,
            Modificado
        }

        #endregion

        #region "Propiedades"

        /// <summary>
        /// Identificador único de la entidad en la base de datos.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nombre descriptivo del permiso o rol.
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Estado actual de la entidad respecto a su persistencia en la base de datos.
        /// </summary>
        public EstadoEntidad Estado { get; set; } = EstadoEntidad.SinCambio;

        #endregion

        #region "Contrato Composite (Métodos Abstractos)"

        public abstract bool ValidarPermisos(string permisoBuscado);
        public abstract void AgregarHijo(ComponenteBE hijo);
        public abstract void QuitarHijo(ComponenteBE hijo);
        public abstract IReadOnlyCollection<ComponenteBE> ObtenerHijos();

        #endregion

        #region "Gestión de Estados"

        /// <summary>
        /// Restablece el estado de la entidad a 'SinCambio' tras una persistencia exitosa.
        /// Debe implementarse para propagar la limpieza de forma recursiva en clases compuestas.
        /// </summary>
        public abstract void LimpiarEstados();

        #endregion
    }
}