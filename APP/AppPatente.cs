using ABS.Repositories;

namespace APP
{
    public class AppPatente(IPatenteDbRepository repository)
    {
        public List<DOM.Patente> Traer() => repository.Traer();

        public DOM.Patente? TraerPorId(int id) => repository.TraerPorId(id);

        public DOM.Patente Crear(string nombre)
        {
            var patente = new DOM.Patente
            {
                Nombre = nombre.Trim()
            };

            return repository.Crear(patente);
        }

        public void Modificar(int id, string nombre)
        {
            var patenteExistente = repository.TraerPorId(id);
            if (patenteExistente == null)
                throw new InvalidOperationException($"Patente con ID {id} no encontrada");

            var patente = new DOM.Patente
            {
                ID = id,
                Nombre = nombre.Trim()
            };

            repository.Modificar(patente);
        }

        public void Eliminar(int id) => repository.Eliminar(id);
    }
}
