namespace ABS.Repositories;

/// <summary>
/// Interfaz para el repositorio de usuarios que accede a base de datos
/// </summary>
public interface IUsuarioDbRepository
{
    List<DOM.domUsuario> Traer();
    DOM.domUsuario Crear(DOM.domUsuario usuario);
    bool ExisteEmail(string email);
}
