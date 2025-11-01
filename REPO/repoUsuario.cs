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

        public DOM.domUsuario Crear(DOM.domUsuario usuario)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = @"INSERT INTO Usuario (Apellido, Nombre, Email, Rol, Estado, Clave, DV) 
                                       VALUES (@Apellido, @Nombre, @Email, @Rol, @Estado, @Clave, @DV);
                                       SELECT CAST(SCOPE_IDENTITY() as int);";
                    cmd.Connection = _dataAccess.AbrirConexion();
                    
                    cmd.Parameters.AddWithValue("@Apellido", usuario.Apellido);
                    cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    cmd.Parameters.AddWithValue("@Email", usuario.Email);
                    cmd.Parameters.AddWithValue("@Rol", (int)usuario.Rol);
                    cmd.Parameters.AddWithValue("@Estado", (int)usuario.Estado);
                    cmd.Parameters.AddWithValue("@Clave", usuario.Clave);
                    cmd.Parameters.AddWithValue("@DV", usuario.DV ?? "");
                    
                    var nuevoId = (int)cmd.ExecuteScalar();
                    
                    return new DOM.domUsuario
                    {
                        ID = nuevoId,
                        Apellido = usuario.Apellido,
                        Nombre = usuario.Nombre,
                        Email = usuario.Email,
                        Rol = usuario.Rol,
                        Estado = usuario.Estado,
                        Clave = usuario.Clave,
                        DV = usuario.DV,
                        Fecha_Agregar = DateTime.Now
                    };
                }
            }
            finally
            {
                _dataAccess.CerrarConexion();
            }
        }

        public bool ExisteEmail(string email)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "SELECT COUNT(*) FROM Usuario WHERE Email = @Email";
                    cmd.Connection = _dataAccess.AbrirConexion();
                    cmd.Parameters.AddWithValue("@Email", email);
                    
                    var count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
            finally
            {
                _dataAccess.CerrarConexion();
            }
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
                    Rol = (DOM.domUsuario.RolUsuario)Convert.ToInt32(dr["Rol"]),
                    Estado = (DOM.domUsuario.EstadoUsuario)Convert.ToInt32(dr["Estado"]),
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
