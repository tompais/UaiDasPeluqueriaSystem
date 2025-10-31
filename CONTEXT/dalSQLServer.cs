using System.Data.SqlClient;

namespace CONTEXT
{
    public class dalSQLServer
    {
        private SqlConnection con;

        public dalSQLServer()
        {
            con = new SqlConnection();
        }

        private string StringConexion()
        {
            // Permite configurar la cadena de conexi�n mediante variable de entorno
            // Si no est� definida, usa el valor por defecto
            return Environment.GetEnvironmentVariable("PELUQUERIA_CONNECTIONSTRING") 
                   ?? "Data Source=DESKTOP-02DP0JO;Initial Catalog=PeluSystem;Integrated Security=True";
        }

        public SqlConnection AbrirConexion()
        {
            con.ConnectionString = StringConexion();
            con.Open();
            return con;
        }

        public void CerrarConexion()
        {
            con.Close();
        }

        public SqlDataReader EjecutarSQL(SqlCommand cmd)
        {
            return cmd.ExecuteReader();
        }
    }
}
