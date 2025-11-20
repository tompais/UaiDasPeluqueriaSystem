namespace DOM
{
    /// <summary>
    /// Clase que representa una patente (permiso individual).
    /// Implementa el patrón Leaf del patrón Composite.
    /// </summary>
    public class Patente : Elemento
    {
        /// <summary>
        /// Muestra la información de la patente.
        /// </summary>
        public override void Mostrar() => Console.WriteLine($"Patente: {Nombre}");
    }
}
