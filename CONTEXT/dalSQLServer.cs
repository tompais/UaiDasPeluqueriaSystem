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
            return "Data Source=DESKTOP-02DP0JO;Initial Catalog=PeluqueriaSystem;Integrated Security=True";
        }

        public SqlConnection AbrirConexion()
        {
            con.ConnectionString = StringConexion();
            con.Open();
            return con;
        }

        public void CerarConexion()
        {
            con.Close();
        }

        public SqlDataReader EjecutarSQL(SqlCommand cmd)
        {
            return cmd.ExecuteReader();
        }
    }
}
