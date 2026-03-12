using BE;
using System.Collections.Generic;

namespace DAL.Interfaces
{
    /// <summary>
    /// Define el contrato para el acceso a datos de la entidad Usuario.
    /// </summary>
    public interface IUsuarioDAO
    {
        #region "Lectura"

        /// <summary>
        /// Valida las credenciales de un usuario contra la base de datos.
        /// </summary>
        UsuarioBE ValidarAcceso(string email, string passwordHash);

        /// <summary>
        /// Obtiene la lista completa de usuarios registrados (sin cargar seguridad por performance).
        /// </summary>
        List<UsuarioBE> ListarTodos();

        /// <summary>
        /// Carga la estructura de seguridad (Roles/Permisos) para un usuario específico.
        /// </summary>
        void ObtenerSeguridadUsuario(UsuarioBE pUsuario);

        #endregion

        #region "Escritura"

        /// <summary>
        /// Persiste un nuevo usuario y le asigna un Rol por defecto.
        /// </summary>
        bool Registrar(UsuarioBE pUsuario, string pPasswordHash, int pIdRolBase);

        /// <summary>
        /// Actualiza de forma atómica los roles y permisos asignados a un usuario.
        /// </summary>
        bool GuardarPermisos(UsuarioBE pUsuario);

        /// <summary>
        /// Actualiza la contraseña y setea DebeCambiarPassword en false.
        /// </summary>
        bool ActualizarPassword(int idUsuario, string nuevoPasswordHash);

        /// <summary>
        /// Setea el flag DebeCambiarPassword en true para forzar el cambio al próximo inicio.
        /// </summary>
        bool ForzarCambioPassword(int idUsuario);

        #endregion
    }
}