using DOM;
using Microsoft.Data.SqlClient;

namespace ABS.Repositories;

/// <summary>
/// Interfaz para el repositorio de usuarios que accede a base de datos
/// </summary>
public interface IUsuarioDbRepository
{
    List<DOM.DomUsuario> Traer();
    DOM.DomUsuario? TraerPorId(int id);
    DOM.DomUsuario Crear(DOM.DomUsuario usuario);
    void Modificar(DOM.DomUsuario usuario);
    void Eliminar(int id);
    bool ExisteEmail(string email);
    bool ExisteEmailExcluyendoId(string email, int idExcluir);
    List<DomUsuario> CompletarLista(SqlDataReader dr, List<DomUsuario> lista);
}
