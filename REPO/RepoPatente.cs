using ABS.Context;
using ABS.Repositories;
using Microsoft.Data.SqlClient;

namespace REPO
{
    public class RepoPatente(IDataAccess dataAccess) : IPatenteDbRepository
    {
        public List<DOM.Patente> Traer()
        {
            List<DOM.Patente> lista = [];

            try
            {
                using SqlCommand cmd = new();
                cmd.CommandText = "SELECT * FROM Opciones";
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

        public DOM.Patente? TraerPorId(int id)
        {
            try
            {
                using SqlCommand cmd = new();
                cmd.CommandText = "SELECT * FROM Opciones WHERE ID = @ID";
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

        public DOM.Patente Crear(DOM.Patente patente)
        {
            try
            {
                using SqlCommand cmd = new();
                cmd.CommandText = @"INSERT INTO Opciones (Nombre) 
                                   VALUES (@Nombre);
                                   SELECT CAST(SCOPE_IDENTITY() as int);";
                cmd.Connection = dataAccess.AbrirConexion();

                cmd.Parameters.Add("@Nombre", System.Data.SqlDbType.VarChar, 100).Value = patente.Nombre;

                var nuevoId = (int)cmd.ExecuteScalar()!;

                return new DOM.Patente
                {
                    ID = nuevoId,
                    Nombre = patente.Nombre
                };
            }
            finally
            {
                dataAccess.CerrarConexion();
            }
        }

        public void Modificar(DOM.Patente patente)
        {
            try
            {
                using SqlCommand cmd = new();
                cmd.CommandText = @"UPDATE Opciones 
                                   SET Nombre = @Nombre
                                   WHERE ID = @ID";
                cmd.Connection = dataAccess.AbrirConexion();

                cmd.Parameters.Add("@ID", System.Data.SqlDbType.Int).Value = patente.ID;
                cmd.Parameters.Add("@Nombre", System.Data.SqlDbType.VarChar, 100).Value = patente.Nombre;

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
                cmd.CommandText = "DELETE FROM Opciones WHERE ID = @ID";
                cmd.Connection = dataAccess.AbrirConexion();
                cmd.Parameters.Add("@ID", System.Data.SqlDbType.Int).Value = id;

                cmd.ExecuteNonQuery();
            }
            finally
            {
                dataAccess.CerrarConexion();
            }
        }

        public List<DOM.Patente> CompletarLista(SqlDataReader dr, List<DOM.Patente> lista)
        {
            while (dr.Read())
            {
                var patente = new DOM.Patente
                {
                    ID = Convert.ToInt32(dr["ID"]),
                    Nombre = dr["Nombre"]?.ToString() ?? string.Empty
                };
                lista.Add(patente);
            }
            return lista;
        }
    }
}
