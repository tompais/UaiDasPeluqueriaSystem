# Guía de Desarrollo - Sistema de Peluquería

Esta guía contiene información técnica detallada sobre la arquitectura, implementación y casos de prueba del sistema de gestión de usuarios con **CRUD completo**.

---

## 📑 Tabla de Contenidos

1. [Arquitectura Detallada](#arquitectura-detallada)
2. [Principios de Diseño](#principios-de-diseño)
3. [Implementación por Capas](#implementación-por-capas)
4. [Dependency Injection](#dependency-injection)
5. [Flujo de Datos](#flujo-de-datos)
6. [Validaciones](#validaciones)
7. [Seguridad](#seguridad)
8. [Base de Datos](#base-de-datos)
9. [Casos de Prueba](#casos-de-prueba)
10. [Optimizaciones](#optimizaciones)
11. [Escalabilidad](#escalabilidad)
12. [Checklist de Calidad](#checklist-de-calidad)

---

## 🏗️ Arquitectura Detallada

### Diagrama de Capas

```
┌───────────────────────────────────────┐
│        UI (PeluqueriaSystem)          │
│  - FormPrincipal (MDI Container)      │
│  - FormUsuarios (CRUD)           │
│  - FormAltaUsuario (Alta/Modificación)│
│  - DependencyInjectionContainer │
└───────────────────────────────────────┘
           │ Usa
        ↓
┌───────────────────────────────────────┐
│     APP (Lógica de Negocio)           │
│  - AppUsuario│
│    * Traer()             │
│    * TraerPorId(id)         │
│  * Crear(...)            │
│    * Modificar(...)      │
│    * Eliminar(id)            │
│    * ExisteEmail(email)    │
│    * ExisteEmailExcluyendoId(...)     │
└───────────────────────────────────────┘
   │ Usa
     ┌─────┴─────┐
     ↓    ↓
┌──────────────┐  ┌──────────────────────┐
│ REPO       │  │ SERV                 │
│ - RepoUsuario│  │ - EncriptacionService│
│   * Traer()  │  │   * Encriptar()      │
│   * TraerPorId│  └──────────────────────┘
│   * Crear()  │
│   * Modificar│
│   * Eliminar │
│   * ExisteEmail│
└──────────────┘
     │ Usa
     ↓
┌──────────────────┐
│ CONTEXT    │
│ - DalSQLServer   │
│   * AbrirConexion│
│   * CerrarConexion│
│   * EjecutarSQL  │
└──────────────────┘
     │ Usa
↓
┌──────────────────┐
│ SQL Server       │
│ - PeluSystem     │
│   * Usuario      │
│   * Rol          │
│   * Estado       │
└──────────────────┘
     │ Modela
     ↓
┌──────────────────┐
│ DOM (Dominio)    │
│ - DomUsuario     │
│   * ID           │
│   * Nombre       │
│   * Apellido     │
│   * Email        │
│   * Clave        │
│   * Rol (enum)   │
│   * Estado (enum)│
│   * DV           │
│   * Fecha_Agregar│
│   * FechaModificacion│
└──────────────────┘
      ↑
      │ Todas las capas dependen de
┌──────────────────┐
│ ABS (Abstracciones)│
│ - IUsuarioDbRepository│
│ - IDataAccess   │
│ - IEncriptacionService│
└──────────────────┘
```

### Proyectos y Responsabilidades

| Proyecto | Tipo | Responsabilidad | Dependencias |
|----------|------|----------------|--------------|
| **DOM** | Class Library | Entidades del dominio (DomUsuario, enums) | Ninguna |
| **ABS** | Class Library | Interfaces y abstracciones | DOM |
| **SERV** | Class Library | Servicios auxiliares (encriptación SHA256) | ABS |
| **CONTEXT** | Class Library | Acceso a datos SQL Server (DalSQLServer) | ABS, Microsoft.Data.SqlClient |
| **REPO** | Class Library | Repositorio CRUD (RepoUsuario) | ABS, CONTEXT, DOM, Microsoft.Data.SqlClient |
| **APP** | Class Library | Lógica de negocio (AppUsuario) | ABS, DOM, REPO, SERV |
| **PeluqueriaSystem** | WinForms App | Interfaz de usuario | Todos |

---

## 🎯 Principios de Diseño

### SOLID - Análisis Detallado

#### Single Responsibility Principle (SRP)

**✅ Cada clase tiene una única razón para cambiar**

- `DomUsuario`: Solo representa la entidad del dominio
  - Cambiaría si: Los atributos del usuario cambian
  
- `AppUsuario`: Solo contiene lógica de negocio
  - Cambiaría si: Las reglas de negocio cambian
  
- `RepoUsuario`: Solo maneja persistencia SQL
  - Cambiaría si: Las operaciones de BD cambian
  
- `EncriptacionService`: Solo encripta datos
  - Cambiaría si: El algoritmo de encriptación cambia

- `DalSQLServer`: Solo maneja conexiones SQL
  - Cambiaría si: La forma de conectar a SQL Server cambia

#### Open/Closed Principle (OCP)

**✅ Abierto para extensión, cerrado para modificación**

```csharp
// Ejemplo: Cambiar de SQL Server a MongoDB sin modificar APP

// Sin modificar IUsuarioDbRepository:
public class MongoUsuarioRepository : IUsuarioDbRepository
{
    // Nueva implementación con MongoDB
}

// Solo cambiar el registro en DI:
services.AddScoped<IUsuarioDbRepository, MongoUsuarioRepository>();
```

#### Dependency Inversion Principle (DIP)

**✅ Módulos de alto nivel no dependen de los de bajo nivel**

```csharp
// APP (alto nivel) no depende de REPO (bajo nivel)
// Ambos dependen de IUsuarioDbRepository (abstracción)

// APP
public class AppUsuario
{
    private readonly IUsuarioDbRepository _repository; // ✅ Abstracción
    
public AppUsuario(IUsuarioDbRepository repository)
    {
        _repository = repository;
    }
}

// REPO
public class RepoUsuario : IUsuarioDbRepository // ✅ Implementa abstracción
{
    // ...
}
```

---

## 💉 Dependency Injection

### Configuración del Contenedor

```csharp
public static class DependencyInjectionContainer
{
    public static void ConfigurarServicios()
{
  var services = new ServiceCollection();

        // Scoped - Nueva instancia por operación
        services.AddScoped<IDataAccess, DalSQLServer>();
        services.AddScoped<IUsuarioDbRepository, RepoUsuario>();
        services.AddScoped<IEncriptacionService, EncriptacionService>();
     services.AddScoped<AppUsuario>();

    // Transient - Nueva instancia cada vez
        services.AddTransient<FormPrincipal>();
        services.AddTransient<FormUsuarios>();
        services.AddTransient<FormAltaUsuario>();

        _serviceProvider = services.BuildServiceProvider();
    }
}
```

### Lifetimes Explicados

| Lifetime | Cuándo usar | Ejemplo en el proyecto |
|----------|-------------|----------------------|
| **Scoped** | Nueva instancia por operación/scope | `DalSQLServer`, `RepoUsuario`, `AppUsuario` |
| **Transient** | Muy ligeros, sin estado | `FormUsuarios`, `FormAltaUsuario` |

---

## 🔄 Flujo de Datos - Operaciones CRUD

### CREATE - Alta de Usuario

```
Usuario → FormAltaUsuario (ID=0) → AppUsuario.Crear()
  → Validar datos
  → ExisteEmail()
  → Encriptar clave (SHA256)
  → RepoUsuario.Crear()
    → INSERT INTO Usuario
  → Retornar usuario con ID
```

### READ - Listar Usuarios

```
Usuario → FormUsuarios.Load → AppUsuario.Traer()
  → RepoUsuario.Traer()
    → SELECT * FROM Usuario
    → CompletarLista(SqlDataReader)
  → DataGridView.DataSource = lista
```

### UPDATE - Modificar Usuario

```
Usuario → FormUsuarios (selecciona fila) → FormAltaUsuario (ID>0)
  → Cargar usuario: AppUsuario.TraerPorId(id)
  → Modificar campos
  → AppUsuario.Modificar()
    → Validar datos
    → ExisteEmailExcluyendoId() ← ¡IMPORTANTE!
    → Encriptar nueva clave (si se proporcionó)
    → RepoUsuario.Modificar()
      → UPDATE Usuario SET ..., FechaModificacion = GETDATE()
```

### DELETE - Eliminar Usuario

```
Usuario → FormUsuarios (selecciona fila) → Confirmar
  → AppUsuario.Eliminar(id)
    → RepoUsuario.Eliminar(id)
      → DELETE FROM Usuario WHERE ID = @ID
```

---

## ✅ Validaciones

### Niveles de Validación

#### 1. UI (Prevención)

```csharp
// FormAltaUsuario.Designer.cs
txtNombre.MaxLength = 50;
txtApellido.MaxLength = 80;
txtEmail.MaxLength = 180;
txtClave.MaxLength = 11;
txtClave.UseSystemPasswordChar = true;
```

#### 2. Lógica de Negocio (Reglas)

**En modo ALTA:**
```csharp
- Nombre: Obligatorio, ≤50 caracteres
- Apellido: Obligatorio, ≤80 caracteres
- Email: Obligatorio, formato válido, único, ≤180 caracteres
- Clave: Obligatoria, exactamente 11 caracteres
```

**En modo MODIFICACIÓN:**
```csharp
- Clave: Opcional (si no se proporciona, mantiene la actual)
- Email: Validar con ExisteEmailExcluyendoId() para permitir mantener el propio email
```

#### 3. Repositorio (Integridad)

```csharp
// Defensive programming en APIs públicas
public void Eliminar(int id)
{
    ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
    // ...
}
```

### Matriz de Validaciones CRUD

| Operación | Campo | Validación | Error |
|-----------|-------|------------|-------|
| **CREATE** | Email | No debe existir | "El email ya está registrado" |
| **CREATE** | Clave | Exactamente 11 caracteres | "La clave debe tener exactamente 11 caracteres" |
| **UPDATE** | Email | No debe estar en uso por **otro** usuario | "El email ya está registrado por otro usuario" |
| **UPDATE** | Clave | Opcional, si se proporciona: 11 caracteres | Mantiene actual si vacío |
| **DELETE** | ID | Debe existir | Eliminación silenciosa si no existe |

---

## 🔐 Seguridad

### Encriptación de Claves SHA256

```csharp
public class EncriptacionService : IEncriptacionService
{
    public string Encriptar(string textoPlano)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(textoPlano);
  
        var bytes = Encoding.UTF8.GetBytes(textoPlano);
var hash = SHA256.HashData(bytes);
        return Convert.ToBase64String(hash);
    }
}
```

**Ejemplo:**
```
Entrada:  "MiClave1234" (11 caracteres)
Salida:   "5nY8xR7vK3mP9qW2dF6hL1tG4jN8uB3xE7cA5zS2mK9=" (44 caracteres Base64)
```

### Prevención de Inyección SQL

✅ **CORRECTO - Parámetros tipados:**
```csharp
cmd.Parameters.Add("@Email", SqlDbType.VarChar, 180).Value = email;
cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;
```

❌ **EVITADO - AddWithValue (inferencia de tipos):**
```csharp
cmd.Parameters.AddWithValue("@Email", email); // Puede inferir mal el tipo
```

### Manejo Seguro de Conexiones

```csharp
public SqlConnection AbrirConexion()
{
    if (con.State == ConnectionState.Closed)
    {
  con.ConnectionString = StringConexion();
con.Open();
    }
    return con;
}

public void CerrarConexion()
{
 if (con.State != ConnectionState.Closed)
    {
        con.Close();
    }
}
```

---

## 🗄️ Base de Datos

### Esquema de la Tabla Usuario

```sql
CREATE TABLE [dbo].[Usuario] (
    [ID] INT NOT NULL IDENTITY(1,1),
    [Apellido] VARCHAR(80) NULL,
    [Nombre] VARCHAR(50) NULL,
  [Email] VARCHAR(180) NULL,
    [Rol] INT NOT NULL,
    [Estado] INT NOT NULL,
    [Clave] VARCHAR(64) NULL,  -- SHA256 Base64 = 44 caracteres
 [DV] VARCHAR(50) NULL,
    [Fecha_Agregar] DATETIME NOT NULL DEFAULT GETDATE(),
    [FechaModificacion] DATETIME NULL,
    CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED ([ID] ASC)
);
```

### Operaciones SQL

| Operación | Query |
|-----------|-------|
| **Traer** | `SELECT * FROM Usuario` |
| **TraerPorId** | `SELECT * FROM Usuario WHERE ID = @ID` |
| **Crear** | `INSERT INTO Usuario (...) VALUES (...); SELECT CAST(SCOPE_IDENTITY() as int)` |
| **Modificar** | `UPDATE Usuario SET ..., FechaModificacion = GETDATE() WHERE ID = @ID` |
| **Eliminar** | `DELETE FROM Usuario WHERE ID = @ID` |
| **ExisteEmail** | `SELECT COUNT(*) FROM Usuario WHERE Email = @Email` |
| **ExisteEmailExcluyendoId** | `SELECT COUNT(*) FROM Usuario WHERE Email = @Email AND ID != @ID` |

---

## 🧪 Casos de Prueba

### ✅ Casos Positivos CRUD

#### CP01: Crear Usuario Válido
**Procedimiento:**
1. Abrir formulario de alta
2. Completar todos los campos correctamente
3. Clave de exactamente 11 caracteres
4. Guardar

**Resultado esperado:**
- ✅ Usuario creado con ID autoincremental
- ✅ Aparece en lista
- ✅ Clave hasheada (44 caracteres)

---

#### CP02: Modificar Usuario - Mantener Clave
**Procedimiento:**
1. Seleccionar usuario en lista
2. Clic en "Modificar"
3. Cambiar nombre/apellido
4. Dejar clave vacía
5. Guardar

**Resultado esperado:**
- ✅ Datos actualizados
- ✅ Clave no cambia
- ✅ `FechaModificacion` actualizada

---

#### CP03: Modificar Usuario - Cambiar Clave
**Procedimiento:**
1. Seleccionar usuario
2. Modificar
3. Ingresar nueva clave (11 caracteres)
4. Guardar

**Resultado esperado:**
- ✅ Clave hasheada nuevamente
- ✅ Hash diferente al anterior

---

#### CP04: Modificar Usuario - Mantener Email
**Procedimiento:**
1. Usuario con email: test@test.com
2. Modificar usuario
3. Mantener email: test@test.com
4. Guardar

**Resultado esperado:**
- ✅ Permite guardar sin error de "email duplicado"

---

#### CP05: Eliminar Usuario
**Procedimiento:**
1. Seleccionar usuario
2. Clic en "Eliminar"
3. Confirmar

**Resultado esperado:**
- ✅ Muestra confirmación
- ✅ Usuario eliminado de BD
- ✅ Desaparece de lista

---

### ❌ Casos Negativos CRUD

#### CN01: Email Duplicado en Alta
```
Usuario 1: test@test.com → Creado
Usuario 2: test@test.com → ❌ Error
```
**Error esperado:** `"El email ya está registrado"`

---

#### CN02: Email Duplicado en Modificación
```
Usuario 1: usuario1@test.com
Usuario 2: usuario2@test.com
Modificar Usuario 2 → email: usuario1@test.com → ❌ Error
```
**Error esperado:** `"El email ya está registrado por otro usuario"`

---

#### CN03: Clave Incorrecta en Alta
```
Clave: "1234567890" (10 caracteres) → ❌ Error
```
**Error esperado:** `"La clave debe tener exactamente 11 caracteres"`

---

#### CN04: Clave Incorrecta en Modificación (si se proporciona)
```
Nueva clave: "123456" (6 caracteres) → ❌ Error
```
**Error esperado:** `"La clave debe tener exactamente 11 caracteres"`

---

### 🔄 Casos de Integración

#### CI01: Flujo Completo CRUD
**Procedimiento:**
1. Crear usuario1
2. Crear usuario2
3. Listar (debe mostrar 2)
4. Modificar usuario1
5. Listar (verifica cambio)
6. Eliminar usuario2
7. Listar (solo usuario1)

**Resultado:** ✅ Todo funciona correctamente

---

## ⚡ Optimizaciones Implementadas

### 1. Parámetros SQL Tipados

**Antes (menos eficiente):**
```csharp
cmd.Parameters.AddWithValue("@Email", email);
```

**Ahora (optimizado):**
```csharp
cmd.Parameters.Add("@Email", SqlDbType.VarChar, 180).Value = email;
```

**Beneficios:**
- ✅ Sin inferencia de tipos en cada llamada
- ✅ SQL Server puede optimizar planes de ejecución
- ✅ Previene conversiones implícitas

### 2. Manejo de Estado de Conexión

```csharp
public SqlConnection AbrirConexion()
{
    if (con.State == ConnectionState.Closed)
    {
  con.ConnectionString = StringConexion();
con.Open();
    }
    return con;
}

public void CerrarConexion()
{
 if (con.State != ConnectionState.Closed)
    {
        con.Close();
    }
}
```

**Beneficios:**
- ✅ Previene errores de "conexión ya abierta"
- ✅ Previene errores de "conexión ya cerrada"
- ✅ Código más robusto

### 3. Validación de Email con Exclusión

```csharp
// Evita falsos positivos en modificación
public bool ExisteEmailExcluyendoId(string email, int idExcluir)
{
    // SELECT COUNT(*) WHERE Email = @Email AND ID != @ID
}
```

**Beneficio:**
- ✅ Usuario puede mantener su propio email al modificar

---

## 🔮 Escalabilidad

### Cómo Extender el Sistema

#### 1. Agregar Entity Framework Core

```bash
dotnet add REPO package Microsoft.EntityFrameworkCore.SqlServer
```

```csharp
public class PeluqueriaDbContext : DbContext
{
    public DbSet<DomUsuario> Usuarios { get; set; }
}

public class EfUsuarioRepository : IUsuarioDbRepository
{
    private readonly PeluqueriaDbContext _context;
    
    public List<DomUsuario> Traer() => _context.Usuarios.ToList();
}
```

**Cambio en DI:**
```csharp
services.AddDbContext<PeluqueriaDbContext>();
services.AddScoped<IUsuarioDbRepository, EfUsuarioRepository>();
```

#### 2. Agregar Tests Unitarios

```csharp
[Fact]
public void Crear_ConEmailDuplicado_DebeRetornarError()
{
    // Arrange
    var mockRepo = new Mock<IUsuarioDbRepository>();
    mockRepo.Setup(r => r.ExisteEmail(It.IsAny<string>())).Returns(true);
    var service = new AppUsuario(mockRepo.Object, null);
    
    // Act & Assert
    Assert.Throws<InvalidOperationException>(() => 
        service.Crear("Juan", "Pérez", "duplicado@test.com", "12345678901", DomUsuario.RolUsuario.Cliente)
    );
}
```

---

## ✅ Checklist de Calidad

### Antes de Commitear

#### Compilación
- [ ] `dotnet build` sin errores
- [ ] `dotnet build` sin warnings
- [ ] Todas las referencias correctas

#### Funcionalidad CRUD
- [ ] CREATE funciona con datos válidos
- [ ] READ lista todos los usuarios
- [ ] UPDATE modifica correctamente
  - [ ] Mantiene clave si no se proporciona
  - [ ] Permite mantener propio email
- [ ] DELETE elimina con confirmación

#### Código
- [ ] Sin código comentado
- [ ] Sin TODOs pendientes
- [ ] Sin console.WriteLine()
- [ ] Nombres descriptivos
- [ ] Métodos < 30 líneas

#### Arquitectura
- [ ] Dependencias correctas
- [ ] UI no llama directamente a REPO
- [ ] APP no depende de Windows Forms

#### Base de Datos
- [ ] Connection string configurado
- [ ] Scripts SQL ejecutados
- [ ] Conexiones se cierran correctamente

---

## 📚 Referencias

### Documentación Oficial
- [Microsoft.Data.SqlClient](https://docs.microsoft.com/sql/connect/ado-net/sql)
- [Dependency Injection en .NET](https://docs.microsoft.com/dotnet/core/extensions/dependency-injection)
- [C# 12 Features](https://docs.microsoft.com/dotnet/csharp/whats-new/csharp-12)

### Mejores Prácticas
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [SOLID Principles](https://en.wikipedia.org/wiki/SOLID)
- [SQL Injection Prevention](https://cheatsheetseries.owasp.org/cheatsheets/SQL_Injection_Prevention_Cheat_Sheet.html)

---

**Última actualización:** 2025
**Versión:** 2.0 - CRUD Completo
**Mantenedor:** Sistema de Gestión Peluquería
