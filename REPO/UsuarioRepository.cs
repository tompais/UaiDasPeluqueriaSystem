using ABS.Repositories;
using CONTEXT;
using DOM;

namespace REPO;

/// <summary>
/// Repositorio para operaciones CRUD de usuarios
/// </summary>
public class UsuarioRepository(InMemoryContext context) : IUsuarioRepository
{
    private readonly InMemoryContext _context = context;

    public Usuario Crear(Usuario usuario)
    {
        _context.AgregarUsuario(usuario);
        return usuario;
    }

    public Usuario? ObtenerPorId(int id)
    {
        return _context.ObtenerUsuarioPorId(id);
    }

    public Usuario? ObtenerPorEmail(string email)
    {
        return _context.ObtenerUsuarioPorEmail(email);
    }

    public IEnumerable<Usuario> ObtenerTodos()
    {
        return _context.Usuarios;
    }

    public bool ExisteEmail(string email)
    {
        return _context.ExisteEmail(email);
    }
}
