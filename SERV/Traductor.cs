namespace SERV
{
    /// <summary>
    /// Clase que proporciona servicios de traducción de palabras
    /// </summary>
    public static class Traductor
    {
        /// <summary>
        /// Traduce una palabra usando la colección de traducciones proporcionada
        /// </summary>
        /// <param name="palabra">Palabra original a traducir</param>
        /// <param name="traducciones">Colección de traducciones disponibles</param>
        /// <returns>La traducción si existe, o la palabra original si no se encuentra</returns>
        public static string Traducir(string palabra, IEnumerable<DOM.Traduccion> traducciones)
        {
            var resultado = traducciones.FirstOrDefault(t => t.PalabraOriginal.Equals(palabra, StringComparison.OrdinalIgnoreCase));
            return resultado?.PalabraIdioma ?? palabra;
        }
    }
}
