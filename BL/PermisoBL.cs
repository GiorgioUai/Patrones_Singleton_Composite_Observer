using System;
using System.Collections.Generic;
using BE;
using DAL;

namespace BL
{
    public class PermisoBL
    {
        private readonly IPermisoDAL _permisoDAL;

        // Constructor para la UI (Mantiene el aislamiento de la DAL en la vista)
        public PermisoBL() : this(new PermisoDAL())
        {
        }

        // Constructor para Inyección/Testeo
        public PermisoBL(IPermisoDAL permisoDAL)
        {
            _permisoDAL = permisoDAL ?? throw new ArgumentNullException(nameof(permisoDAL));
        }

        public void GuardarPermisoSimple(PermisoBE pPermiso)
        {
            if (pPermiso == null)
            {
                throw new ArgumentNullException(nameof(pPermiso), "El objeto permiso no puede ser nulo.");
            }

            if (string.IsNullOrWhiteSpace(pPermiso.Nombre))
            {
                throw new Exception("El nombre del permiso es obligatorio para su creación.");
            }
            _permisoDAL.GuardarPermisoSimple(pPermiso);
        }

        public List<PermisoBE> ListarPermisosSimples()
        {
            return _permisoDAL.ListarSimples();
        }

        // NUEVO: Método de eliminación con validación de integridad lógica
        public void EliminarPermiso(PermisoBE pPermiso)
        {
            // Validamos que el objeto no sea nulo
            if (pPermiso == null)
            {
                throw new ArgumentNullException(nameof(pPermiso), "Debe seleccionar un permiso para eliminar.");
            }

            // Validamos que tenga un ID válido (que exista en la base de datos)
            if (pPermiso.Id <= 0)
            {
                throw new Exception("El permiso seleccionado no tiene una identidad válida para ser eliminado.");
            }

            // Nota para el futuro: Aquí invocaremos la validación de dependencias 
            // (si está en uso por familias o usuarios) una vez tengamos las tablas relacionales.

            _permisoDAL.EliminarPermiso(pPermiso);
        }
    }
}