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
    void AsignarPatente(int idFamilia, int idPatente);
    void AsignarFamiliaHija(int idFamiliaPadre, int idFamiliaHija);
    void RemoverPatente(int idFamilia, int idPatente);
    void RemoverFamiliaHija(int idFamiliaPadre, int idFamiliaHija);
    List<Patente> TraerPatentesDeFamilia(int idFamilia);
    List<Familia> TraerFamiliasHijas(int idFamilia);
    List<Familia> CompletarLista(SqlDataReader dr, List<Familia> lista);
}
