using ABS.Application;
using ABS.Repositories;
using ABS.Services;
using DOM;
using System.ComponentModel.DataAnnotations;

namespace APP;

/// <summary>
/// Servicio de lógica de negocio para usuarios
/// </summary>
public class UsuarioService(
  IUsuarioRepository usuarioRepository,
   IEncriptacionService encriptacionService) : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository = usuarioRepository;
    private readonly IEncriptacionService _encriptacionService = encriptacionService;

    public ResultadoOperacion<Usuario> CrearUsuario(CrearUsuarioRequest request)
    {
        // Validar Request
        var erroresValidacion = ValidarCrearUsuarioRequest(request);
        if (erroresValidacion.Count != 0)
        {
            return ResultadoOperacion<Usuario>.Error("Error de validación", erroresValidacion);
        }

        // Verificar que el email no exista
        if (_usuarioRepository.ExisteEmail(request.Email))
        {
            return ResultadoOperacion<Usuario>.Error("El email ya está registrado");
        }

        // Validar longitud de la clave (11 caracteres)
        if (request.Clave.Length != 11)
        {
            return ResultadoOperacion<Usuario>.Error("La clave debe tener exactamente 11 caracteres");
        }

        // Crear entidad de usuario usando el constructor
        var usuario = new Usuario(
            nombre: request.Nombre.Trim(),
    apellido: request.Apellido.Trim(),
         email: request.Email.Trim().ToLowerInvariant(),
            clave: _encriptacionService.Encriptar(request.Clave),
            rol: request.Rol
      );

        // Guardar en repositorio
        var usuarioCreado = _usuarioRepository.Crear(usuario);

        return ResultadoOperacion<Usuario>.Exito(usuarioCreado, "Usuario creado exitosamente");
    }

    public IEnumerable<Usuario> ObtenerTodos()
    {
        return _usuarioRepository.ObtenerTodos();
    }

    private List<string> ValidarCrearUsuarioRequest(CrearUsuarioRequest request)
    {
        var errores = new List<string>();

        // Validar Nombre
        if (string.IsNullOrWhiteSpace(request.Nombre))
        {
            errores.Add("El nombre es obligatorio");
        }
        else if (request.Nombre.Length > 50)
        {
            errores.Add("El nombre no puede superar los 50 caracteres");
        }

        // Validar Apellido
        if (string.IsNullOrWhiteSpace(request.Apellido))
        {
            errores.Add("El apellido es obligatorio");
        }
        else if (request.Apellido.Length > 80)
        {
            errores.Add("El apellido no puede superar los 80 caracteres");
        }

        // Validar Email
        if (string.IsNullOrWhiteSpace(request.Email))
        {
            errores.Add("El email es obligatorio");
        }
        else if (request.Email.Length > 180)
        {
            errores.Add("El email no puede superar los 180 caracteres");
        }
        else if (!EsEmailValido(request.Email))
        {
            errores.Add("El formato del email no es válido");
        }

        // Validar Clave
        if (string.IsNullOrWhiteSpace(request.Clave))
        {
            errores.Add("La clave es obligatoria");
        }

        // Nota: Rol no requiere validación adicional ya que es un enum

        return errores;
    }

    public bool EsEmailValido(string email)
    {
        return new EmailAddressAttribute().IsValid(email);
    }
}
