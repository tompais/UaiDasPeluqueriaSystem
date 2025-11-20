namespace DOM
{
    /// <summary>
    /// Clase que representa una familia de permisos.
    /// Implementa el patrón Composite, puede contener patentes y/o otras familias.
    /// </summary>
    public class Familia : Elemento
    {
        private readonly List<Elemento> _elementos = [];

        /// <summary>
        /// Obtiene los elementos contenidos en esta familia.
        /// </summary>
        public IReadOnlyList<Elemento> Elementos => _elementos.AsReadOnly();

        /// <summary>
        /// Agrega un elemento (Patente o Familia) a esta familia.
        /// </summary>
        /// <param name="elemento">Elemento a agregar</param>
        public void Agregar(Elemento elemento) => _elementos.Add(elemento);

        /// <summary>
        /// Remueve un elemento de esta familia.
        /// </summary>
        /// <param name="elemento">Elemento a remover</param>
        public void Remover(Elemento elemento) => _elementos.Remove(elemento);

        /// <summary>
        /// Limpia todos los elementos de esta familia.
        /// </summary>
        public void Limpiar() => _elementos.Clear();

        /// <summary>
        /// Muestra la información de la familia y todos sus elementos recursivamente.
        /// </summary>
        public override void Mostrar()
        {
            Console.WriteLine($"Familia: {Nombre}");
            _elementos.ForEach(e => e.Mostrar());
        }

        /// <summary>
        /// Obtiene todos los permisos (patentes) de forma recursiva.
        /// Utiliza LINQ para recorrer y filtrar la jerarquía.
        /// </summary>
        /// <returns>Lista de todas las patentes contenidas en esta familia</returns>
        public List<Patente> ObtenerTodasLasPatentes() =>
            _elementos
                .SelectMany(e => e is Patente p 
                    ? [p] 
                    : e is Familia f 
                        ? f.ObtenerTodasLasPatentes() 
                        : [])
                .ToList();
    }
}
