using System.Data.SqlClient;
using ABS.Context;
using ABS.Repositories;

namespace REPO
{
    public class repoUsuario : IUsuarioDbRepository
    {
        private readonly IDataAccess _dataAccess;

        public repoUsuario(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public List<DOM.domUsuario> Traer()
        {
            List<DOM.domUsuario> lista = new List<DOM.domUsuario>();
            
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "SELECT * FROM Usuario";
                    cmd.Connection = _dataAccess.AbrirConexion();
                    
                    using (SqlDataReader dr = _dataAccess.EjecutarSQL(cmd))
                    {
                        lista = CompletarLista(dr, lista);
                    }
                }
            }
            finally
            {
                _dataAccess.CerrarConexion();
            }
            
            return lista;
        }

        private List<DOM.domUsuario> CompletarLista(SqlDataReader dr, List<DOM.domUsuario> lista)
        {
            while (dr.Read())
            {
                DOM.domUsuario u = new DOM.domUsuario
                {
                    ID = Convert.ToInt32(dr["ID"]),
                    Apellido = dr["Apellido"].ToString(),
                    Nombre = dr["Nombre"].ToString(),
                    Email = dr["Email"].ToString(),
                    Rol = (DOM.Usuario.RolUsuario)Convert.ToInt32(dr["Rol"]),
                    Estado = (DOM.Usuario.EstadoUsuario)Convert.ToInt32(dr["Estado"]),
                    Clave = dr["Clave"].ToString(),
                    DV = dr["DV"].ToString(),
                    Fecha_Agregar = Convert.ToDateTime(dr["Fecha_Agregar"])
                };
                lista.Add(u);
            }
            return lista;
        }
    }
}
