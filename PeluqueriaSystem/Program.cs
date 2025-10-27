namespace PeluqueriaSystem
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Configurar Dependency Injection
            DependencyInjectionContainer.ConfigurarServicios();

            ApplicationConfiguration.Initialize();

            // Obtener el formulario principal desde el contenedor DI
            var formPrincipal = DependencyInjectionContainer.ObtenerServicio<FormPrincipal>();
            Application.Run(formPrincipal);
        }
    }
}