using ABS.Context;
using Microsoft.Data.SqlClient;

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
            // Permite configurar la cadena de conexión mediante variable de entorno
            // Si no está definida, usa el valor por defecto
            Environment.GetEnvironmentVariable("PELUQUERIA_CONNECTIONSTRING")
            ?? "Data Source=192.168.1.40,1433;Initial Catalog=PeluSystem;User ID=sa;Password=yourpassword;TrustServerCertificate=True";

        public SqlConnection AbrirConexion()
        {
            if (con.State == System.Data.ConnectionState.Closed)
            {
                con.ConnectionString = StringConexion();
                con.Open();
            }
            return con;
        }

        public void CerrarConexion()
        {
            if (con.State != System.Data.ConnectionState.Closed)
            {
                con.Close();
            }
        }

        public SqlDataReader EjecutarSQL(SqlCommand cmd) => cmd.ExecuteReader();
    }
}
