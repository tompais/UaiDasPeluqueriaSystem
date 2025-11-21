namespace DOM
{
    /// <summary>
    /// Clase que representa una familia de permisos.
    /// Implementa el patrón Composite, puede contener patentes y/o otras familias.
    /// </summary>
    public class Familia
    {
        public int ID { get; init; }
        public required string Nombre { get; set; }
        
        private readonly List<Patente> _patentes = [];
        private readonly List<Familia> _familias = [];

        /// <summary>
        /// Obtiene las patentes contenidas en esta familia.
        /// </summary>
        public IReadOnlyList<Patente> Patentes => _patentes.AsReadOnly();

        /// <summary>
        /// Obtiene las familias hijas contenidas en esta familia.
        /// </summary>
        public IReadOnlyList<Familia> Familias => _familias.AsReadOnly();

        /// <summary>
        /// Agrega una patente a esta familia.
        /// </summary>
        /// <param name="patente">Patente a agregar</param>
        public void AgregarPatente(Patente patente) => _patentes.Add(patente);

        /// <summary>
        /// Agrega una familia hija a esta familia.
        /// </summary>
        /// <param name="familia">Familia a agregar</param>
        public void AgregarFamilia(Familia familia) => _familias.Add(familia);

        /// <summary>
        /// Remueve una patente de esta familia.
        /// </summary>
        /// <param name="patente">Patente a remover</param>
        public void RemoverPatente(Patente patente) => _patentes.Remove(patente);

        /// <summary>
        /// Remueve una familia hija de esta familia.
        /// </summary>
        /// <param name="familia">Familia a remover</param>
        public void RemoverFamilia(Familia familia) => _familias.Remove(familia);

        /// <summary>
        /// Limpia todos los elementos de esta familia.
        /// </summary>
        public void Limpiar()
        {
            _patentes.Clear();
            _familias.Clear();
        }

        /// <summary>
        /// Obtiene todos los permisos (patentes) de forma recursiva.
        /// Utiliza LINQ para recorrer y filtrar la jerarquía.
        /// </summary>
        /// <returns>Lista de todas las patentes contenidas en esta familia</returns>
        public List<Patente> ObtenerTodasLasPatentes() =>
            _patentes
                .Concat(_familias.SelectMany(f => f.ObtenerTodasLasPatentes()))
                .ToList();
    }
}
