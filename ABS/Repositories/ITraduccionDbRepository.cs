using DOM;
using Microsoft.Data.SqlClient;

namespace ABS.Repositories;

/// <summary>
/// Interfaz para el repositorio de traducciones que accede a base de datos
/// </summary>
public interface ITraduccionDbRepository
{
    List<Traduccion> TraerPorIdioma(int idIdioma);
    List<Traduccion> CompletarLista(SqlDataReader dr, List<Traduccion> lista);
}
