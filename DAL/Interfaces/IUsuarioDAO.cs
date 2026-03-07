using BE;

namespace DAL.Interfaces
{
    /// <summary>
    /// Define el contrato para el acceso a datos de la entidad Usuario.
    /// </summary>
    public interface IUsuarioDAO
    {
        /// <summary>
        /// Valida las credenciales de un usuario contra la base de datos.
        /// </summary>
        UsuarioBE ValidarAcceso(string email, string passwordHash);

        /// <summary>
        /// Persiste un nuevo usuario y le asigna un Rol por defecto.
        /// </summary>
        /// <param name="pUsuario">Entidad con los datos personales.</param>
        /// <param name="pPasswordHash">Contraseña ya cifrada en la BL.</param>
        /// <param name="pIdRolBase">ID del Rol que se asignará automáticamente.</param>
        /// <returns>True si la transacción fue exitosa.</returns>
        bool Registrar(UsuarioBE pUsuario, string pPasswordHash, int pIdRolBase);
    }
}