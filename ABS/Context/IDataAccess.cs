using Microsoft.Data.SqlClient;

namespace ABS.Context;

/// <summary>
/// Interfaz para el acceso a datos SQL Server
/// </summary>
public interface IDataAccess
{
    SqlConnection AbrirConexion();
    void CerrarConexion();
    SqlDataReader EjecutarSQL(SqlCommand cmd);
}
