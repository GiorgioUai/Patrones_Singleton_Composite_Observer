using BE;

namespace DAL
{
    public interface IUsuarioDAO
    {
        UsuarioBE ValidarAcceso(string email, string passwordHash);
    }
}
