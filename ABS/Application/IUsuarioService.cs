using DOM;

namespace ABS.Application;

/// <summary>
/// Interfaz para la lógica de negocio de usuarios
/// </summary>
public interface IUsuarioService
{
    ResultadoOperacion<Usuario> CrearUsuario(CrearUsuarioRequest request);
    IEnumerable<Usuario> ObtenerTodos();
}

/// <summary>
/// Request para crear un nuevo usuario
/// </summary>
public record CrearUsuarioRequest(
    string Nombre,
    string Apellido,
    string Email,
    string Clave,
    Usuario.RolUsuario Rol
);

/// <summary>
/// Resultado de una operación con información de éxito o error
/// </summary>
public record ResultadoOperacion<T>(
    bool Exitoso,
    string Mensaje,
    T? Datos,
    List<string> Errores
)
{
    public static ResultadoOperacion<T> Exito(T datos, string mensaje = "Operación exitosa")
    {
    return new ResultadoOperacion<T>(
    Exitoso: true,
     Mensaje: mensaje,
    Datos: datos,
            Errores: []
        );
    }

    public static ResultadoOperacion<T> Error(string mensaje, List<string>? errores = null)
    {
   return new ResultadoOperacion<T>(
            Exitoso: false,
            Mensaje: mensaje,
  Datos: default,
   Errores: errores ?? []
     );
    }
}
