using System.Data.SqlClient;

namespace REPO
{
    public static class repoUsuario
    {
        public static List<DOM.domUsuario> Traer()
        {
            List<DOM.domUsuario> lista = new List<DOM.domUsuario>();
            CONTEXT.dalSQLServer sql = new CONTEXT.dalSQLServer();
            
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "SELECT * FROM Usuario";
                cmd.Connection = sql.AbrirConexion();
                
                using (SqlDataReader dr = sql.EjecutarSQL(cmd))
                {
                    lista = CompletarLista(dr, lista);
                }
            }
            
            sql.CerrarConexion();
            return lista;
        }

        private static List<DOM.domUsuario> CompletarLista(SqlDataReader dr, List<DOM.domUsuario> lista)
        {
            while (dr.Read())
            {
                DOM.domUsuario u = new DOM.domUsuario
                {
                    ID = int.Parse(dr["ID"].ToString()),
                    Apellido = dr["Apellido"].ToString(),
                    Nombre = dr["Nombre"].ToString(),
                    Email = dr["Email"].ToString(),
                    Rol = (DOM.Usuario.RolUsuario)int.Parse(dr["Rol"].ToString()),
                    Estado = (DOM.Usuario.EstadoUsuario)int.Parse(dr["Estado"].ToString()),
                    Clave = dr["Clave"].ToString(),
                    DV = dr["DV"].ToString(),
                    Fecha_Agregar = DateTime.Parse(dr["Fecha_Agregar"].ToString())
                };
                lista.Add(u);
            }
            return lista;
        }
    }
}
