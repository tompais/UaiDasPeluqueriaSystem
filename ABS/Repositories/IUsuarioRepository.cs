using DOM;

namespace ABS.Repositories;

/// <summary>
/// Interfaz para operaciones de repositorio de usuarios
/// </summary>
public interface IUsuarioRepository
{
    Usuario Crear(Usuario usuario);
    Usuario? ObtenerPorId(int id);
    Usuario? ObtenerPorEmail(string email);
    IEnumerable<Usuario> ObtenerTodos();
    bool ExisteEmail(string email);
}
