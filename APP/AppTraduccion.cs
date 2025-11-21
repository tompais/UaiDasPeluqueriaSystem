using ABS.Repositories;
using SERV;

namespace APP
{
    public class AppTraduccion
    {
        private static ITraduccionDbRepository? _repository;
        private static readonly object _lock = new();

        public AppTraduccion(ITraduccionDbRepository repository)
        {
            lock (_lock)
            {
                _repository = repository;
            }
        }

        public static List<DOM.Traduccion> TraerPorIdioma(int idIdioma)
        {
            lock (_lock)
            {
                if (_repository == null)
                    throw new InvalidOperationException("El repositorio de traducciones no ha sido inicializado");

                return _repository.TraerPorIdioma(idIdioma);
            }
        }

        /// <summary>
        /// Traduce una palabra al idioma especificado
        /// </summary>
        /// <param name="palabra">Palabra original a traducir</param>
        /// <param name="idIdioma">ID del idioma al que se desea traducir</param>
        /// <returns>La traducción si existe, o la palabra original si no se encuentra</returns>
        public static string Traducir(string palabra, int idIdioma)
        {
            var traducciones = TraerPorIdioma(idIdioma);
            return Traductor.Traducir(palabra, traducciones);
        }
    }
}
