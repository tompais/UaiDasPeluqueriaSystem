# 🧩 Parte 7 - Múltiples Idiomas

## 🎯 Objetivo

Implementar soporte para múltiples idiomas en el sistema de peluquería.  
La solución debe permitir traducir palabras originales a un idioma solicitado, utilizando un diccionario almacenado en la base de datos.

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
| `DOM`    | `Traduccion`     | Entidad que representa una palabra traducida |
| `REPO`   | `RepoTraduccion` | Métodos para acceder al diccionario en la base de datos |
| `APP`    | `AppTraduccion`  | Wrapper que llama a `RepoTraduccion` |
| `SERV`   | `Traductor`      | Clase con método para convertir palabra original al idioma solicitado |
| `UI`     | Formularios       | Formularios que permitan seleccionar idioma y mostrar traducciones |

---

## 🧩 Base de Datos

### Tabla Diccionario

```sql
CREATE TABLE [dbo].[Diccionario] (
    [ID] INT NOT NULL IDENTITY(1,1),
    [IDIdioma] INT NOT NULL,
    [PalabraOriginal] VARCHAR(100) NOT NULL,
    [PalabraIdioma] VARCHAR(100) NOT NULL,
    CONSTRAINT [PK_Diccionario] PRIMARY KEY ([ID])
);
```

---

## 🧩 Clase Traductor en `SERV`

### Requerimientos

• Clase pública Traductor en la capa SERV.
• Método Traducir(string palabra, int idIdioma) que devuelva la traducción.
• Usar LINQ para buscar la traducción en la colección obtenida desde la base de datos.
• Mantener un único return.

### Ejemplo de implementación sugerida

```csharp
namespace SERV
{
    public class Traductor()
    {
        public static string Traducir(string palabra, int idIdioma)
        {
            var traducciones = AppTraduccion.TraerPorIdioma(idIdioma);
            var resultado = traducciones.FirstOrDefault(t => t.PalabraOriginal.Equals(palabra, StringComparison.OrdinalIgnoreCase));
            return resultado?.PalabraIdioma ?? palabra;
        }
    }
}
```

---

## ⚠️ Aclaración extra

> Antes de crear la clase Traductor, el agente debe revisar si ya existe alguna implementación de traducción en el proyecto.
>
> •  Si existe, debe adaptarla para cumplir con los principios definidos (PascalCase, expression body, constructor primario sin campos, único return, LINQ, tipos implícitos).
> •  Si no existe, debe crear la clase desde cero en la capa SERV.

---

## 🧠 Pedido a GitHub Copilot

> •  Implementar la clase Traductor en la capa SERV.
> •  Crear la tabla Diccionario en la base de datos.
> •  Implementar repositorios y wrappers (RepoTraduccion, AppTraduccion) siguiendo el patrón de las partes anteriores.
> •  Usar LINQ y tipos implícitos (var) siempre que sea conveniente.
> •  Revisar si ya existen implementaciones previas y adaptarlas.
> •  Mantener coherencia con la arquitectura de N capas y las prácticas definidas en las partes anteriores.

---

## 🙋‍♂️ Pedido del Desarrollador

> "Quiero que esta etapa implemente el soporte para múltiples idiomas en el sistema. Que el código sea limpio, desacoplado y funcional. Que se usen tipos implícitos y LINQ siempre que sea conveniente. Que se mantenga la coherencia entre la base de datos y el código, y que se adapten implementaciones previas si ya existen."
