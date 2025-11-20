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
                    // Cargar elementos de la familia
                    var elementos = TraerElementosDeFamilia(id);
                    elementos.ForEach(familia.Agregar);
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
                // Primero eliminar las relaciones en FamiliaElemento
                cmd.CommandText = "DELETE FROM FamiliaElemento WHERE IDFamilia = @ID";
                cmd.Connection = dataAccess.AbrirConexion();
                cmd.Parameters.Add("@ID", System.Data.SqlDbType.Int).Value = id;
                cmd.ExecuteNonQuery();

                // Luego eliminar la familia
                cmd.CommandText = "DELETE FROM Familia WHERE ID = @ID";
                cmd.ExecuteNonQuery();
            }
            finally
            {
                dataAccess.CerrarConexion();
            }
        }

        public void AsignarElemento(int idFamilia, int idElemento)
        {
            // Determinar el tipo de elemento consultando las tablas
            var tipoElemento = DeterminarTipoElemento(idElemento);
            
            try
            {
                using SqlCommand cmd = new();
                cmd.CommandText = @"INSERT INTO FamiliaElemento (IDFamilia, IDElemento, TipoElemento) 
                                   VALUES (@IDFamilia, @IDElemento, @TipoElemento)";
                cmd.Connection = dataAccess.AbrirConexion();

                cmd.Parameters.Add("@IDFamilia", System.Data.SqlDbType.Int).Value = idFamilia;
                cmd.Parameters.Add("@IDElemento", System.Data.SqlDbType.Int).Value = idElemento;
                cmd.Parameters.Add("@TipoElemento", System.Data.SqlDbType.Char, 1).Value = tipoElemento;

                cmd.ExecuteNonQuery();
            }
            finally
            {
                dataAccess.CerrarConexion();
            }
        }

        private string DeterminarTipoElemento(int idElemento)
        {
            try
            {
                using SqlCommand cmd = new();
                // Usar un solo query con UNION para verificar en ambas tablas
                cmd.CommandText = @"
                    SELECT 'P' AS Tipo FROM Opciones WHERE ID = @ID
                    UNION ALL
                    SELECT 'F' AS Tipo FROM Familia WHERE ID = @ID";
                cmd.Connection = dataAccess.AbrirConexion();
                cmd.Parameters.Add("@ID", System.Data.SqlDbType.Int).Value = idElemento;

                using SqlDataReader dr = dataAccess.EjecutarSQL(cmd);
                if (dr.Read())
                    return dr["Tipo"]?.ToString() ?? string.Empty;

                throw new InvalidOperationException($"Element with ID {idElemento} not found in Options or Family tables");
            }
            finally
            {
                dataAccess.CerrarConexion();
            }
        }

        public void RemoverElemento(int idFamilia, int idElemento)
        {
            try
            {
                using SqlCommand cmd = new();
                // Eliminar sin importar el tipo (TipoElemento es parte de la clave primaria pero no necesario para DELETE)
                cmd.CommandText = @"DELETE FROM FamiliaElemento 
                                   WHERE IDFamilia = @IDFamilia AND IDElemento = @IDElemento";
                cmd.Connection = dataAccess.AbrirConexion();

                cmd.Parameters.Add("@IDFamilia", System.Data.SqlDbType.Int).Value = idFamilia;
                cmd.Parameters.Add("@IDElemento", System.Data.SqlDbType.Int).Value = idElemento;

                cmd.ExecuteNonQuery();
            }
            finally
            {
                dataAccess.CerrarConexion();
            }
        }

        public List<DOM.Elemento> TraerElementosDeFamilia(int idFamilia)
        {
            List<DOM.Elemento> elementos = [];

            try
            {
                using SqlCommand cmd = new();
                // Usar TipoElemento para determinar si es Patente o Familia sin ambigüedad
                cmd.CommandText = @"
                    SELECT O.ID, O.Nombre, 'Patente' AS Tipo
                    FROM FamiliaElemento FE
                    INNER JOIN Opciones O ON FE.IDElemento = O.ID
                    WHERE FE.IDFamilia = @IDFamilia AND FE.TipoElemento = 'P'
                    UNION ALL
                    SELECT F.ID, F.Nombre, 'Familia' AS Tipo
                    FROM FamiliaElemento FE
                    INNER JOIN Familia F ON FE.IDElemento = F.ID
                    WHERE FE.IDFamilia = @IDFamilia AND FE.TipoElemento = 'F'";
                cmd.Connection = dataAccess.AbrirConexion();
                cmd.Parameters.Add("@IDFamilia", System.Data.SqlDbType.Int).Value = idFamilia;

                using SqlDataReader dr = dataAccess.EjecutarSQL(cmd);
                
                while (dr.Read())
                {
                    var tipo = dr["Tipo"]?.ToString();
                    DOM.Elemento elemento;

                    if (tipo == "Patente")
                    {
                        elemento = new DOM.Patente
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            Nombre = dr["Nombre"]?.ToString() ?? string.Empty
                        };
                    }
                    else
                    {
                        var familia = new DOM.Familia
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            Nombre = dr["Nombre"]?.ToString() ?? string.Empty
                        };
                        // Cargar recursivamente los elementos de esta familia hija
                        // Nota: Para jerarquías muy profundas, considerar usar CTE (Common Table Expression)
                        // para evitar N+1 queries. La implementación actual es adecuada para jerarquías
                        // pequeñas (2-3 niveles) que son típicas en sistemas de permisos.
                        var subElementos = TraerElementosDeFamilia(familia.ID);
                        subElementos.ForEach(familia.Agregar);
                        elemento = familia;
                    }

                    elementos.Add(elemento);
                }
            }
            finally
            {
                dataAccess.CerrarConexion();
            }

            return elementos;
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
