using System;
using System.Collections.Generic;
using System.Linq;
using BE;
using DAL;

namespace BL
{
    /// <summary>
    /// Gestiona la lógica de negocio para la seguridad (Roles y Permisos).
    /// </summary>
    public class PermisoBL
    {
        #region "Atributos / Variables Privadas"

        private readonly IPermisoDAL _permisoDAL;

        #endregion

        #region "Constructores"

        public PermisoBL() : this(new PermisoDAL()) { }

        public PermisoBL(IPermisoDAL permisoDAL)
        {
            _permisoDAL = permisoDAL ?? throw new ArgumentNullException(nameof(permisoDAL));
        }

        #endregion

        #region "Gestión de Permisos Simples"

        public void GuardarPermisoSimple(PermisoBE pPermiso)
        {
            if (pPermiso == null) throw new ArgumentNullException(nameof(pPermiso));
            if (string.IsNullOrWhiteSpace(pPermiso.Nombre)) throw new Exception("Nombre obligatorio.");
            _permisoDAL.GuardarPermisoSimple(pPermiso);
        }

        public List<PermisoBE> ListarPermisosSimples() => _permisoDAL.ListarSimples();

        public void EliminarPermiso(PermisoBE pPermiso)
        {
            if (pPermiso == null || pPermiso.Id <= 0) throw new Exception("Entidad no válida.");
            _permisoDAL.EliminarPermiso(pPermiso);
        }

        #endregion

        #region "Gestión de Roles (Composite)"

        public List<ComponenteBE> ListarTodo() => _permisoDAL.ListarTodo();

        /// <summary>
        /// Persiste el Rol y sincroniza la memoria RAM.
        /// </summary>
        public void GuardarRol(CompuestoBE pRol)
        {
            if (pRol == null) throw new ArgumentNullException(nameof(pRol));
            _permisoDAL.GuardarEstructuraRol(pRol);
            pRol.LimpiarEstados(); // Sincronización post-commit
        }

        public List<ComponenteBE> ObtenerCatalogoParaRol(int idRolActual)
        {
            return _permisoDAL.ListarTodo().Where(c => c.Id != idRolActual).ToList();
        }

        #endregion

        #region "Algoritmos de Validación Jerárquica"

        public string ObtenerContenedorRecursivo(ComponenteBE raiz, int idBuscado)
        {
            foreach (var hijo in raiz.ObtenerHijos())
            {
                if (hijo.Id == idBuscado) return raiz.Nombre;
                if (hijo is CompuestoBE)
                {
                    var res = ObtenerContenedorRecursivo(hijo, idBuscado);
                    if (res != null) return res;
                }
            }
            return null;
        }

        #endregion
    }
}