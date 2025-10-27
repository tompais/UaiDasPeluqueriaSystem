# Sistema de Gestión para Peluquería

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-12.0-239120)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

Sistema de gestión de usuarios para peluquería desarrollado con **Windows Forms (.NET 8)** siguiendo principios de **Clean Architecture**, **SOLID**, y **Clean Code**.

---

## ?? Descripción del Proyecto

Este proyecto implementa la funcionalidad de **Alta de Usuario (CU8.1)** para un sistema de gestión de peluquería, demostrando la aplicación práctica de:

- ? **Clean Architecture** - Separación en capas con proyectos independientes
- ? **SOLID** - Todos los principios aplicados
- ? **Clean Code** - Código limpio, claro y mantenible
- ? **DRY** - Sin duplicación de lógica
- ? **YAGNI** - Solo lo necesario implementado
- ? **Dependency Injection** - Desacoplamiento mediante interfaces

?? **Nota:** Este proyecto **no usa base de datos**. La persistencia es completamente **en memoria** para fines educativos.

---

## ?? Inicio Rápido

### Prerrequisitos

- [Visual Studio 2022](https://visualstudio.microsoft.com/) o superior
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Windows 10/11

### Ejecutar el Proyecto

1. **Clonar o abrir la solución**
   ```bash
   # Si usas git
   git clone [url-del-repo]
   cd PeluqueriaSystem
   ```

2. **Abrir en Visual Studio**
   - Doble clic en `PeluqueriaSystem.sln`
   - O desde Visual Studio: `Archivo > Abrir > Proyecto/Solución`

3. **Compilar y ejecutar**
   - Presionar **F5** o clic en el botón "? Iniciar"
   - O usar el comando:
     ```bash
     dotnet run --project PeluqueriaSystem
     ```

4. **Usar la aplicación**
   - En el formulario principal, ir a: **Administración ? Usuarios**
   - Clic en **Nuevo Usuario**
   - Completar formulario (?? la clave debe tener **exactamente 11 caracteres**)
   - Guardar y verificar en la lista

---

## ?? Arquitectura

### Estructura de N Capas

```
PeluqueriaSystem.sln
?
??? DOM/      # ?? Entidades del dominio (Usuario, EstadoUsuario)
??? ABS/     # ?? Interfaces y abstracciones (IUsuarioRepository, IUsuarioService)
??? SERV/       # ?? Servicios auxiliares (EncriptacionService)
??? CONTEXT/       # ?? Contexto de datos en memoria (InMemoryContext)
??? REPO/     # ?? Repositorio CRUD (UsuarioRepository)
??? APP/           # ?? Lógica de negocio (UsuarioService)
??? PeluqueriaSystem/ # ??? Interfaz de usuario (Windows Forms)
    ??? frmPrincipal.cs      # MDI Principal
    ??? frmUsuarios.cs  # Listado
    ??? frmAltaUsuario.cs    # Alta de usuario
```

### Flujo de Dependencias

```
    UI (PeluqueriaSystem)
  ?
    APP (Lógica de Negocio)
  ?
    REPO (Repositorio) + SERV (Servicios)
      ?
    CONTEXT (Datos en Memoria)
           ?
    DOM (Entidades)
           ?
    ABS (Interfaces) ? Todas las capas dependen de abstracciones
```

**Principio clave:** Las dependencias apuntan hacia adentro (hacia el dominio).

---

## ? Características Implementadas

### Funcionalidad CU8.1 - Alta de Usuario

| Característica | Descripción |
|---------------|-------------|
| ? **Formulario MDI** | Contenedor principal con menú de navegación |
| ? **Listado de usuarios** | DataGridView con todos los usuarios registrados |
| ? **Alta de usuario** | Formulario modal con validaciones completas |
| ? **Validaciones robustas** | En UI y en lógica de negocio |
| ? **Encriptación SHA256** | Claves nunca en texto plano |
| ? **Email único** | No permite emails duplicados |
| ? **IDs autogenerados** | Asignación automática y única |
| ? **Thread-safe** | Operaciones concurrentes protegidas |

### Campos del Usuario

| Campo | Tipo | Validación |
|-------|------|------------|
| **ID** | int | Autogenerado, único, no nulo |
| **Nombre** | string | Obligatorio, máx. 50 caracteres |
| **Apellido** | string | Obligatorio, máx. 80 caracteres |
| **Email** | string | Formato válido, único, máx. 180 caracteres |
| **Clave** | string | **Exactamente 11 caracteres**, encriptada SHA256 |
| **Estado** | enum | Activo (0) o Baja (1) |
| **Rol** | int | Rango 0-9 |
| **FechaCreacion** | DateTime | Automático al crear |
| **UsuarioCreacion** | string | Registrado en el sistema |

---

## ?? Principios Aplicados

### SOLID

- **S**ingle Responsibility: Cada clase tiene una única responsabilidad
- **O**pen/Closed: Extensible mediante interfaces sin modificar código
- **L**iskov Substitution: Todas las implementaciones sustituibles por sus interfaces
- **I**nterface Segregation: Interfaces específicas y cohesivas
- **D**ependency Inversion: Dependencias mediante abstracciones (DI)

### Clean Architecture

- ? Independencia de frameworks
- ? Testeable en cada capa
- ? Independencia de UI
- ? Independencia de base de datos
- ? Regla de dependencia respetada

### Clean Code

- ? Nombres descriptivos y claros
- ? Métodos pequeños y cohesivos
- ? Sin código duplicado
- ? Comentarios XML en APIs públicas
- ? Sin código muerto

### Dependency Injection

- ? Contenedor: `Microsoft.Extensions.DependencyInjection`
- ? Todas las dependencias inyectadas
- ? Scopes apropiados (Singleton/Scoped/Transient)
- ? Sin validaciones null redundantes (C# 11 + NRT)

---

## ?? Casos de Prueba

### ? Casos Positivos

1. **Crear usuario con datos válidos**
   - Todos los campos completos y correctos
   - Resultado: Usuario creado exitosamente

2. **Crear múltiples usuarios**
   - Varios usuarios con emails diferentes
   - Resultado: Todos se crean con IDs únicos

### ? Casos Negativos

1. **Email duplicado**
   - Intentar crear usuario con email existente
   - Resultado: Error "El email ya está registrado"

2. **Clave con longitud incorrecta**
 - Clave con menos o más de 11 caracteres
   - Resultado: Error "La clave debe tener exactamente 11 caracteres"

3. **Email inválido**
   - Formato de email incorrecto
   - Resultado: Error "El formato del email no es válido"

4. **Campos vacíos**
   - Dejar campos obligatorios sin llenar
   - Resultado: Lista de errores de validación

Ver más casos de prueba en [`DEVELOPMENT.md`](DEVELOPMENT.md)

---

## ?? Tecnologías y Paquetes

### Stack Principal

- **.NET 8.0** - Framework
- **C# 12** - Lenguaje
- **Windows Forms** - UI
- **Microsoft.Extensions.DependencyInjection** - Contenedor DI

### Características de C# Utilizadas

- ? Nullable Reference Types (NRT)
- ? Record types
- ? Pattern matching
- ? Top-level statements
- ? Target-typed new expressions
- ? Init-only properties

---

## ?? Seguridad

- **Encriptación de claves**: SHA256 (hash unidireccional)
- **Sin texto plano**: Las claves nunca se almacenan sin encriptar
- **Thread-safety**: Operaciones en memoria protegidas con locks
- **Validación de entrada**: En UI y en lógica de negocio

?? **Nota de producción:** Para sistemas reales, se recomienda usar `BCrypt` o `Argon2` con salt automático en lugar de SHA256.

---

## ?? Documentación Adicional

- [`DEVELOPMENT.md`](DEVELOPMENT.md) - Guía técnica detallada, arquitectura y casos de prueba
- Comentarios XML en el código para IntelliSense

---

## ?? Futuras Extensiones (Sugeridas)

El proyecto está diseñado para ser fácilmente extensible:

- [ ] Modificación de usuarios (CRUD Update)
- [ ] Eliminación lógica (cambiar Estado a Baja)
- [ ] Búsqueda y filtros en el listado
- [ ] Sistema de roles y permisos
- [ ] Migración a base de datos (SQL Server + Entity Framework)
- [ ] Tests unitarios con xUnit/NUnit
- [ ] Logging con ILogger
- [ ] API REST

Para agregar nuevas funcionalidades, seguir el mismo patrón:
1. Agregar entidad en **DOM**
2. Crear interfaces en **ABS**
3. Implementar en **REPO/SERV**
4. Lógica de negocio en **APP**
5. UI en **PeluqueriaSystem**
6. Registrar en DI

---

## ?? Contribuir

Este es un proyecto educativo. Si deseas mejorarlo:

1. Mantén los principios SOLID y Clean Architecture
2. Sigue las convenciones de código existentes
3. Agrega tests para nuevas funcionalidades
4. Actualiza la documentación

---

## ?? Licencia

Este proyecto es de código abierto con fines educativos.

---

## ????? Autor

Desarrollado como proyecto académico para demostrar la aplicación práctica de:
- Clean Architecture
- Principios SOLID
- Clean Code
- Dependency Injection

---

## ?? Soporte

Para dudas o problemas:
1. Revisa la documentación en [`DEVELOPMENT.md`](DEVELOPMENT.md)
2. Verifica que el proyecto compile: `dotnet build`
3. Restaura paquetes NuGet si es necesario: `dotnet restore`

---

**Estado del Proyecto:** ? **Completo y Funcional**

**Compilación:** ? Sin errores ni warnings

**Cumplimiento:** ? 100% de requerimientos implementados
