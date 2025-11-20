# Sistema de Gestión para Peluquería

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-12.0-239120)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

Sistema de gestión de usuarios para peluquería desarrollado con **Windows Forms (.NET 8)** siguiendo principios de **Clean Architecture**, **SOLID**, y **Clean Code**.

---

## 📋 Descripción del Proyecto

Este proyecto implementa la funcionalidad de **gestión completa de usuarios (CRUD)** para un sistema de gestión de peluquería, demostrando la aplicación práctica de:

- 🏗 **Clean Architecture** - Separación en capas con proyectos independientes
- 💎 **SOLID** - Todos los principios aplicados
- ✨ **Clean Code** - Código limpio, claro y mantenible
- 🔄 **DRY** - Sin duplicación de lógica
- 🎯 **YAGNI** - Solo lo necesario implementado
- 💉 **Dependency Injection** - Desacoplamiento mediante interfaces

🔗 **Nota:** Este proyecto utiliza **SQL Server** para persistencia de datos. La arquitectura sigue un modelo N-capas con acceso a base de datos real.

---

## 🚀 Inicio Rápido

### Prerrequisitos

- [Visual Studio 2022](https://visualstudio.microsoft.com/) o superior
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/sql-server) (Express, Developer o superior)
- Windows 10/11

### Configurar Base de Datos

1. **Ejecutar script SQL**
   ```bash
   # Desde la raíz del proyecto
   sqlcmd -S tu_servidor -d master -E -i "PeluqueriaSystem\Database\00_CompleteScript_Standalone.sql"
   
   # O con autenticación SQL
   sqlcmd -S tu_servidor -U tu_usuario -P tu_contraseña -i "PeluqueriaSystem\Database\00_CompleteScript_Standalone.sql"
   ```

2. **Configurar connection string**
   
   Opción A - Variable de entorno (recomendado):
   ```powershell
   $env:PELUQUERIA_CONNECTIONSTRING = "Data Source=tu_servidor;Initial Catalog=PeluSystem;User ID=tu_usuario;Password=tu_contraseña;TrustServerCertificate=True"
   ```

   Opción B - Editar `CONTEXT/DalSQLServer.cs`:
   ```csharp
   private static string StringConexion() =>
       Environment.GetEnvironmentVariable("PELUQUERIA_CONNECTIONSTRING")
       ?? "Data Source=tu_servidor;Initial Catalog=PeluSystem;Integrated Security=True;TrustServerCertificate=True";
   ```

### Ejecutar el Proyecto

1. **Clonar o abrir la solución**
   ```bash
   git clone https://github.com/tompais/UaiDasPeluqueriaSystem
   cd UaiDasPeluqueriaSystem
   ```

2. **Restaurar paquetes**
   ```bash
   dotnet restore
   ```

3. **Compilar**
 ```bash
   dotnet build
   ```

4. **Ejecutar**
   ```bash
   dotnet run --project PeluqueriaSystem
 ```
   O presionar **F5** en Visual Studio

5. **Usar la aplicación**
   - En el formulario principal, ir a: **Administración → Usuarios**
   - **Nuevo Usuario**: Crear un nuevo usuario
   - **Modificar**: Editar usuario seleccionado
   - **Eliminar**: Borrar usuario seleccionado (con confirmación)
   - ⚠️ La clave debe tener **exactamente 11 caracteres**

---

## 🏛️ Arquitectura

### Estructura de N Capas

```
PeluqueriaSystem.sln
│
├── DOM/ # 📦 Entidades del dominio (DomUsuario, enums)
├── ABS/        # 🔌 Interfaces y abstracciones
├── SERV/    # ⚙️ Servicios auxiliares (EncriptacionService, Encriptar)
├── CONTEXT/       # 🗄️ Acceso a datos SQL Server (DalSQLServer)
├── REPO/       # 💾 Repositorio CRUD (RepoUsuario)
├── APP/           # 🧠 Lógica de negocio (AppUsuario)
└── PeluqueriaSystem/  # 🖥️ Interfaz de usuario (Windows Forms)
    ├── FormPrincipal.cs
    ├── FormUsuarios.cs    # Listado con CRUD
    └── FormAltaUsuario.cs     # Alta/Modificación
```

### Flujo de Dependencias

```
  UI (PeluqueriaSystem)
     ↓
    APP (AppUsuario)
      ↓
    REPO (RepoUsuario) + SERV (EncriptacionService)
     ↓
    CONTEXT (DalSQLServer)
       ↓
    SQL Server (PeluSystem)
         ↓
    DOM (DomUsuario)
      ↑
    ABS (Interfaces) ← Todas las capas dependen de abstracciones
```

**Principio clave:** Las dependencias apuntan hacia adentro (hacia el dominio), con persistencia real en SQL Server.

---

## ✨ Características Implementadas

### Funcionalidad CRUD Completa de Usuarios

| Operación | Descripción | Implementado |
|-----------|-------------|--------------|
| **Create** | Alta de nuevos usuarios con validaciones | ✅ |
| **Read** | Listado de todos los usuarios | ✅ |
| **Update** | Modificación de usuarios existentes | ✅ |
| **Delete** | Eliminación con confirmación | ✅ |

### Características Adicionales

| Característica | Descripción |
|----------------|-------------|
| 🖼️ **Formulario MDI** | Contenedor principal con menú de navegación |
| 📊 **DataGridView** | Listado profesional con columnas configurables |
| ✏️ **Modo Alta/Modificación** | Un solo formulario para ambas operaciones |
| ✅ **Validaciones robustas** | En UI y en lógica de negocio |
| 🔐 **Encriptación MD5** | Claves hasheadas (32 caracteres hexadecimales) |
| 📧 **Email único** | Validación con exclusión de ID en modificación |
| 🔢 **IDs autogenerados** | Gestionados por SQL Server (IDENTITY) |
| 📅 **Auditoría** | FechaAgregar y FechaModificacion automáticas |
| 💾 **Persistencia SQL** | Datos almacenados en SQL Server |
| 🎨 **UI profesional** | Diseño limpio y funcional |

### Campos del Usuario

| Campo | Tipo | Tamaño | Validación |
|-------|------|--------|------------|
| **ID** | int | - | Autogenerado, único, PK |
| **Nombre** | varchar | 50 | Obligatorio |
| **Apellido** | varchar | 80 | Obligatorio |
| **Email** | varchar | 180 | Formato válido, único |
| **Clave** | varchar | 64 | **11 caracteres** (hasheada a 32) |
| **Rol** | int | - | 0-3 (Cliente/Empleado/Supervisor/Admin) |
| **Estado** | int | - | 0-1 (Activo/Baja) |
| **DV** | varchar | 50 | Dígito verificador |
| **Fecha_Agregar** | datetime | - | Automático (GETDATE()) |
| **FechaModificacion** | datetime | - | Automático en UPDATE |

---

## 🎯 Principios Aplicados

### SOLID

- **S**ingle Responsibility: Cada clase tiene una única responsabilidad
  - `RepoUsuario`: Solo operaciones de BD
  - `AppUsuario`: Solo lógica de negocio
  - `EncriptacionService`: Solo encriptación
  
- **O**pen/Closed: Extensible mediante interfaces sin modificar código
  - Se puede cambiar de SQL Server a otro provider sin afectar APP
  
- **L**iskov Substitution: Implementaciones intercambiables
  - Cualquier `IUsuarioDbRepository` funciona igual
  
- **I**nterface Segregation: Interfaces específicas y cohesivas
  - `IDataAccess`, `IUsuarioDbRepository`, `IEncriptacionService` separadas
  
- **D**ependency Inversion: Dependencias mediante abstracciones
  - APP depende de `IUsuarioDbRepository`, no de `RepoUsuario`

### Clean Architecture

- 🔒 Independencia de frameworks
- ✅ Testeable en cada capa
- 🖥️ Independencia de UI
- 💾 Independencia de base de datos
- ➡️ Regla de dependencia respetada

### Clean Code

- 📝 Nombres descriptivos y claros
- 🔧 Métodos pequeños y cohesivos
- 🚫 Sin código duplicado
- 💬 Comentarios XML en APIs públicas
- ♻️ Sin código muerto

### Dependency Injection

- 📦 Contenedor: `Microsoft.Extensions.DependencyInjection`
- 💉 Todas las dependencias inyectadas
- ⏱️ Scopes apropiados (Singleton/Scoped/Transient)
- ✨ Nullable Reference Types habilitados

---

## 🧪 Casos de Prueba

### ✅ Casos Positivos

1. **Crear usuario válido**
   - Resultado: Usuario creado con ID autoincremental

2. **Modificar usuario existente**
   - Sin cambiar clave: Mantiene la clave actual
   - Cambiando clave: Se hashea la nueva
   - Resultado: `FechaModificacion` actualizada

3. **Eliminar usuario**
   - Confirmación requerida
   - Resultado: Usuario eliminado de BD

4. **Listar usuarios**
   - Muestra todos los usuarios con paginación en DataGridView

### ❌ Casos Negativos

1. **Email duplicado en alta**
   - Error: "El email ya está registrado"

2. **Email duplicado en modificación**
   - Si intenta cambiar a email de otro usuario
   - Error: "El email ya está registrado por otro usuario"

3. **Clave con longitud incorrecta**
   - Error: "La clave debe tener exactamente 11 caracteres"

4. **Campos obligatorios vacíos**
   - Error: Lista de validaciones

Ver más casos en [`DEVELOPMENT.md`](PeluqueriaSystem/DEVELOPMENT.md)

---

## 🛠️ Tecnologías y Paquetes

### Stack Principal

- **.NET 8.0** - Framework
- **C# 12** - Lenguaje con Nullable Reference Types
- **Windows Forms** - UI
- **SQL Server** - Base de datos relacional
- **Microsoft.Data.SqlClient 6.1.2** - Provider moderno de ADO.NET
- **Microsoft.Extensions.DependencyInjection** - Contenedor DI

### Características de C# 12 Utilizadas

- ✨ Nullable Reference Types (NRT)
- 🎯 Primary constructors
- 🔀 Collection expressions `[]`
- 📦 Target-typed new expressions
- 🔒 Init-only properties
- 💡 Pattern matching

### Mejoras de Performance

- 🚀 Parámetros SQL tipados (`SqlDbType`) en lugar de `AddWithValue`
- 🔄 Manejo correcto de estado de conexión
- 📊 Uso de `using` statements para liberar recursos
- ⚡ Consultas optimizadas con índices

---

## 🔐 Seguridad

### Encriptación de Claves

#### MD5 (Principal)
- **Algoritmo:** MD5 (hash de 128 bits)
- **Output:** Hexadecimal (32 caracteres)
- **Clase:** `EncriptacionService`
- **Características:**
  - ✅ Hash unidireccional (no reversible)
  - ✅ Determinista
  - ⚠️ Sin salt (contexto educativo)

**Ejemplo:**
```
Entrada:  "MiClave1234" (11 caracteres)
Salida:   "0871A29869FB7B8B58235C472213C23E" (32 caracteres hexadecimales)
```

**Uso:**
```csharp
// A través del servicio de encriptación (usado por AppUsuario)
string hash = encriptacionService.Encriptar("MiClave1234");
```

#### Clase auxiliar Encriptar
- **Clase:** `Encriptar`
- **Método:** `static string CreateMD5(string input)`
- Proporciona acceso estático al mismo algoritmo MD5 para casos de uso auxiliares

**Ejemplo:**
```csharp
string hash = Encriptar.CreateMD5("MiClave1234");
// Resultado: "0871A29869FB7B8B58235C472213C23E"
```

### Prevención de Inyección SQL

- ✅ Todos los comandos usan `SqlParameter`
- ✅ Tipos de datos especificados correctamente
- ✅ Tamaños de columnas definidos

### Integridad de Datos

- ✅ Foreign Keys en base de datos
- ✅ Transacciones implícitas en operaciones
- ✅ Validación de email único con exclusión de ID
- ✅ Try-finally garantiza cierre de conexiones

⚠️ **Nota de producción:** Para sistemas reales, usar `BCrypt` o `Argon2` con salt automático.

---

## 📚 Documentación Adicional

- [`DEVELOPMENT.md`](PeluqueriaSystem/DEVELOPMENT.md) - Guía técnica detallada, arquitectura y casos de prueba
- Comentarios XML en el código para IntelliSense
- Scripts SQL documentados en `PeluqueriaSystem/Database/`

---

## 🔮 Futuras Extensiones (Sugeridas)

El proyecto está diseñado para ser fácilmente extensible:

- [ ] Sistema de roles y permisos avanzado
- [ ] Búsqueda y filtros en el listado
- [ ] Exportación a Excel/PDF
- [ ] Auditoría completa de cambios
- [ ] Cambio de clave por el usuario
- [ ] Recuperación de contraseña por email
- [ ] Tests unitarios con xUnit/NUnit + Moq
- [ ] Logging con Serilog
- [ ] Migración a Entity Framework Core
- [ ] API REST para integración

Para agregar nuevas funcionalidades, seguir el mismo patrón arquitectónico.

---

## 🤝 Contribuir

Este es un proyecto educativo. Si deseas mejorarlo:

1. Mantén los principios SOLID y Clean Architecture
2. Sigue las convenciones de código existentes
3. Agrega tests para nuevas funcionalidades
4. Actualiza la documentación

---

## 📝 Licencia

Este proyecto es de código abierto con fines educativos.

---

## 👨‍💻 Autor

Desarrollado como proyecto académico para demostrar la aplicación práctica de:
- Clean Architecture en Windows Forms
- Principios SOLID en .NET 8
- Clean Code con C# 12
- Dependency Injection
- Persistencia con SQL Server

---

## 💬 Soporte

Para dudas o problemas:
1. Revisa la documentación en [`DEVELOPMENT.md`](PeluqueriaSystem/DEVELOPMENT.md)
2. Verifica que el proyecto compile: `dotnet build`
3. Verifica la conexión a SQL Server
4. Restaura paquetes NuGet si es necesario: `dotnet restore`

---

**Estado del Proyecto:** ✅ **Completo y Funcional con CRUD**

**Compilación:** ✅ Sin errores ni warnings

**Cumplimiento:** ✅ 100% de requerimientos implementados

**Base de Datos:** ✅ SQL Server con scripts incluidos
