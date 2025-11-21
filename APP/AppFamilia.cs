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

        public void AsignarPatente(int idFamilia, int idPatente) => 
            repository.AsignarPatente(idFamilia, idPatente);

        public void AsignarFamiliaHija(int idFamiliaPadre, int idFamiliaHija) => 
            repository.AsignarFamiliaHija(idFamiliaPadre, idFamiliaHija);

        public void RemoverPatente(int idFamilia, int idPatente) => 
            repository.RemoverPatente(idFamilia, idPatente);

        public void RemoverFamiliaHija(int idFamiliaPadre, int idFamiliaHija) => 
            repository.RemoverFamiliaHija(idFamiliaPadre, idFamiliaHija);

        public List<DOM.Patente> TraerPatentesDeFamilia(int idFamilia) => 
            repository.TraerPatentesDeFamilia(idFamilia);

        public List<DOM.Familia> TraerFamiliasHijas(int idFamilia) => 
            repository.TraerFamiliasHijas(idFamilia);
    }
}
