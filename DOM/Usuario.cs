namespace DOM;

/// <summary>
/// Entidad de dominio que representa un usuario del sistema
/// </summary>
/// <remarks>
/// Constructor para crear un nuevo usuario con datos obligatorios
/// </remarks>
/// <param name="nombre">Nombre del usuario</param>
/// <param name="apellido">Apellido del usuario</param>
/// <param name="email">Email del usuario</param>
/// <param name="clave">Clave encriptada del usuario</param>
/// <param name="rol">Rol del usuario (Cliente por defecto)</param>
public class Usuario(string nombre, string apellido, string email, string clave, Usuario.RolUsuario rol = Usuario.RolUsuario.Cliente)
{
    public int Id { get; set; }
    public string Nombre { get; private set; } = nombre;
    public string Apellido { get; private set; } = apellido;
    public string Email { get; private set; } = email;
    public string Clave { get; private set; } = clave;
    public EstadoUsuario Estado { get; set; } = EstadoUsuario.Activo;
    public RolUsuario Rol { get; set; } = rol;
    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    public string NombreCompleto => $"{Nombre} {Apellido}";

    /// <summary>
    /// Estados posibles para un usuario
    /// </summary>
    public enum EstadoUsuario
    {
        Activo = 0,
        Baja = 1
    }

    /// <summary>
    /// Roles disponibles en el sistema
    /// </summary>
    public enum RolUsuario
    {
        Cliente = 0,
        Empleado = 1,
        Supervisor = 2,
        Administrador = 3
    }
}
