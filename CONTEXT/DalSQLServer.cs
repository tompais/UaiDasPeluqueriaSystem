using ABS.Context;
using System.Data.SqlClient;

namespace CONTEXT
{
    public class DalSQLServer : IDataAccess
    {
        private readonly SqlConnection con;

        public DalSQLServer()
        {
            con = new SqlConnection();
        }

        private static string StringConexion() =>
            // Permite configurar la cadena de conexi�n mediante variable de entorno
            // Si no est� definida, usa el valor por defecto
            Environment.GetEnvironmentVariable("PELUQUERIA_CONNECTIONSTRING")
            ?? "Data Source=DESKTOP-02DP0JO;Initial Catalog=PeluSystem;Integrated Security=True";

        public SqlConnection AbrirConexion()
        {
            con.ConnectionString = StringConexion();
            con.Open();
            return con;
        }

        public void CerrarConexion() => con.Close();

        public SqlDataReader EjecutarSQL(SqlCommand cmd) => cmd.ExecuteReader();
    }
}
