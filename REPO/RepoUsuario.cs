using ABS.Context;
using ABS.Repositories;
using System.Data.SqlClient;

namespace REPO
{
    public class RepoUsuario(IDataAccess dataAccess) : IUsuarioDbRepository
    {
        public List<DOM.DomUsuario> Traer()
        {
            List<DOM.DomUsuario> lista = [];

            try
            {
                using SqlCommand cmd = new();
                cmd.CommandText = "SELECT * FROM Usuario";
                cmd.Connection = dataAccess.AbrirConexion();

                using SqlDataReader dr = dataAccess.EjecutarSQL(cmd);
                lista = CompletarLista(dr, lista);
            }
            finally
            {
                dataAccess.CerrarConexion();
            }

            return lista;
        }

        public DOM.DomUsuario Crear(DOM.DomUsuario usuario)
        {
            try
            {
                using SqlCommand cmd = new();
                cmd.CommandText = @"INSERT INTO Usuario (Apellido, Nombre, Email, Rol, Estado, Clave, DV) 
                                       VALUES (@Apellido, @Nombre, @Email, @Rol, @Estado, @Clave, @DV);
                                       SELECT CAST(SCOPE_IDENTITY() as int);";
                cmd.Connection = dataAccess.AbrirConexion();

                cmd.Parameters.AddWithValue("@Apellido", usuario.Apellido);
                cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                cmd.Parameters.AddWithValue("@Email", usuario.Email);
                cmd.Parameters.AddWithValue("@Rol", (int)usuario.Rol);
                cmd.Parameters.AddWithValue("@Estado", (int)usuario.Estado);
                cmd.Parameters.AddWithValue("@Clave", usuario.Clave);
                cmd.Parameters.AddWithValue("@DV", usuario.DV ?? "");

                var nuevoId = (int)cmd.ExecuteScalar();

                return new DOM.DomUsuario
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
            finally
            {
                dataAccess.CerrarConexion();
            }
        }

        public bool ExisteEmail(string email)
        {
            try
            {
                using SqlCommand cmd = new();
                cmd.CommandText = "SELECT COUNT(*) FROM Usuario WHERE Email = @Email";
                cmd.Connection = dataAccess.AbrirConexion();
                cmd.Parameters.AddWithValue("@Email", email);

                var count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
            finally
            {
                dataAccess.CerrarConexion();
            }
        }

        public List<DOM.DomUsuario> CompletarLista(SqlDataReader dr, List<DOM.DomUsuario> lista)
        {
            while (dr.Read())
            {
                DOM.DomUsuario u = new()
                {
                    ID = Convert.ToInt32(dr["ID"]),
                    Apellido = dr["Apellido"]?.ToString() ?? string.Empty,
                    Nombre = dr["Nombre"]?.ToString() ?? string.Empty,
                    Email = dr["Email"]?.ToString() ?? string.Empty,
                    Rol = (DOM.DomUsuario.RolUsuario)Convert.ToInt32(dr["Rol"]),
                    Estado = (DOM.DomUsuario.EstadoUsuario)Convert.ToInt32(dr["Estado"]),
                    Clave = dr["Clave"]?.ToString() ?? string.Empty,
                    DV = dr["DV"]?.ToString() ?? string.Empty,
                    Fecha_Agregar = Convert.ToDateTime(dr["Fecha_Agregar"])
                };
                lista.Add(u);
            }
            return lista;
        }
    }
}
