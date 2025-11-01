using ABS.Repositories;

namespace APP
{
    public class appUsuario
    {
        private readonly IUsuarioDbRepository _repository;

        public appUsuario(IUsuarioDbRepository repository)
        {
            _repository = repository;
        }

        public List<DOM.domUsuario> Traer() => _repository.Traer();
    }
}
