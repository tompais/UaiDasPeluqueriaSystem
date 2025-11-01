namespace DOM
{
    public class DomUsuario
    {
        public int ID { get; init; }
        public required string Apellido { get; set; }
        public required string Nombre { get; set; }
        public required string Email { get; set; }
        public RolUsuario Rol { get; set; }
        public EstadoUsuario Estado { get; set; }
        public required string Clave { get; set; }
        public required string DV { get; set; }
        public DateTime Fecha_Agregar { get; init; } = DateTime.Now;

        /// <summary>
        /// Estados posibles para un usuario
        /// </summary>
        public enum EstadoUsuario
        {
            Activo = 0,
            Baja = 1
        }

        /// <summary>
        /// Roles disponibles en el sistema
        /// </summary>
        public enum RolUsuario
        {
            Cliente = 0,
            Empleado = 1,
            Supervisor = 2,
            Administrador = 3
        }
    }
}
