using ABS.Context;
using ABS.Repositories;
using Microsoft.Data.SqlClient;

namespace REPO
{
    public class RepoTraduccion(IDataAccess dataAccess) : ITraduccionDbRepository
    {
        public List<DOM.Traduccion> TraerPorIdioma(int idIdioma)
        {
            List<DOM.Traduccion> lista = [];

            try
            {
                using SqlCommand cmd = new();
                cmd.CommandText = "SELECT * FROM Diccionario WHERE IDIdioma = @IDIdioma";
                cmd.Connection = dataAccess.AbrirConexion();
                cmd.Parameters.Add("@IDIdioma", System.Data.SqlDbType.Int).Value = idIdioma;

                using SqlDataReader dr = dataAccess.EjecutarSQL(cmd);
                lista = CompletarLista(dr, lista);
            }
            finally
            {
                dataAccess.CerrarConexion();
            }

            return lista;
        }

        public List<DOM.Traduccion> CompletarLista(SqlDataReader dr, List<DOM.Traduccion> lista)
        {
            while (dr.Read())
            {
                DOM.Traduccion t = new()
                {
                    ID = Convert.ToInt32(dr["ID"]),
                    IDIdioma = Convert.ToInt32(dr["IDIdioma"]),
                    PalabraOriginal = dr["PalabraOriginal"]?.ToString() ?? string.Empty,
                    PalabraIdioma = dr["PalabraIdioma"]?.ToString() ?? string.Empty
                };
                lista.Add(t);
            }
            return lista;
        }
    }
}
