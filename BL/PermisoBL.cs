using System;
using System.Collections.Generic;
using System.Linq;
using BE;
using DAL;
using DAL.Interfaces;

namespace BL
{
    public class PermisoBL
    {
        private readonly IPermisoDAL _permisoDAL;

        public PermisoBL() : this(new PermisoDAL()) { }
        public PermisoBL(IPermisoDAL permisoDAL) { _permisoDAL = permisoDAL ?? throw new ArgumentNullException(nameof(permisoDAL)); }

        public bool ExisteNombre(string pNombre)
        {
            if (string.IsNullOrWhiteSpace(pNombre)) return false;
            return _permisoDAL.ListarTodo().Any(x => x.Nombre.Equals(pNombre.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        public void GuardarPermisoSimple(PermisoBE pPermiso)
        {
            if (pPermiso == null) throw new ArgumentNullException(nameof(pPermiso));
            if (pPermiso.Id == 0 && ExisteNombre(pPermiso.Nombre)) throw new Exception($"El permiso '{pPermiso.Nombre}' ya existe.");
            _permisoDAL.GuardarPermisoSimple(pPermiso);
        }

        public List<PermisoBE> ListarPermisosSimples() => _permisoDAL.ListarSimples();
        public void EliminarPermiso(ComponenteBE pComp) => _permisoDAL.EliminarPermiso(pComp);
        public List<ComponenteBE> ListarTodo() => _permisoDAL.ListarTodo();

        public ComponenteBE ObtenerEstructuraCompleta(int idComponente)
        {
            var seleccionado = _permisoDAL.ListarTodo().FirstOrDefault(x => x.Id == idComponente);
            if (seleccionado != null && seleccionado is CompuestoBE)
            {
                ((PermisoDAL)_permisoDAL).LlenarHijos(seleccionado);
                seleccionado.LimpiarEstados();
            }
            return seleccionado;
        }

        public void GuardarRol(CompuestoBE pRol)
        {
            if (pRol == null) throw new ArgumentNullException(nameof(pRol));
            if (pRol.Id == 0 && ExisteNombre(pRol.Nombre)) throw new Exception($"El nombre '{pRol.Nombre}' ya existe.");
            _permisoDAL.GuardarEstructuraRol(pRol);
            pRol.LimpiarEstados();
        }

        public List<ComponenteBE> ObtenerCatalogoParaRol(int idRolActual) => _permisoDAL.ListarTodo().Where(c => c.Id != idRolActual).ToList();

        public string ObtenerContenedorRecursivo(ComponenteBE raiz, int idBuscado)
        {
            if (raiz.Id == idBuscado) return raiz.Nombre;

            foreach (var hijo in raiz.ObtenerHijos())
            {
                if (hijo.Id == idBuscado) return raiz.Nombre;
                if (hijo is CompuestoBE) { var res = ObtenerContenedorRecursivo(hijo, idBuscado); if (res != null) return res; }
            }
            return null;
        }

        /// <summary>
        /// Blindaje Superior: Valida si el nuevo elemento o CUALQUIERA de sus hijos ya existen en la raíz.
        /// </summary>
        public bool EstructuraContieneDuplicados(ComponenteBE raizDestino, ComponenteBE nuevoElemento, out string nombreConflicto)
        {
            nombreConflicto = string.Empty;

            // 1. Validamos el elemento principal (sea patente o el rol mismo)
            var contenedor = ObtenerContenedorRecursivo(raizDestino, nuevoElemento.Id);
            if (contenedor != null)
            {
                nombreConflicto = nuevoElemento.Nombre;
                return true;
            }

            // 2. Si es un rol, validamos recursivamente todos sus hijos contra la raíz destino
            if (nuevoElemento is CompuestoBE nuevoRol)
            {
                foreach (var hijoNuevo in nuevoRol.ObtenerHijos())
                {
                    // Llamada recursiva interna para validar cada rama del nuevo elemento
                    if (EstructuraContieneDuplicados(raizDestino, hijoNuevo, out nombreConflicto))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        public void CargarHijos(ComponenteBE pComponente)
        {
            if (pComponente is CompuestoBE)
            {
                // Invocamos el método de la DAL que llena la lista interna de hijos
                _permisoDAL.LlenarHijos(pComponente);
            }
        }
    }
}