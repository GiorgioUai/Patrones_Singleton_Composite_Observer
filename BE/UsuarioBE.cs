using System;
using System.Collections.Generic;
using System.Linq;

namespace BE
{
    /// <summary>
    /// Representa la entidad de Usuario con soporte para seguridad basada en Roles (Composite)
    /// y persistencia de preferencias de personalización.
    /// </summary>
    public class UsuarioBE
    {
        #region "Propiedades de Perfil"

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        /// <summary>
        /// Identificador del idioma preferido del usuario.
        /// Sincronizado con la columna 'IdIdioma' de SQL.
        /// </summary>
        public int IdIdioma { get; set; }

        #endregion

        #region "Estado de Cuenta y Seguridad"

        /// <summary>
        /// Indica si el usuario debe ser redirigido obligatoriamente al cambio de password.
        /// Seteado en true cuando el administrador blanquea la clave o expira.
        /// </summary>
        public bool DebeCambiarPassword { get; set; }

        #endregion

        #region "Seguridad (Pattern Composite)"

        // Lista privada para manejo interno de la estructura de permisos
        private List<ComponenteBE> _permisos = new List<ComponenteBE>();

        /// <summary>
        /// Colección de permisos y roles asignados al usuario (Solo lectura).
        /// </summary>
        public IReadOnlyCollection<ComponenteBE> Permisos => _permisos.AsReadOnly();

        /// <summary>
        /// Agrega un componente (Permiso simple o Rol) a la estructura del usuario.
        /// Valida que no se agregue el mismo componente raíz dos veces.
        /// </summary>
        public void AgregarPermiso(ComponenteBE permiso)
        {
            if (permiso != null && !_permisos.Any(x => x.Id == permiso.Id))
            {
                _permisos.Add(permiso);
            }
        }

        /// <summary>
        /// Remueve un componente específico de la lista de permisos del usuario.
        /// </summary>
        public void QuitarPermiso(ComponenteBE permiso)
        {
            if (permiso != null)
            {
                var item = _permisos.FirstOrDefault(x => x.Id == permiso.Id);
                if (item != null)
                {
                    _permisos.Remove(item);
                }
            }
        }

        /// <summary>
        /// Limpia la colección de permisos actual para permitir una reasignación completa.
        /// </summary>
        public void LimpiarPermisos()
        {
            _permisos.Clear();
        }

        /// <summary>
        /// Valida de forma recursiva si el usuario posee el permiso solicitado (Tag).
        /// </summary>
        /// <param name="nombrePermiso">Nombre del permiso o Tag a validar.</param>
        public bool ValidarPermisos(string nombrePermiso)
        {
            foreach (var p in _permisos)
            {
                if (p.ValidarPermisos(nombrePermiso)) return true;
            }
            return false;
        }

        #endregion
    }
}