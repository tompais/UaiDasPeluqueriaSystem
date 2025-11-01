# Sistema de Gesti�n para Peluquer�a

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-12.0-239120)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

Sistema de gesti�n de usuarios para peluquer�a desarrollado con **Windows Forms (.NET 8)** siguiendo principios de **Clean Architecture**, **SOLID**, y **Clean Code**.

---

## ?? Descripci�n del Proyecto

Este proyecto implementa la funcionalidad de **Alta de Usuario (CU8.1)** para un sistema de gesti�n de peluquer�a, demostrando la aplicaci�n pr�ctica de:

- ? **Clean Architecture** - Separaci�n en capas con proyectos independientes
- ? **SOLID** - Todos los principios aplicados
- ? **Clean Code** - C�digo limpio, claro y mantenible
- ? **DRY** - Sin duplicaci�n de l�gica
- ? **YAGNI** - Solo lo necesario implementado
- ? **Dependency Injection** - Desacoplamiento mediante interfaces

?? **Nota:** Este proyecto utiliza **SQL Server** para persistencia de datos. La arquitectura sigue un modelo N-capas con acceso a base de datos real.

---

## ?? Inicio R�pido

### Prerrequisitos

- [Visual Studio 2022](https://visualstudio.microsoft.com/) o superior
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Windows 10/11

### Ejecutar el Proyecto

1. **Clonar o abrir la soluci�n**
   ```bash
   # Si usas git
   git clone [url-del-repo]
   cd PeluqueriaSystem
   ```

2. **Abrir en Visual Studio**
   - Doble clic en `PeluqueriaSystem.sln`
   - O desde Visual Studio: `Archivo > Abrir > Proyecto/Soluci�n`

3. **Compilar y ejecutar**
   - Presionar **F5** o clic en el bot�n "? Iniciar"
   - O usar el comando:
     ```bash
     dotnet run --project PeluqueriaSystem
     ```

4. **Usar la aplicaci�n**
   - En el formulario principal, ir a: **Administraci�n ? Usuarios**
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
??? APP/           # ?? L�gica de negocio (UsuarioService)
??? PeluqueriaSystem/ # ??? Interfaz de usuario (Windows Forms)
    ??? frmPrincipal.cs      # MDI Principal
    ??? frmUsuarios.cs  # Listado
    ??? frmAltaUsuario.cs    # Alta de usuario
```

### Flujo de Dependencias

```
    UI (PeluqueriaSystem)
  ?
    APP (L�gica de Negocio - AppUsuario)
  ?
    REPO (Repositorio - RepoUsuario) + SERV (Servicios - EncriptacionService)
      ?
    CONTEXT (Acceso a Datos - DalSQLServer)
           ?
    SQL Server Database (PeluSystem)
           ?
    DOM (Entidades - DomUsuario)
           ?
    ABS (Interfaces) ? Todas las capas dependen de abstracciones
```

**Principio clave:** Las dependencias apuntan hacia adentro (hacia el dominio), con persistencia en SQL Server.

---

## ? Caracter�sticas Implementadas

### Funcionalidad CU8.1 - Alta de Usuario

| Caracter�stica | Descripci�n |
|---------------|-------------|
| ? **Formulario MDI** | Contenedor principal con men� de navegaci�n |
| ? **Listado de usuarios** | DataGridView con todos los usuarios registrados |
| ? **Alta de usuario** | Formulario modal con validaciones completas |
| ? **Validaciones robustas** | En UI y en l�gica de negocio |
| ? **Encriptaci�n SHA256** | Claves nunca en texto plano |
| ? **Email �nico** | No permite emails duplicados |
| ? **IDs autogenerados** | Asignaci�n autom�tica por SQL Server (IDENTITY) |
| ? **Persistencia SQL** | Datos almacenados en SQL Server |

### Campos del Usuario

| Campo | Tipo | Validaci�n |
|-------|------|------------|
| **ID** | int | Autogenerado, �nico, no nulo |
| **Nombre** | string | Obligatorio, m�x. 50 caracteres |
| **Apellido** | string | Obligatorio, m�x. 80 caracteres |
| **Email** | string | Formato v�lido, �nico, m�x. 180 caracteres |
| **Clave** | string | **Exactamente 11 caracteres**, encriptada SHA256 |
| **Estado** | enum | Activo (0) o Baja (1) |
| **Rol** | int | Rango 0-9 |
| **FechaCreacion** | DateTime | Autom�tico al crear |
| **UsuarioCreacion** | string | Registrado en el sistema |

---

## ?? Principios Aplicados

### SOLID

- **S**ingle Responsibility: Cada clase tiene una �nica responsabilidad
- **O**pen/Closed: Extensible mediante interfaces sin modificar c�digo
- **L**iskov Substitution: Todas las implementaciones sustituibles por sus interfaces
- **I**nterface Segregation: Interfaces espec�ficas y cohesivas
- **D**ependency Inversion: Dependencias mediante abstracciones (DI)

### Clean Architecture

- ? Independencia de frameworks
- ? Testeable en cada capa
- ? Independencia de UI
- ? Independencia de base de datos
- ? Regla de dependencia respetada

### Clean Code

- ? Nombres descriptivos y claros
- ? M�todos peque�os y cohesivos
- ? Sin c�digo duplicado
- ? Comentarios XML en APIs p�blicas
- ? Sin c�digo muerto

### Dependency Injection

- ? Contenedor: `Microsoft.Extensions.DependencyInjection`
- ? Todas las dependencias inyectadas
- ? Scopes apropiados (Singleton/Scoped/Transient)
- ? Sin validaciones null redundantes (C# 11 + NRT)

---

## ?? Casos de Prueba

### ? Casos Positivos

1. **Crear usuario con datos v�lidos**
   - Todos los campos completos y correctos
   - Resultado: Usuario creado exitosamente

2. **Crear m�ltiples usuarios**
   - Varios usuarios con emails diferentes
   - Resultado: Todos se crean con IDs �nicos

### ? Casos Negativos

1. **Email duplicado**
   - Intentar crear usuario con email existente
   - Resultado: Error "El email ya est� registrado"

2. **Clave con longitud incorrecta**
 - Clave con menos o m�s de 11 caracteres
   - Resultado: Error "La clave debe tener exactamente 11 caracteres"

3. **Email inv�lido**
   - Formato de email incorrecto
   - Resultado: Error "El formato del email no es v�lido"

4. **Campos vac�os**
   - Dejar campos obligatorios sin llenar
   - Resultado: Lista de errores de validaci�n

Ver m�s casos de prueba en [`DEVELOPMENT.md`](DEVELOPMENT.md)

---

## ?? Tecnolog�as y Paquetes

### Stack Principal

- **.NET 8.0** - Framework
- **C# 12** - Lenguaje
- **Windows Forms** - UI
- **SQL Server** - Base de datos
- **System.Data.SqlClient** - Proveedor de datos ADO.NET
- **Microsoft.Extensions.DependencyInjection** - Contenedor DI

### Caracter�sticas de C# Utilizadas

- ? Nullable Reference Types (NRT)
- ? Record types
- ? Pattern matching
- ? Top-level statements
- ? Target-typed new expressions
- ? Init-only properties

---

## ?? Seguridad

- **Encriptaci�n de claves**: SHA256 (hash unidireccional)
- **Sin texto plano**: Las claves nunca se almacenan sin encriptar
- **Transacciones SQL**: Integridad de datos garantizada
- **Validaci�n de entrada**: En UI y en l�gica de negocio
- **Conexi�n segura**: Integrated Security con SQL Server

?? **Nota de producci�n:** Para sistemas reales, se recomienda usar `BCrypt` o `Argon2` con salt autom�tico en lugar de SHA256.

---

## ?? Documentaci�n Adicional

- [`DEVELOPMENT.md`](DEVELOPMENT.md) - Gu�a t�cnica detallada, arquitectura y casos de prueba
- Comentarios XML en el c�digo para IntelliSense

---

## ?? Futuras Extensiones (Sugeridas)

El proyecto est� dise�ado para ser f�cilmente extensible:

- [ ] Modificaci�n de usuarios (CRUD Update)
- [ ] Eliminaci�n l�gica (cambiar Estado a Baja)
- [ ] B�squeda y filtros en el listado
- [ ] Sistema de roles y permisos extendido
- [ ] Migraci�n a Entity Framework Core
- [ ] Tests unitarios con xUnit/NUnit
- [ ] Logging con ILogger
- [ ] API REST

Para agregar nuevas funcionalidades, seguir el mismo patr�n:
1. Agregar entidad en **DOM**
2. Crear interfaces en **ABS**
3. Implementar en **REPO/SERV**
4. L�gica de negocio en **APP**
5. UI en **PeluqueriaSystem**
6. Registrar en DI

---

## ?? Contribuir

Este es un proyecto educativo. Si deseas mejorarlo:

1. Mant�n los principios SOLID y Clean Architecture
2. Sigue las convenciones de c�digo existentes
3. Agrega tests para nuevas funcionalidades
4. Actualiza la documentaci�n

---

## ?? Licencia

Este proyecto es de c�digo abierto con fines educativos.

---

## ????? Autor

Desarrollado como proyecto acad�mico para demostrar la aplicaci�n pr�ctica de:
- Clean Architecture
- Principios SOLID
- Clean Code
- Dependency Injection

---

## ?? Soporte

Para dudas o problemas:
1. Revisa la documentaci�n en [`DEVELOPMENT.md`](DEVELOPMENT.md)
2. Verifica que el proyecto compile: `dotnet build`
3. Restaura paquetes NuGet si es necesario: `dotnet restore`

---

**Estado del Proyecto:** ? **Completo y Funcional**

**Compilaci�n:** ? Sin errores ni warnings

**Cumplimiento:** ? 100% de requerimientos implementados
