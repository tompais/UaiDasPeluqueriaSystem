using ABS.Repositories;
using ABS.Services;

namespace APP
{
    public class appUsuario
    {
        private readonly IUsuarioDbRepository _repository;
        private readonly IEncriptacionService _encriptacionService;

        public appUsuario(IUsuarioDbRepository repository, IEncriptacionService encriptacionService)
        {
            _repository = repository;
            _encriptacionService = encriptacionService;
        }

        public List<DOM.domUsuario> Traer() => _repository.Traer();

        public DOM.domUsuario Crear(string nombre, string apellido, string email, string clave, DOM.Usuario.RolUsuario rol)
        {
            var usuario = new DOM.domUsuario
            {
                Nombre = nombre.Trim(),
                Apellido = apellido.Trim(),
                Email = email.Trim().ToLowerInvariant(),
                Clave = _encriptacionService.Encriptar(clave),
                Rol = rol,
                Estado = DOM.Usuario.EstadoUsuario.Activo,
                DV = "" // Dígito verificador - por ahora vacío
            };

            return _repository.Crear(usuario);
        }

        public bool ExisteEmail(string email) => _repository.ExisteEmail(email);
    }
}
