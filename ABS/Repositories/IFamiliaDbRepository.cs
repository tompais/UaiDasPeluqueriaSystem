using DOM;
using Microsoft.Data.SqlClient;

namespace ABS.Repositories;

/// <summary>
/// Interfaz para el repositorio de familias que accede a base de datos
/// </summary>
public interface IFamiliaDbRepository
{
    List<Familia> Traer();
    Familia? TraerPorId(int id);
    Familia Crear(Familia familia);
    void Modificar(Familia familia);
    void Eliminar(int id);
    void AsignarElemento(int idFamilia, int idElemento);
    void RemoverElemento(int idFamilia, int idElemento);
    List<Elemento> TraerElementosDeFamilia(int idFamilia);
    List<Familia> CompletarLista(SqlDataReader dr, List<Familia> lista);
}
