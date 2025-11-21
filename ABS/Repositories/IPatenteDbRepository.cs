using DOM;
using Microsoft.Data.SqlClient;

namespace ABS.Repositories;

/// <summary>
/// Interfaz para el repositorio de patentes que accede a base de datos
/// </summary>
public interface IPatenteDbRepository
{
    List<Patente> Traer();
    Patente? TraerPorId(int id);
    Patente Crear(Patente patente);
    void Modificar(Patente patente);
    void Eliminar(int id);
    List<Patente> CompletarLista(SqlDataReader dr, List<Patente> lista);
}
