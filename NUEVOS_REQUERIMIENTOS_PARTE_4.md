# 🧩 Parte 4 - Conexión entre Código y Base de Datos

## 🎯 Objetivo

Establecer la conexión entre el código desarrollado en C# y la base de datos SQL Server, asegurando que la estructura de la entidad `Usuario` en el código coincida con la tabla `Usuario` en la base de datos. Esta etapa debe mostrar los datos en pantalla utilizando Windows Forms, respetando la arquitectura de N capas.

---

## ⚙️ Requisitos Generales

- Lenguaje: C#
- Framework: .NET 8.0
- UI: Windows Forms
- Arquitectura: N Capas
- Base de datos: SQL Server (`PeluqueriaSystem`)
- Principios: Clean Code, Clean Architecture, SOLID, DRY, YAGNI, KISS, Dependency Injection

---

## 🧱 Capas involucradas y clases requeridas

| Capa     | Clase requerida        | Descripción |
|----------|------------------------|-------------|
| `DOM`    | `domUsuario`           | Entidad con propiedades del usuario |
| `REPO`   | `repoUsuario`          | Clase estática con método `Traer()` y función `CompletarLista()` |
| `APP`    | `appUsuario`           | Wrapper que llama a `repoUsuario.Traer()` |
| `CONTEXT`| `dalSQLServer`         | Clase para manejar conexión y ejecución de comandos SQL |
| `UI`     | `PeluqueriaSystem`     | Formulario MDI principal |
| `UI`     | `frmUsuarios`          | Formulario para mostrar listado de usuarios |
| `UI`     | `frmAltaUsuario`       | Formulario para alta de usuario (no se modifica en esta etapa) |

---

## 🧩 Código requerido por capa

### 1️⃣ Clase `domUsuario` en `DOM`

```csharp
namespace DOM
{
    public class domUsuario
    {
        public int ID { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public int Rol { get; set; }
        public int Estado { get; set; }
        public string Clave { get; set; }
        public string DV { get; set; }
        public DateTime Fecha_Agregar { get; set; }
    }
}
```

---

### 2️⃣ Clase repoUsuario en REPO
```csharp
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
```

---

### 3️⃣ Clase appUsuario en APP
```csharp
namespace APP
{
    public class appUsuario
    {
        public List<DOM.domUsuario> Traer()
        {
            return REPO.repoUsuario.Traer();
        }
    }
}
```

---

### 4️⃣ Clase dalSQLServer en CONTEXT
```csharp
namespace CONTEXT
{
    public class dalSQLServer
    {
        private SqlConnection con;

        public dalSQLServer()
        {
            con = new SqlConnection();
        }

        private string StringConexion()
        {
            return "Data Source=DESKTOP-02DP0JO;Initial Catalog=PeluqueriaSystem;Integrated Security=True";
        }

        public SqlConnection AbrirConexion()
        {
            con.ConnectionString = StringConexion();
            con.Open();
            return con;
        }

        public void CerarConexion()
        {
            con.Close();
        }

        public SqlDataReader EjecutarSQL(SqlCommand cmd)
        {
            return cmd.ExecuteReader();
        }
    }
}
```

---

## 🖥️ Formularios en UI

### Formulario PeluqueriaSystem
```csharp
private void usuariosToolStripMenuItem_Click(object sender, EventArgs e)
{
    frmUsuarios f = new frmUsuarios();
    f.MdiParent = this;
    f.Show();
    f.Left = (this.Left + this.Width - f.Width) / 2;
    f.Top = (this.Top + this.Height - f.Height) / 2;
}
```

---

### Formulario frmUsuarios
```csharp
private void frmUsuarios_Load(object sender, EventArgs e)
{
    APP.appUsuario usuario = new APP.appUsuario();
    DBUsuarios.DataSource = usuario.Traer();
    usuario = null;
}
```

---

## 🧠 Sincronización entre Código y Base de Datos
> ⚠️ A nivel código se realizaron modificaciones en la clase Usuario para simplificar la estructura, ya que por ahora solo se implementa la funcionalidad de alta de usuario.

### ❌ Propiedades eliminadas en el código:
- Fecha_Modificar
- Usuario_Modificar
- Usuario_Agregar

### ✅ Propiedades mantenidas:
- ID
- Apellido
- Nombre
- Email
- Rol
- Estado
- Clave
- DV
- Fecha_Agregar

---

## 🧠 Pedido a GitHub Copilot
- Ajustar los scripts de base de datos generados en la Parte 2 para que coincidan con la clase Usuario actual.
- Eliminar del script SQL las columnas que ya no existen en el código.
- Asegurar que los nombres, tipos de datos y restricciones coincidan entre la tabla y la clase.
- Mantener la columna Fecha_Agregar con valor por defecto GETDATE() para registrar la fecha de creación.
- Verificar que el campo ID siga siendo autoincremental (IDENTITY(1,1)).
- Aplicar buenas prácticas como KISS: mantener el diseño simple, claro y enfocado solo en lo necesario.
- Mantener la arquitectura de N capas y respetar la separación de responsabilidades.

---

## 🙋‍♂️ Pedido del Desarrollador
> "Quiero que esta etapa conecte correctamente el código con la base de datos, respetando la arquitectura y las buenas prácticas. Que se simplifique lo innecesario, se mantenga la coherencia entre capas, y que la tabla Usuario refleje fielmente la entidad Usuario que estamos usando en C#. No quiero campos que no se usan ni lógica innecesaria. Que todo sea claro, limpio y funcional."
