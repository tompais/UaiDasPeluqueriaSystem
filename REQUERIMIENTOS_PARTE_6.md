# 🧩 Parte 6 - Usuario, Familia y Patente (Patrón Composite)

## 🎯 Objetivo

Implementar las clases necesarias para gestionar **Usuarios, Familias y Patentes** aplicando el **patrón Composite**, de modo que el sistema pueda determinar el escenario de trabajo según el usuario que ingresa.  

---

## ⚙️ Requisitos Generales

- Lenguaje: C#
- Framework: .NET 8.0
- UI: Windows Forms
- Arquitectura: N Capas
- Base de datos: SQL Server (`PeluSystem`)
- Principios:  
  - Clean Code  
  - Clean Architecture  
  - SOLID  
  - DRY  
  - YAGNI  
  - KISS  
  - Dependency Injection  
  - Expression Body (cuando sea posible)  
  - Constructor primario sin campos  
  - Funciones con único `return`  
  - Tipos implícitos (`var`) cuando sea conveniente  
  - Uso de **LINQ** para consultas y transformaciones de colecciones  

---

## 🧱 Capas involucradas y clases requeridas

| Capa     | Clase requerida   | Descripción |
|----------|------------------|-------------|
| `DOM`    | `Usuario`        | Entidad que representa al usuario del sistema |
| `DOM`    | `Patente`        | Clase que representa una opción/permiso individual |
| `DOM`    | `Familia`        | Clase que agrupa patentes y/o familias (Composite) |
| `REPO`   | `RepoUsuario`    | Métodos CRUD para usuarios |
| `REPO`   | `RepoFamilia`    | Métodos CRUD para familias |
| `REPO`   | `RepoPatente`    | Métodos CRUD para patentes |
| `APP`    | `AppUsuario`     | Wrapper que llama a `RepoUsuario` |
| `APP`    | `AppFamilia`     | Wrapper que llama a `RepoFamilia` |
| `APP`    | `AppPatente`     | Wrapper que llama a `RepoPatente` |
| `CONTEXT`| `DalSQLServer`   | Clase para manejar conexión y ejecución de comandos SQL |
| `UI`     | Formularios       | Formularios para gestionar usuarios, familias y patentes |

---

## 🧩 Patrón Composite

### Modelo de clases

- **Component** → Clase abstracta `Elemento` con operación común (`Ejecutar`, `Mostrar`, etc.)
- **Leaf** → Clase `Patente` (representa permisos individuales)
- **Composite** → Clase `Familia` (puede contener patentes y otras familias)

### Ejemplo de implementación sugerida

```csharp
namespace DOM
{
    public abstract class Elemento
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public abstract void Mostrar();
    }

    public class Patente : Elemento
    {
        public override void Mostrar() => Console.WriteLine($"Patente: {Nombre}");
    }

    public class Familia : Elemento
    {
        private readonly List<Elemento> _elementos = new();

        public void Agregar(Elemento elemento) => _elementos.Add(elemento);

        public override void Mostrar()
        {
            Console.WriteLine($"Familia: {Nombre}");
            foreach (var e in _elementos)
                e.Mostrar();
        }
    }
}
```

> ⚠️ Usar var en las instanciaciones y LINQ para recorrer/filtrar colecciones cuando sea conveniente.

---

## 🧩 Base de Datos

### Tablas requeridas

```sql
-- Tabla Opciones (Patentes)
CREATE TABLE [dbo].[Opciones] (
    [ID] INT NOT NULL IDENTITY(1,1),
    [Nombre] VARCHAR(100) NOT NULL,
    CONSTRAINT [PK_Opciones] PRIMARY KEY ([ID])
);

-- Tabla Familia (relación muchos a muchos entre elementos)
CREATE TABLE [dbo].[Familia] (
    [ID] INT NOT NULL IDENTITY(1,1),
    [Nombre] VARCHAR(100) NOT NULL,
    CONSTRAINT [PK_Familia] PRIMARY KEY ([ID])
);

-- Relación Familia-Elemento (Composite)
CREATE TABLE [dbo].[FamiliaElemento] (
    [IDFamilia] INT NOT NULL,
    [IDElemento] INT NOT NULL,
    CONSTRAINT [FK_FamiliaElemento_Familia] FOREIGN KEY ([IDFamilia]) REFERENCES [dbo].[Familia]([ID]),
    CONSTRAINT [FK_FamiliaElemento_Opciones] FOREIGN KEY ([IDElemento]) REFERENCES [dbo].[Opciones]([ID])
);
```

---

## ⚠️ Aclaración extra

> Antes de crear las clases Familia y Patente, el agente debe revisar si ya existe alguna implementación relacionada en el proyecto.
> •  Si existe, debe adaptarla para cumplir con los principios definidos (PascalCase, expression body, constructor primario sin campos, único return, LINQ, tipos implícitos).
> •  Si no existe, debe crear las clases desde cero en la capa DOM.

---

## 🧠 Pedido a GitHub Copilot

> •  Implementar las clases Usuario, Familia y Patente aplicando el patrón Composite.
> •  Implementar las tablas Opciones y Familia en la base de datos, con la relación muchos a muchos.
> •  Usar LINQ para recorrer y filtrar colecciones en las clases Composite.
> •  Usar tipos implícitos (var) en lo posible.
> •  Mantener coherencia con la arquitectura de N capas y las prácticas definidas en las partes anteriores.
> •  Revisar si ya existen implementaciones previas y adaptarlas.

---

## 🙋‍♂️ Pedido del Desarrollador

> "Quiero que esta etapa implemente el patrón Composite para gestionar usuarios, familias y patentes. Que el código sea limpio, desacoplado y funcional. Que se usen tipos implícitos y LINQ siempre que sea conveniente. Que se mantenga la coherencia entre la base de datos y el código, y que se adapten implementaciones previas si ya existen."
