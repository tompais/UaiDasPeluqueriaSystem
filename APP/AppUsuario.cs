using ABS.Repositories;
using ABS.Services;

namespace APP
{
    public class AppUsuario(IUsuarioDbRepository repository, IEncriptacionService encriptacionService)
    {
        public List<DOM.DomUsuario> Traer() => repository.Traer();

        public DOM.DomUsuario? TraerPorId(int id) => repository.TraerPorId(id);

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

        public void Modificar(int id, string nombre, string apellido, string email, string? clave, 
            DOM.DomUsuario.RolUsuario rol, DOM.DomUsuario.EstadoUsuario estado)
        {
            var usuarioExistente = repository.TraerPorId(id);
            if (usuarioExistente == null)
                throw new InvalidOperationException($"Usuario con ID {id} no encontrado");

            // Validar que el email no esté en uso por otro usuario
            if (repository.ExisteEmailExcluyendoId(email.Trim().ToLowerInvariant(), id))
                throw new InvalidOperationException("El email ya está registrado por otro usuario");

            var usuario = new DOM.DomUsuario
            {
                ID = id,
                Nombre = nombre.Trim(),
                Apellido = apellido.Trim(),
                Email = email.Trim().ToLowerInvariant(),
                // Si se proporciona una nueva clave, la encripta; si no, mantiene la existente
                Clave = string.IsNullOrWhiteSpace(clave) 
                    ? usuarioExistente.Clave 
                    : encriptacionService.Encriptar(clave),
                Rol = rol,
                Estado = estado,
                DV = usuarioExistente.DV,
                Fecha_Agregar = usuarioExistente.Fecha_Agregar,
                FechaModificacion = DateTime.Now
            };

            repository.Modificar(usuario);
        }

        public void Eliminar(int id) => repository.Eliminar(id);

        public bool ExisteEmail(string email) => repository.ExisteEmail(email);

        public bool ExisteEmailExcluyendoId(string email, int idExcluir) => 
            repository.ExisteEmailExcluyendoId(email, idExcluir);
    }
}
