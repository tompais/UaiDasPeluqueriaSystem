namespace DOM
{
    /// <summary>
    /// Clase abstracta base para implementar el patrón Composite.
    /// Representa un elemento que puede ser una Patente (Leaf) o una Familia (Composite).
    /// </summary>
    public abstract class Elemento
    {
        public int ID { get; set; }
        public required string Nombre { get; set; }
        
        /// <summary>
        /// Método abstracto para mostrar el elemento.
        /// Cada implementación define su comportamiento específico.
        /// </summary>
        public abstract void Mostrar();
    }
}
