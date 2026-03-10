using System.Collections.Generic;
using BE;
using System.Data.SqlClient;

namespace DAL.Interfaces
{
    /// <summary>
    /// Contrato para la persistencia de componentes de seguridad.
    /// Define operaciones polimórficas para manejar la estructura Composite de Roles y Permisos.
    /// </summary>
    public interface IPermisoDAL
    {
        #region "Operaciones de Lectura e Hidratación"

        /// <summary>
        /// Recupera la nómina completa de permisos simples (Patentes) registrados en el sistema.
        /// </summary>
        /// <returns>Lista de objetos PermisoBE.</returns>
        List<PermisoBE> ListarSimples();

        /// <summary>
        /// Obtiene la lista completa de componentes (Roles y Permisos) en su nivel raíz.
        /// </summary>
        /// <returns>Colección polimórfica de ComponenteBE.</returns>
        List<ComponenteBE> ListarTodo();

        /// <summary>
        /// Obtiene la estructura completa de permisos asignada a un usuario específico.
        /// </summary>
        /// <param name="usuarioId">ID único del usuario.</param>
        /// <returns>Colección de componentes asignados.</returns>
        List<ComponenteBE> ObtenerPermisosUsuario(int usuarioId);

        /// <summary>
        /// Hidrata un componente cargando de forma recursiva sus componentes hijos.
        /// </summary>
        /// <param name="pComponente">Componente de tipo Rol (Compuesto) a hidratar.</param>
        void LlenarHijos(ComponenteBE pComponente);

        /// <summary>
        /// Sobrecarga para hidratación recursiva utilizando una conexión activa o transacción.
        /// </summary>
        /// <param name="pComponente">Componente a hidratar.</param>
        /// <param name="connection">Conexión SQL activa.</param>
        void LlenarHijos(ComponenteBE pComponente, SqlConnection connection);

        #endregion

        #region "Operaciones de Escritura y Mantenimiento"

        /// <summary>
        /// Persiste un permiso simple en el repositorio de datos.
        /// </summary>
        /// <param name="pPermiso">Entidad permiso a guardar.</param>
        void GuardarPermisoSimple(PermisoBE pPermiso);

        /// <summary>
        /// Persiste la estructura jerárquica y relaciones de un Rol (Componente Compuesto).
        /// </summary>
        /// <param name="pRol">Entidad Rol con sus hijos asignados.</param>
        void GuardarEstructuraRol(CompuestoBE pRol);

        /// <summary>
        /// Elimina un componente del sistema de forma polimórfica mediante su identificador.
        /// </summary>
        /// <param name="pComponente">Entidad componente a eliminar.</param>
        void EliminarPermiso(ComponenteBE pComponente);

        #endregion
    }
}