using ABS.Application;
using ABS.Repositories;
using ABS.Services;
using APP;
using CONTEXT;
using Microsoft.Extensions.DependencyInjection;
using REPO;
using SERV;

namespace PeluqueriaSystem;

/// <summary>
/// Configurador del contenedor de Dependency Injection
/// </summary>
public static class DependencyInjectionContainer
{
    private static ServiceProvider? _serviceProvider;

    public static void ConfigurarServicios()
    {
        var services = new ServiceCollection();

        // Contexto (Singleton para mantener datos en memoria)
        services.AddSingleton<InMemoryContext>();

        // Repositorios
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();

        // Servicios
        services.AddScoped<IEncriptacionService, EncriptacionService>();
        services.AddScoped<IUsuarioService, UsuarioService>();

        // Formularios
        services.AddTransient<FormPrincipal>();
        services.AddTransient<FormUsuarios>();
        services.AddTransient<FormAltaUsuario>();

        _serviceProvider = services.BuildServiceProvider();
    }

    public static T ObtenerServicio<T>() where T : notnull
    {
        if (_serviceProvider == null)
            throw new InvalidOperationException("El contenedor de servicios no ha sido configurado. Llame a ConfigurarServicios() primero.");

        return _serviceProvider.GetRequiredService<T>();
    }
}
