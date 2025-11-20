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
