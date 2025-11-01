using DOM;
using System.Data.SqlClient;

namespace ABS.Repositories;

/// <summary>
/// Interfaz para el repositorio de usuarios que accede a base de datos
/// </summary>
public interface IUsuarioDbRepository
{
    List<DOM.DomUsuario> Traer();
    DOM.DomUsuario Crear(DOM.DomUsuario usuario);
    bool ExisteEmail(string email);
    List<DomUsuario> CompletarLista(SqlDataReader dr, List<DomUsuario> lista);
}
