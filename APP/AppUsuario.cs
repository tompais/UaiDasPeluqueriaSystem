using ABS.Repositories;
using ABS.Services;

namespace APP
{
    public class AppUsuario(IUsuarioDbRepository repository, IEncriptacionService encriptacionService)
    {
        public List<DOM.DomUsuario> Traer() => repository.Traer();

        public DOM.DomUsuario Crear(string nombre, string apellido, string email, string clave, DOM.DomUsuario.RolUsuario rol)
        {
            var usuario = new DOM.DomUsuario
            {
                Nombre = nombre.Trim(),
                Apellido = apellido.Trim(),
                Email = email.Trim().ToLowerInvariant(),
                Clave = encriptacionService.Encriptar(clave),
                Rol = rol,
                Estado = DOM.DomUsuario.EstadoUsuario.Activo,
                DV = "" // Dígito verificador - por ahora vacío
            };

            return repository.Crear(usuario);
        }

        public bool ExisteEmail(string email) => repository.ExisteEmail(email);
    }
}
