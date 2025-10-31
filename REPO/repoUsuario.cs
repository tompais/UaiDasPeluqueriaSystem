using System.Data.SqlClient;

namespace REPO
{
    public static class repoUsuario
    {
        public static List<DOM.domUsuario> Traer()
        {
            List<DOM.domUsuario> lista = new List<DOM.domUsuario>();
            CONTEXT.dalSQLServer sql = new CONTEXT.dalSQLServer();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM Usuario";
            cmd.Connection = sql.AbrirConexion();
            lista = CompletarLista(sql.EjecutarSQL(cmd), lista);
            sql.CerarConexion();
            return lista;
        }

        private static List<DOM.domUsuario> CompletarLista(SqlDataReader dr, List<DOM.domUsuario> lista)
        {
            while (dr.Read())
            {
                DOM.domUsuario u = new DOM.domUsuario();
                u.ID = int.Parse(dr["ID"].ToString());
                u.Apellido = dr["Apellido"].ToString();
                u.Nombre = dr["Nombre"].ToString();
                u.Email = dr["Email"].ToString();
                u.Rol = int.Parse(dr["Rol"].ToString());
                u.Estado = int.Parse(dr["Estado"].ToString());
                u.Clave = dr["Clave"].ToString();
                u.DV = dr["DV"].ToString();
                u.Fecha_Agregar = DateTime.Parse(dr["Fecha_Agregar"].ToString());
                lista.Add(u);
            }
            return lista;
        }
    }
}
