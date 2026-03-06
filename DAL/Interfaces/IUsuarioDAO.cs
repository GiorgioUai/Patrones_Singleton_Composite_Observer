using BE;

namespace DAL.Interfaces
{
    public interface IUsuarioDAO
    {
        UsuarioBE ValidarAcceso(string email, string passwordHash);
    }
}
