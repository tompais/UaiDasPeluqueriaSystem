using ABS.Repositories;
using SERV;

namespace APP
{
    public class AppTraduccion(ITraduccionDbRepository repository)
    {
        public List<DOM.Traduccion> TraerPorIdioma(int idIdioma) => repository.TraerPorIdioma(idIdioma);

        /// <summary>
        /// Traduce una palabra al idioma especificado
        /// </summary>
        /// <param name="palabra">Palabra original a traducir</param>
        /// <param name="idIdioma">ID del idioma al que se desea traducir</param>
        /// <returns>La traducción si existe, o la palabra original si no se encuentra</returns>
        public string Traducir(string palabra, int idIdioma)
        {
            var traducciones = TraerPorIdioma(idIdioma);
            return Traductor.Traducir(palabra, traducciones);
        }
    }
}
