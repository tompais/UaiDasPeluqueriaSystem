using DOM;

namespace CONTEXT;

/// <summary>
/// Contexto en memoria para almacenar los datos de la aplicaci�n
/// </summary>
public class InMemoryContext
{
    private readonly List<Usuario> _usuarios = [];
    private int _siguienteId = 1;
    private readonly object _lock = new();

    public List<Usuario> Usuarios
    {
        get
        {
            lock (_lock)
            {
                return [.. _usuarios];
            }
        }
    }

    public void AgregarUsuario(Usuario usuario)
    {
        lock (_lock)
        {
            usuario.Id = _siguienteId++;
            _usuarios.Add(usuario);
        }
    }

    public Usuario? ObtenerUsuarioPorId(int id)
    {
        lock (_lock)
            return _usuarios.FirstOrDefault(u => u.Id == id);
    }

    public Usuario? ObtenerUsuarioPorEmail(string email)
    {
        lock (_lock)
            return _usuarios.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
    }

    public bool ExisteEmail(string email)
    {
        lock (_lock)
            return _usuarios.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
    }
}
