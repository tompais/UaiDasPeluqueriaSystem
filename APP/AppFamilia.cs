using ABS.Repositories;

namespace APP
{
    public class AppFamilia(IFamiliaDbRepository repository)
    {
        public List<DOM.Familia> Traer() => repository.Traer();

        public DOM.Familia? TraerPorId(int id) => repository.TraerPorId(id);

        public DOM.Familia Crear(string nombre)
        {
            var familia = new DOM.Familia
            {
                Nombre = nombre.Trim()
            };

            return repository.Crear(familia);
        }

        public void Modificar(int id, string nombre)
        {
            var familiaExistente = repository.TraerPorId(id);
            if (familiaExistente == null)
                throw new InvalidOperationException($"Familia con ID {id} no encontrada");

            var familia = new DOM.Familia
            {
                ID = id,
                Nombre = nombre.Trim()
            };

            repository.Modificar(familia);
        }

        public void Eliminar(int id) => repository.Eliminar(id);

        public void AsignarElemento(int idFamilia, int idElemento) => 
            repository.AsignarElemento(idFamilia, idElemento);

        public void RemoverElemento(int idFamilia, int idElemento) => 
            repository.RemoverElemento(idFamilia, idElemento);

        public List<DOM.Elemento> TraerElementosDeFamilia(int idFamilia) => 
            repository.TraerElementosDeFamilia(idFamilia);
    }
}
