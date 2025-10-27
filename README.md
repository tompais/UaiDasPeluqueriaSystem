### Campos del Usuario

| Campo | Tipo | Validación |
|-------|------|------------|
| **ID** | int | Autogenerado, único, no nulo |
| **Nombre** | string | Obligatorio, máx. 50 caracteres |
| **Apellido** | string | Obligatorio, máx. 80 caracteres |
| **Email** | string | Formato válido, único, máx. 180 caracteres |
| **Clave** | string | **Exactamente 11 caracteres**, hash SHA256 |
| **Estado** | enum | Activo (0) o Baja (1) - **Valor por defecto: Activo** |
| **Rol** | enum | Cliente, Empleado, Supervisor, Administrador |
| **FechaCreacion** | DateTime | **Automático (DateTime.Now por defecto)** |

---

## ?? Seguridad

- **Hash de claves**: SHA256 (hash unidireccional)
- **Sin texto plano**: Las claves nunca se almacenan sin hashear
- **Thread-safety**: Operaciones en memoria protegidas con locks
- **Validación de entrada**: En UI y en lógica de negocio

?? **Nota de producción:** Para sistemas reales, se recomienda usar `BCrypt` o `Argon2` con salt automático en lugar de SHA256.

---

## ?? Tecnologías y Paquetes

### Stack Principal

- **.NET 8.0** - Framework
- **C# 12** - Lenguaje
- **Windows Forms** - UI
- **Microsoft.Extensions.DependencyInjection** - Contenedor DI

### Características de C# Utilizadas

- ? Nullable Reference Types (NRT)
- ? Primary Constructors (.NET 8)
- ? Collection expressions (.NET 8)
- ? Pattern matching
- ? Target-typed new expressions
- ? Init-only properties

### Decisiones de Diseño

- ? **Operaciones síncronas**: Datos en memoria, sin necesidad de async/await
- ? **Code simplicity**: Sin overhead de Task para operaciones instantáneas
- ? **YAGNI aplicado**: Async solo cuando hay I/O real (BD, archivos, red)
