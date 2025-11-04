using ABS.Context;
using ABS.Repositories;
using Microsoft.Data.SqlClient;

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

        public DOM.DomUsuario? TraerPorId(int id)
        {
            try
            {
                using SqlCommand cmd = new();
                cmd.CommandText = "SELECT * FROM Usuario WHERE ID = @ID";
                cmd.Connection = dataAccess.AbrirConexion();
                cmd.Parameters.Add("@ID", System.Data.SqlDbType.Int).Value = id;

                using SqlDataReader dr = dataAccess.EjecutarSQL(cmd);
                var lista = CompletarLista(dr, []);
                return lista.FirstOrDefault();
            }
            finally
            {
                dataAccess.CerrarConexion();
            }
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

                cmd.Parameters.Add("@Apellido", System.Data.SqlDbType.VarChar, 80).Value = usuario.Apellido;
                cmd.Parameters.Add("@Nombre", System.Data.SqlDbType.VarChar, 50).Value = usuario.Nombre;
                cmd.Parameters.Add("@Email", System.Data.SqlDbType.VarChar, 180).Value = usuario.Email;
                cmd.Parameters.Add("@Rol", System.Data.SqlDbType.Int).Value = (int)usuario.Rol;
                cmd.Parameters.Add("@Estado", System.Data.SqlDbType.Int).Value = (int)usuario.Estado;
                cmd.Parameters.Add("@Clave", System.Data.SqlDbType.VarChar, 64).Value = usuario.Clave;
                cmd.Parameters.Add("@DV", System.Data.SqlDbType.VarChar, 50).Value = usuario.DV ?? "";

                var nuevoId = (int)cmd.ExecuteScalar()!;

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

        public void Modificar(DOM.DomUsuario usuario)
        {
            try
            {
                using SqlCommand cmd = new();
                cmd.CommandText = @"UPDATE Usuario 
                                     SET Apellido = @Apellido,
                                         Nombre = @Nombre,
                                         Email = @Email,
                                         Rol = @Rol,
                                         Estado = @Estado,
                                         Clave = @Clave,
                                         DV = @DV,
                                     FechaModificacion = GETDATE()
                                     WHERE ID = @ID";
                cmd.Connection = dataAccess.AbrirConexion();

                cmd.Parameters.Add("@ID", System.Data.SqlDbType.Int).Value = usuario.ID;
                cmd.Parameters.Add("@Apellido", System.Data.SqlDbType.VarChar, 80).Value = usuario.Apellido;
                cmd.Parameters.Add("@Nombre", System.Data.SqlDbType.VarChar, 50).Value = usuario.Nombre;
                cmd.Parameters.Add("@Email", System.Data.SqlDbType.VarChar, 180).Value = usuario.Email;
                cmd.Parameters.Add("@Rol", System.Data.SqlDbType.Int).Value = (int)usuario.Rol;
                cmd.Parameters.Add("@Estado", System.Data.SqlDbType.Int).Value = (int)usuario.Estado;
                cmd.Parameters.Add("@Clave", System.Data.SqlDbType.VarChar, 64).Value = usuario.Clave;
                cmd.Parameters.Add("@DV", System.Data.SqlDbType.VarChar, 50).Value = usuario.DV ?? "";

                cmd.ExecuteNonQuery();
            }
            finally
            {
                dataAccess.CerrarConexion();
            }
        }

        public void Eliminar(int id)
        {
            try
            {
                using SqlCommand cmd = new();
                cmd.CommandText = "DELETE FROM Usuario WHERE ID = @ID";
                cmd.Connection = dataAccess.AbrirConexion();
                cmd.Parameters.Add("@ID", System.Data.SqlDbType.Int).Value = id;

                cmd.ExecuteNonQuery();
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
                cmd.Parameters.Add("@Email", System.Data.SqlDbType.VarChar, 180).Value = email;

                var count = (int)cmd.ExecuteScalar()!;
                return count > 0;
            }
            finally
            {
                dataAccess.CerrarConexion();
            }
        }

        public bool ExisteEmailExcluyendoId(string email, int idExcluir)
        {
            try
            {
                using SqlCommand cmd = new();
                cmd.CommandText = "SELECT COUNT(*) FROM Usuario WHERE Email = @Email AND ID != @ID";
                cmd.Connection = dataAccess.AbrirConexion();
                cmd.Parameters.Add("@Email", System.Data.SqlDbType.VarChar, 180).Value = email;
                cmd.Parameters.Add("@ID", System.Data.SqlDbType.Int).Value = idExcluir;

                var count = (int)cmd.ExecuteScalar()!;
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
                    Fecha_Agregar = Convert.ToDateTime(dr["Fecha_Agregar"]),
                    FechaModificacion = dr["FechaModificacion"] != DBNull.Value
                        ? Convert.ToDateTime(dr["FechaModificacion"])
                        : null
                };
                lista.Add(u);
            }
            return lista;
        }
    }
}
