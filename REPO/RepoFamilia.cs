using ABS.Context;
using ABS.Repositories;
using Microsoft.Data.SqlClient;

namespace REPO
{
    public class RepoFamilia(IDataAccess dataAccess) : IFamiliaDbRepository
    {
        public List<DOM.Familia> Traer()
        {
            List<DOM.Familia> lista = [];

            try
            {
                using SqlCommand cmd = new();
                cmd.CommandText = "SELECT * FROM Familia";
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

        public DOM.Familia? TraerPorId(int id)
        {
            try
            {
                using SqlCommand cmd = new();
                cmd.CommandText = "SELECT * FROM Familia WHERE ID = @ID";
                cmd.Connection = dataAccess.AbrirConexion();
                cmd.Parameters.Add("@ID", System.Data.SqlDbType.Int).Value = id;

                using SqlDataReader dr = dataAccess.EjecutarSQL(cmd);
                var lista = CompletarLista(dr, []);
                var familia = lista.FirstOrDefault();

                if (familia != null)
                {
                    // Cargar patentes y familias hijas
                    var patentes = TraerPatentesDeFamilia(id);
                    patentes.ForEach(familia.AgregarPatente);

                    var familiasHijas = TraerFamiliasHijas(id);
                    familiasHijas.ForEach(familia.AgregarFamilia);
                }

                return familia;
            }
            finally
            {
                dataAccess.CerrarConexion();
            }
        }

        public DOM.Familia Crear(DOM.Familia familia)
        {
            try
            {
                using SqlCommand cmd = new();
                cmd.CommandText = @"INSERT INTO Familia (Nombre) 
                                   VALUES (@Nombre);
                                   SELECT CAST(SCOPE_IDENTITY() as int);";
                cmd.Connection = dataAccess.AbrirConexion();

                cmd.Parameters.Add("@Nombre", System.Data.SqlDbType.VarChar, 100).Value = familia.Nombre;

                var nuevoId = (int)cmd.ExecuteScalar()!;

                return new DOM.Familia
                {
                    ID = nuevoId,
                    Nombre = familia.Nombre
                };
            }
            finally
            {
                dataAccess.CerrarConexion();
            }
        }

        public void Modificar(DOM.Familia familia)
        {
            try
            {
                using SqlCommand cmd = new();
                cmd.CommandText = @"UPDATE Familia 
                                   SET Nombre = @Nombre
                                   WHERE ID = @ID";
                cmd.Connection = dataAccess.AbrirConexion();

                cmd.Parameters.Add("@ID", System.Data.SqlDbType.Int).Value = familia.ID;
                cmd.Parameters.Add("@Nombre", System.Data.SqlDbType.VarChar, 100).Value = familia.Nombre;

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
                // Las relaciones se eliminan por CASCADE en la BD
                cmd.CommandText = "DELETE FROM Familia WHERE ID = @ID";
                cmd.Connection = dataAccess.AbrirConexion();
                cmd.Parameters.Add("@ID", System.Data.SqlDbType.Int).Value = id;
                cmd.ExecuteNonQuery();
            }
            finally
            {
                dataAccess.CerrarConexion();
            }
        }

        public void AsignarPatente(int idFamilia, int idPatente)
        {
            try
            {
                using SqlCommand cmd = new();
                cmd.CommandText = @"INSERT INTO FamiliaPatente (IDFamilia, IDPatente) 
                                   VALUES (@IDFamilia, @IDPatente)";
                cmd.Connection = dataAccess.AbrirConexion();

                cmd.Parameters.Add("@IDFamilia", System.Data.SqlDbType.Int).Value = idFamilia;
                cmd.Parameters.Add("@IDPatente", System.Data.SqlDbType.Int).Value = idPatente;

                cmd.ExecuteNonQuery();
            }
            finally
            {
                dataAccess.CerrarConexion();
            }
        }

        public void AsignarFamiliaHija(int idFamiliaPadre, int idFamiliaHija)
        {
            try
            {
                using SqlCommand cmd = new();
                cmd.CommandText = @"INSERT INTO FamiliaFamilia (IDFamiliaPadre, IDFamiliaHija) 
                                   VALUES (@IDFamiliaPadre, @IDFamiliaHija)";
                cmd.Connection = dataAccess.AbrirConexion();

                cmd.Parameters.Add("@IDFamiliaPadre", System.Data.SqlDbType.Int).Value = idFamiliaPadre;
                cmd.Parameters.Add("@IDFamiliaHija", System.Data.SqlDbType.Int).Value = idFamiliaHija;

                cmd.ExecuteNonQuery();
            }
            finally
            {
                dataAccess.CerrarConexion();
            }
        }

        public void RemoverPatente(int idFamilia, int idPatente)
        {
            try
            {
                using SqlCommand cmd = new();
                cmd.CommandText = @"DELETE FROM FamiliaPatente 
                                   WHERE IDFamilia = @IDFamilia AND IDPatente = @IDPatente";
                cmd.Connection = dataAccess.AbrirConexion();

                cmd.Parameters.Add("@IDFamilia", System.Data.SqlDbType.Int).Value = idFamilia;
                cmd.Parameters.Add("@IDPatente", System.Data.SqlDbType.Int).Value = idPatente;

                cmd.ExecuteNonQuery();
            }
            finally
            {
                dataAccess.CerrarConexion();
            }
        }

        public void RemoverFamiliaHija(int idFamiliaPadre, int idFamiliaHija)
        {
            try
            {
                using SqlCommand cmd = new();
                cmd.CommandText = @"DELETE FROM FamiliaFamilia 
                                   WHERE IDFamiliaPadre = @IDFamiliaPadre AND IDFamiliaHija = @IDFamiliaHija";
                cmd.Connection = dataAccess.AbrirConexion();

                cmd.Parameters.Add("@IDFamiliaPadre", System.Data.SqlDbType.Int).Value = idFamiliaPadre;
                cmd.Parameters.Add("@IDFamiliaHija", System.Data.SqlDbType.Int).Value = idFamiliaHija;

                cmd.ExecuteNonQuery();
            }
            finally
            {
                dataAccess.CerrarConexion();
            }
        }

        public List<DOM.Patente> TraerPatentesDeFamilia(int idFamilia)
        {
            List<DOM.Patente> patentes = [];

            try
            {
                using SqlCommand cmd = new();
                cmd.CommandText = @"
                    SELECT O.ID, O.Nombre
                    FROM FamiliaPatente FP
                    INNER JOIN Opciones O ON FP.IDPatente = O.ID
                    WHERE FP.IDFamilia = @IDFamilia";
                cmd.Connection = dataAccess.AbrirConexion();
                cmd.Parameters.Add("@IDFamilia", System.Data.SqlDbType.Int).Value = idFamilia;

                using SqlDataReader dr = dataAccess.EjecutarSQL(cmd);
                
                while (dr.Read())
                {
                    var patente = new DOM.Patente
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        Nombre = dr["Nombre"]?.ToString() ?? string.Empty
                    };
                    patentes.Add(patente);
                }
            }
            finally
            {
                dataAccess.CerrarConexion();
            }

            return patentes;
        }

        public List<DOM.Familia> TraerFamiliasHijas(int idFamilia)
        {
            List<DOM.Familia> familias = [];

            try
            {
                using SqlCommand cmd = new();
                cmd.CommandText = @"
                    SELECT F.ID, F.Nombre
                    FROM FamiliaFamilia FF
                    INNER JOIN Familia F ON FF.IDFamiliaHija = F.ID
                    WHERE FF.IDFamiliaPadre = @IDFamilia";
                cmd.Connection = dataAccess.AbrirConexion();
                cmd.Parameters.Add("@IDFamilia", System.Data.SqlDbType.Int).Value = idFamilia;

                using SqlDataReader dr = dataAccess.EjecutarSQL(cmd);
                
                while (dr.Read())
                {
                    var familiaHija = new DOM.Familia
                    {
                        ID = Convert.ToInt32(dr["ID"]),
                        Nombre = dr["Nombre"]?.ToString() ?? string.Empty
                    };
                    
                    // Cargar recursivamente
                    var patentes = TraerPatentesDeFamilia(familiaHija.ID);
                    patentes.ForEach(familiaHija.AgregarPatente);

                    var subFamilias = TraerFamiliasHijas(familiaHija.ID);
                    subFamilias.ForEach(familiaHija.AgregarFamilia);

                    familias.Add(familiaHija);
                }
            }
            finally
            {
                dataAccess.CerrarConexion();
            }

            return familias;
        }

        public List<DOM.Familia> CompletarLista(SqlDataReader dr, List<DOM.Familia> lista)
        {
            while (dr.Read())
            {
                var familia = new DOM.Familia
                {
                    ID = Convert.ToInt32(dr["ID"]),
                    Nombre = dr["Nombre"]?.ToString() ?? string.Empty
                };
                lista.Add(familia);
            }
            return lista;
        }
    }
}
