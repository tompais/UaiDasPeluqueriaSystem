namespace DOM
{
    public class domUsuario
    {
        public int ID { get; init; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public Usuario.RolUsuario Rol { get; set; }
        public Usuario.EstadoUsuario Estado { get; set; }
        public string Clave { get; set; }
        public string DV { get; set; }
        public DateTime Fecha_Agregar { get; init; } = DateTime.Now;
    }
}
