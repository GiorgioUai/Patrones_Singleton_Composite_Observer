using System.Collections.Generic;
using BE;

namespace DAL.Interfaces
{
    /// <summary>
    /// Contrato para la persistencia de componentes de seguridad.
    /// Define operaciones polimórficas para manejar la estructura Composite.
    /// </summary>
    public interface IPermisoDAL
    {
        void GuardarPermisoSimple(PermisoBE pPermiso);

        List<PermisoBE> ListarSimples();

        /// <summary>
        /// Elimina un componente (Permiso o Rol) de forma polimórfica.
        /// </summary>
        void EliminarPermiso(ComponenteBE pComponente);

        List<ComponenteBE> ListarTodo();

        void GuardarEstructuraRol(CompuestoBE pRol);

        /// <summary>
        /// Obtiene la estructura completa de permisos de un usuario para la sesión activa.
        /// </summary>
        List<ComponenteBE> ObtenerPermisosUsuario(int usuarioId);
    }
}