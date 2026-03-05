using System.Collections.Generic;
using BE;

namespace DAL
{
    public interface IPermisoDAL
    {
        void GuardarPermisoSimple(PermisoBE pPermiso);
        List<PermisoBE> ListarSimples();
        void EliminarPermiso(PermisoBE pPermiso);

        // Nuevo método para obtener la estructura completa de permisos de un usuario
        List<ComponenteBE> ObtenerPermisosUsuario(int usuarioId);
    }
}