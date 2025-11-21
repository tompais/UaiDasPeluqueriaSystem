namespace DOM
{
    /// <summary>
    /// Entidad que representa una palabra traducida en el diccionario
    /// </summary>
    public class Traduccion
    {
        public int ID { get; init; }
        public int IDIdioma { get; init; }
        public required string PalabraOriginal { get; init; }
        public required string PalabraIdioma { get; init; }
    }
}
