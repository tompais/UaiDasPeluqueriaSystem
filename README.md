### Campos del Usuario

| Campo | Tipo | Validaci�n |
|-------|------|------------|
| **ID** | int | Autogenerado, �nico, no nulo |
| **Nombre** | string | Obligatorio, m�x. 50 caracteres |
| **Apellido** | string | Obligatorio, m�x. 80 caracteres |
| **Email** | string | Formato v�lido, �nico, m�x. 180 caracteres |
| **Clave** | string | **Exactamente 11 caracteres**, hash MD5 |
| **Estado** | enum | Activo (0) o Baja (1) - **Valor por defecto: Activo** |
| **Rol** | enum | Cliente, Empleado, Supervisor, Administrador |
| **FechaCreacion** | DateTime | **Autom�tico (DateTime.Now por defecto)** |

---

## 🧩 Sistema de Permisos (Patrón Composite)

El sistema implementa un modelo de permisos flexible basado en el **patrón Composite**:

### Entidades

- **Patente**: Permiso individual (ej: Usuario_Alta, Cliente_Consultar)
- **Familia**: Agrupación de patentes y/o familias (ej: Familia_Usuarios, Rol_Administrador)
- **FamiliaElemento**: Relación muchos-a-muchos que permite la estructura composite

### Características

- Jerarquía recursiva de permisos
- Uso de LINQ para navegación de la estructura
- Aplicación de principios SOLID y Clean Code
- Base de datos con soporte para relaciones composite

Ver `DEVELOPMENT.md` para documentación detallada del patrón.

---

## 🌐 Soporte Multi-Idioma

El sistema incluye infraestructura completa para traducciones en múltiples idiomas:

### Componentes

- **Diccionario**: Tabla en base de datos que almacena traducciones (palabra original → palabra traducida)
- **Traduccion**: Entidad del dominio que representa una palabra traducida
- **RepoTraduccion**: Repositorio para acceso a traducciones en BD
- **AppTraduccion**: Capa de aplicación para gestión de traducciones
- **Traductor**: Servicio utilitario para traducir palabras usando LINQ

### Características

- Traducciones almacenadas en SQL Server
- Búsqueda case-insensitive de palabras
- Fallback a palabra original si no existe traducción
- Soporte para múltiples idiomas (Español, Inglés, Portugués incluidos)
- Arquitectura extensible para agregar más idiomas

### Uso

```csharp
// Traducir una palabra al inglés (IDIdioma = 2)
string traduccion = AppTraduccion.Traducir("Usuario", 2);
// Resultado: "User"

// Usar el servicio Traductor directamente
var traducciones = AppTraduccion.TraerPorIdioma(2);
string traduccion = Traductor.Traducir("Guardar", traducciones);
// Resultado: "Save"
```

---

## ?? Seguridad

- **Hash de claves**: MD5 (hash unidireccional)
- **Sin texto plano**: Las claves nunca se almacenan sin hashear
- **Thread-safety**: Operaciones en memoria protegidas con locks
- **Validaci�n de entrada**: En UI y en l�gica de negocio

?? **Nota de producci�n:** Para sistemas reales, se recomienda usar `BCrypt` o `Argon2` con salt autom�tico en lugar de MD5.

---

## ?? Tecnolog�as y Paquetes

### Stack Principal

- **.NET 8.0** - Framework
- **C# 12** - Lenguaje
- **Windows Forms** - UI
- **Microsoft.Extensions.DependencyInjection** - Contenedor DI

### Caracter�sticas de C# Utilizadas

- ? Nullable Reference Types (NRT)
- ? Primary Constructors (.NET 8)
- ? Collection expressions (.NET 8)
- ? Pattern matching
- ? Target-typed new expressions
- ? Init-only properties

### Decisiones de Dise�o

- ? **Operaciones s�ncronas**: Datos en memoria, sin necesidad de async/await
- ? **Code simplicity**: Sin overhead de Task para operaciones instant�neas
- ? **YAGNI aplicado**: Async solo cuando hay I/O real (BD, archivos, red)
