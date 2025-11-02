# Gu�a de Desarrollo - Sistema de Peluquer�a

Esta gu�a contiene informaci�n t�cnica detallada sobre la arquitectura, implementaci�n y casos de prueba del sistema.

---

## ?? Tabla de Contenidos

1. [Arquitectura Detallada](#arquitectura-detallada)
2. [Principios de Dise�o](#principios-de-dise�o)
3. [Implementaci�n por Capas](#implementaci�n-por-capas)
4. [Dependency Injection](#dependency-injection)
5. [Flujo de Datos](#flujo-de-datos)
6. [Validaciones](#validaciones)
7. [Seguridad](#seguridad)
8. [Casos de Prueba](#casos-de-prueba)
9. [Escalabilidad](#escalabilidad)
10. [Checklist de Calidad](#checklist-de-calidad)

---

## ??? Arquitectura Detallada

### Diagrama de Capas

```
???????????????????????????????????????????????????????????????
?        UI (PeluqueriaSystem)     ?
?  - FormPrincipal (MDI Container)        ?
?  - FormUsuarios (Listado)           ?
?  - FormAltaUsuario (Alta)       ?
?  - DependencyInjectionContainer  ?
???????????????????????????????????????????????????????????????
           ? Usa
   ?
???????????????????????????????????????????????????????????????
?     APP (L�gica de Negocio)        ?
?  - AppUsuario              ?
???????????????????????????????????????????????????????????????
     ? Usa
 ?????????????????????????????????
         ?              ?
??????????????????????????????????????????????????
?   REPO (Repositorio)  ?    ?   SERV (Servicios)     ?
? - RepoUsuario  ? ? - EncriptacionService  ?
????????????????????????    ??????????????????????????
        ? Usa
   ?
???????????????????????????
? CONTEXT (Acceso Datos) ?
?  - DalSQLServer   ?
???????????????????????????
           ? Usa
      ?
???????????????????????????
?  SQL Server Database ?
?  - PeluSystem      ?
?  - Tabla Usuario   ?
???????????????????????????
           ? Usa
      ?
???????????????????????????
?    DOM (Dominio)        ?
?  - DomUsuario        ?
?  - RolUsuario (enum)    ?
?  - EstadoUsuario (enum) ?
???????????????????????????
        ?
      ? Todas las capas dependen de
???????????????????????????
?  ABS (Abstracciones) ?
?  - IUsuarioDbRepository   ?
?  - IDataAccess      ?
?  - IEncriptacionService ?
???????????????????????????
```

### Proyectos y Responsabilidades

| Proyecto | Tipo | Responsabilidad | Dependencias |
|----------|------|----------------|--------------|
| **DOM** | Class Library | Entidades del dominio (DomUsuario, enums) | Ninguna |
| **ABS** | Class Library | Interfaces y abstracciones | DOM |
| **SERV** | Class Library | Servicios auxiliares (encriptaci�n) | ABS |
| **CONTEXT** | Class Library | Acceso a datos SQL Server (DalSQLServer) | ABS |
| **REPO** | Class Library | Repositorio con operaciones de BD (RepoUsuario) | ABS, CONTEXT, DOM |
| **APP** | Class Library | L�gica de negocio (AppUsuario) | ABS, DOM, REPO, SERV |
| **PeluqueriaSystem** | WinForms App | Interfaz de usuario | Todos |

### Flujo de Dependencias

```
Regla fundamental: Las dependencias apuntan hacia adentro

Externo ? Interno:
UI ? APP ? REPO/SERV ? CONTEXT ? SQL Server ? DOM

Todos dependen de ABS (abstracciones)
```

---

## ?? Principios de Dise�o

### SOLID - An�lisis Detallado

#### Single Responsibility Principle (SRP)

**? Cada clase tiene una �nica raz�n para cambiar**

- `Usuario`: Solo representa la entidad del dominio
  - Cambiar�a si: Los atributos del usuario cambian
  
- `UsuarioService`: Solo contiene l�gica de negocio
  - Cambiar�a si: Las reglas de negocio cambian
  
- `UsuarioRepository`: Solo maneja persistencia
  - Cambiar�a si: La forma de almacenar datos cambia
  
- `EncriptacionService`: Solo encripta datos
  - Cambiar�a si: El algoritmo de encriptaci�n cambia

#### Open/Closed Principle (OCP)

**? Abierto para extensi�n, cerrado para modificaci�n**

```csharp
// Ejemplo: Agregar nueva implementaci�n sin modificar c�digo existente

// Sin modificar IUsuarioRepository:
public class SqlUsuarioRepository : IUsuarioRepository
{
    // Nueva implementaci�n con SQL Server
}

// Solo cambiar el registro en DI:
services.AddScoped<IUsuarioRepository, SqlUsuarioRepository>();
```

#### Liskov Substitution Principle (LSP)

**? Las implementaciones son sustituibles por sus abstracciones**

```csharp
// Cualquier IUsuarioRepository puede usarse sin romper el c�digo
IUsuarioRepository repo = new UsuarioRepository(context);
// O
IUsuarioRepository repo = new SqlUsuarioRepository(connectionString);
// O
IUsuarioRepository repo = new MongoUsuarioRepository(config);

// El c�digo que usa IUsuarioRepository funciona con cualquiera
```

#### Interface Segregation Principle (ISP)

**? Interfaces espec�ficas y cohesivas**

```csharp
// ? MALO - Interfaz grande
public interface IUsuarioOperations
{
    Task<Usuario> CrearAsync(Usuario usuario);
    Task<Usuario> ModificarAsync(Usuario usuario);
    Task EliminarAsync(int id);
    string EncriptarClave(string clave);
    bool ValidarEmail(string email);
    // ... m�s m�todos
}

// ? BUENO - Interfaces segregadas
public interface IUsuarioRepository { /* Solo operaciones CRUD */ }
public interface IEncriptacionService { /* Solo encriptaci�n */ }
public interface IValidacionService { /* Solo validaciones */ }
```

#### Dependency Inversion Principle (DIP)

**? M�dulos de alto nivel no dependen de los de bajo nivel**

```csharp
// APP (alto nivel) no depende de REPO (bajo nivel)
// Ambos dependen de IUsuarioRepository (abstracci�n)

// APP
public class UsuarioService
{
    private readonly IUsuarioRepository _repository; // ? Abstracci�n
    
    public UsuarioService(IUsuarioRepository repository)
    {
        _repository = repository;
  }
}

// REPO
public class UsuarioRepository : IUsuarioRepository // ? Implementa abstracci�n
{
    // ...
}
```

### Clean Architecture

#### Regla de Dependencia

> **"El c�digo fuente debe depender solo de cosas que est�n en el mismo nivel o m�s adentro, nunca hacia afuera"**

**? Implementado correctamente:**
- DOM no depende de nadie
- ABS solo depende de DOM
- CONTEXT implementa acceso a SQL Server via IDataAccess
- REPO usa CONTEXT para operaciones de base de datos
- APP depende de ABS y DOM (nunca conoce detalles de persistencia)
- UI puede depender de todos (est� en el c�rculo externo)

#### Independencia de Frameworks

```csharp
// ? APP no sabe nada de Windows Forms ni de SQL Server
// Podr�a usarse en:
// - Aplicaci�n WPF
// - API REST
// - Aplicaci�n de consola
// - Blazor
// Sin modificar APP
// El acceso a datos se puede cambiar (SQL ? MongoDB ? API) sin afectar APP
```

### Clean Code

#### Nombres Descriptivos

```csharp
// ? BIEN
public async Task<ResultadoOperacion<Usuario>> CrearUsuarioAsync(CrearUsuarioDto dto)

// ? MAL
public async Task<Result<User>> Create(CreateDto d)
```

#### M�todos Peque�os

```csharp
// ? Cada m�todo hace una sola cosa
private List<string> ValidarCrearUsuarioDto(CrearUsuarioDto dto)
{
    var errores = new List<string>();
    
    ValidarNombre(dto.Nombre, errores);
    ValidarApellido(dto.Apellido, errores);
    ValidarEmail(dto.Email, errores);
    ValidarClave(dto.Clave, errores);
    ValidarRol(dto.Rol, errores);
    
 return errores;
}
```

### DRY (Don't Repeat Yourself)

**? Validaciones centralizadas**

```csharp
// Una �nica fuente de verdad para validaciones
private List<string> ValidarCrearUsuarioDto(CrearUsuarioDto dto)
{
 // Todas las validaciones aqu�
}

// No hay validaciones duplicadas en:
// - Formulario
// - Controlador
// - Repository
// Solo en el servicio
```

### YAGNI (You Aren't Gonna Need It)

**? Solo lo necesario implementado**

- ? No hay sistema de permisos complicado (no se pidi�)
- ? No hay auditor�a avanzada (no se pidi�)
- ? No hay b�squeda full-text (no se pidi�)
- ? Solo alta de usuario (lo que se pidi�)

---

## ?? Dependency Injection

### Configuraci�n del Contenedor

```csharp
public static class DependencyInjectionContainer
{
    public static void ConfigurarServicios()
    {
        var services = new ServiceCollection();

        // Scoped - Nueva instancia por solicitud/scope
        services.AddScoped<IDataAccess, DalSQLServer>();
        services.AddScoped<IUsuarioDbRepository, RepoUsuario>();
        services.AddScoped<IEncriptacionService, EncriptacionService>();
        services.AddScoped<AppUsuario>();

        // Transient - Nueva instancia cada vez que se solicita
        services.AddTransient<FormPrincipal>();
        services.AddTransient<FormUsuarios>();
        services.AddTransient<FormAltaUsuario>();

        _serviceProvider = services.BuildServiceProvider();
    }
}
```

### Lifetimes Explicados

| Lifetime | Cu�ndo usar | Ejemplo en el proyecto |
|----------|-------------|----------------------|
| **Scoped** | Nueva instancia por operaci�n | Servicios de BD: `DalSQLServer`, `RepoUsuario`, `AppUsuario` |
| **Transient** | Muy ligeros, sin estado | Formularios Windows Forms: `FormUsuarios`, `FormAltaUsuario` |

### Optimizaci�n con C# 11 y Nullable Reference Types

**? Antes (redundante):**
```csharp
public UsuarioService(IUsuarioRepository repository)
{
    _repository = repository ?? throw new ArgumentNullException(nameof(repository));
}
```

**? Ahora (optimizado):**
```csharp
public UsuarioService(IUsuarioRepository repository)
{
    _repository = repository;
}
```

**�Por qu� es seguro?**
1. **NRT (Nullable Reference Types)**: El compilador advierte si pasas null
2. **DI Container**: `GetRequiredService<T>()` lanza excepci�n si no est� registrado
3. **YAGNI**: No agregamos validaciones que nunca se ejecutar�n

**Validaciones que S� mantenemos:**
```csharp
// ? En m�todos p�blicos de API (defensive programming)
public Task<bool> ExisteEmailAsync(string email)
{
  ArgumentException.ThrowIfNullOrWhiteSpace(email);
    return Task.FromResult(_context.ExisteEmail(email));
}
```

---

## ?? Flujo de Datos - Alta de Usuario

### Diagrama de Secuencia

```
Usuario           frmAltaUsuario       UsuarioService       UsuarioRepositoryInMemoryContext
  ?             ?          ?         ?  ?
  ? Completa formulario     ?    ?   ?        ?
  ????????????????????????> ?       ?                  ?   ?
  ??          ?      ?     ?
  ? Clic en Guardar?             ?       ?          ?
  ????????????????????????> ?         ?          ?          ?
  ?        ?        ?          ?           ?
  ?    ? CrearUsuarioAsync  ?           ?             ?
  ?      ????????????????????>?    ?      ?
  ?    ?          ?        ?  ?
  ?             ?   ? Validar DTO          ?       ?
  ?       ?        ????????????     ?  ?
  ?            ?             ?        ?    ?         ?
  ?            ?           ?<??????????           ?       ?
  ?   ?         ?           ?  ?
  ?       ?         ? ExisteEmailAsync     ?         ?
  ?        ?        ??????????????????????>?      ?
  ?          ?             ? ? ExisteEmail      ?
  ? ?    ?       ??????????????????>?
  ?    ?            ?      ?         ?
  ?                 ?   ?               ? false            ?
  ?              ?  ?                 ?<??????????????????
  ? ?         ? false         ?             ?
  ?                ?            ?<??????????????????????          ?
  ?            ? ?     ?        ?
  ?             ?             ? Encriptar(clave)     ?         ?
  ?           ?             ????????????         ?             ?
  ? ?       ?    ?           ?         ?
  ?          ?     ?<??????????           ?              ?
  ? ?    ?             ?      ?
  ?     ?       ? CrearAsync(usuario)  ?     ?
  ?           ? ??????????????????????>?       ?
  ?       ?          ?   ? AgregarUsuario   ?
  ? ?           ?          ??????????????????>?
  ?               ?  ?     ?         ?
  ?             ??         ? Asigna ID++      ?
  ?             ?         ?     ????????????    ?
  ?    ?          ?  ?       ?       ?
  ?         ?       ?       ?<??????????    ?
  ?     ?     ?          ?          ?
  ?     ?   ?        ? Usuario     ?
  ?       ?            ? ?<??????????????????
  ?             ?        ? Usuario          ?       ?
  ?        ?              ?<??????????????????????     ?
  ?  ? ResultadoExitoso   ? ?                  ?
  ?          ?<????????????????????            ?        ?
  ?        ?         ?              ??
  ? MessageBox("�xito")     ?      ?                ?   ?
  ?<?????????????????????????      ?           ?        ?
  ?           ?     ?       ?     ?
```

### Paso a Paso

1. **Usuario completa formulario** (`frmAltaUsuario`)
   - Nombre, Apellido, Email, Clave (11 caracteres), Rol

2. **Usuario hace clic en Guardar**
 - Botones se deshabilitan
   - Se crea `CrearUsuarioDto`

3. **Se llama a `IUsuarioService.CrearUsuarioAsync()`**
   - Se valida el DTO (nombre, apellido, email, clave, rol)
   - Si hay errores, retorna `ResultadoOperacion<Usuario>.Error()`

4. **Se verifica email �nico**
   - Llama a `IUsuarioRepository.ExisteEmailAsync()`
   - Si existe, retorna error

5. **Se valida longitud de clave**
   - Debe ser exactamente 11 caracteres
   - Si no, retorna error

6. **Se encripta la clave**
   - Llama a `IEncriptacionService.Encriptar()`
   - SHA256 hash

7. **Se crea entidad `Usuario`**
   - Con todos los datos validados
- Estado = Activo
   - FechaCreacion = DateTime.Now

8. **Se guarda en repositorio**
   - Llama a `IUsuarioRepository.CrearAsync()`
   - El repositorio llama a `InMemoryContext.AgregarUsuario()`
   - El contexto asigna ID autom�ticamente

9. **Se retorna resultado exitoso**
   - `ResultadoOperacion<Usuario>.Exito()`
   - Con el usuario creado

10. **UI muestra mensaje**
    - MessageBox con "Usuario creado exitosamente"
    - Cierra formulario
    - Recarga lista de usuarios

---

## ? Validaciones

### Niveles de Validaci�n

#### 1. UI (Prevenci�n)

```csharp
// frmAltaUsuario.Designer.cs
txtNombre.MaxLength = 50;
txtApellido.MaxLength = 80;
txtEmail.MaxLength = 180;
txtClave.MaxLength = 11;
txtClave.UseSystemPasswordChar = true;

numRol.Minimum = 0;
numRol.Maximum = 9;
```

**Objetivo:** Prevenir entrada inv�lida antes de que llegue al servicio

#### 2. Servicio (Reglas de Negocio)

```csharp
// APP/UsuarioService.cs
private List<string> ValidarCrearUsuarioDto(CrearUsuarioDto dto)
{
    var errores = new List<string>();

    // Nombre
    if (string.IsNullOrWhiteSpace(dto.Nombre))
    errores.Add("El nombre es obligatorio");
    else if (dto.Nombre.Length > 50)
        errores.Add("El nombre no puede superar los 50 caracteres");

    // Apellido
    if (string.IsNullOrWhiteSpace(dto.Apellido))
        errores.Add("El apellido es obligatorio");
    else if (dto.Apellido.Length > 80)
        errores.Add("El apellido no puede superar los 80 caracteres");

  // Email
    if (string.IsNullOrWhiteSpace(dto.Email))
     errores.Add("El email es obligatorio");
    else if (dto.Email.Length > 180)
        errores.Add("El email no puede superar los 180 caracteres");
    else if (!EsEmailValido(dto.Email))
        errores.Add("El formato del email no es v�lido");

  // Clave
    if (string.IsNullOrWhiteSpace(dto.Clave))
        errores.Add("La clave es obligatoria");

    // Rol
    if (dto.Rol < 0 || dto.Rol > 9)
        errores.Add("El rol debe estar entre 0 y 9");

    return errores;
}

private bool EsEmailValido(string email)
{
    return new EmailAddressAttribute().IsValid(email);
}
```

**Objetivo:** Asegurar reglas de negocio, independiente de la UI

#### 3. Repositorio (Integridad de Datos)

```csharp
// REPO/UsuarioRepository.cs
public Task<Usuario> CrearAsync(Usuario usuario)
{
    ArgumentNullException.ThrowIfNull(usuario);
    
    _context.AgregarUsuario(usuario);
    return Task.FromResult(usuario);
}

public Task<bool> ExisteEmailAsync(string email)
{
    ArgumentException.ThrowIfNullOrWhiteSpace(email);
    
    var existe = _context.ExisteEmail(email);
    return Task.FromResult(existe);
}
```

**Objetivo:** Defensive programming en fronteras de APIs p�blicas

### Matriz de Validaciones

| Campo | UI | Servicio | Repositorio | Error si Falla |
|-------|:--:|:--------:|:-----------:|----------------|
| Nombre obligatorio | - | ? | - | "El nombre es obligatorio" |
| Nombre ? 50 | ? | ? | - | "El nombre no puede superar los 50 caracteres" |
| Apellido obligatorio | - | ? | - | "El apellido es obligatorio" |
| Apellido ? 80 | ? | ? | - | "El apellido no puede superar los 80 caracteres" |
| Email obligatorio | - | ? | - | "El email es obligatorio" |
| Email formato v�lido | - | ? | - | "El formato del email no es v�lido" |
| Email ? 180 | ? | ? | - | "El email no puede superar los 180 caracteres" |
| Email �nico | - | ? | ? | "El email ya est� registrado" |
| Clave obligatoria | - | ? | - | "La clave es obligatoria" |
| Clave = 11 caracteres | ? | ? | - | "La clave debe tener exactamente 11 caracteres" |
| Rol 0-9 | ? | ? | - | "El rol debe estar entre 0 y 9" |

---

## ?? Seguridad

### Encriptaci�n de Claves

**Algoritmo:** SHA256 (Secure Hash Algorithm 256-bit)

```csharp
public class EncriptacionService : IEncriptacionService
{
  public string Encriptar(string textoPlano)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(textoPlano);

        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(textoPlano);
      var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}
```

**Caracter�sticas:**
- ? Hash unidireccional (no reversible)
- ? 256 bits de longitud
- ? Determinista (misma entrada = mismo hash)
- ? Resistente a colisiones
- ?? Sin salt (para simplificar en contexto educativo)

**Ejemplo:**
```
Entrada: "MiClave1234"
Salida:  "5nY8xR7vK3mP9qW2dF6hL1tG4jN8uB3xE7cA5zS2mK9="
```

### Integraci�n con SQL Server

**Arquitectura de Persistencia**

```csharp
// CONTEXT/DalSQLServer.cs - Manejo de conexi�n
public class DalSQLServer : IDataAccess
{
    private readonly SqlConnection con;
    
    public SqlConnection AbrirConexion()
    {
        con.ConnectionString = StringConexion();
        con.Open();
        return con;
    }
    
    public void CerrarConexion() => con.Close();
    
    public SqlDataReader EjecutarSQL(SqlCommand cmd) => cmd.ExecuteReader();
}

// REPO/RepoUsuario.cs - Operaciones de base de datos
public class RepoUsuario : IUsuarioDbRepository
{
    private readonly IDataAccess _dataAccess;
    
    public List<DomUsuario> Traer()
    {
        try
        {
            using SqlCommand cmd = new();
            cmd.CommandText = "SELECT * FROM Usuario";
            cmd.Connection = _dataAccess.AbrirConexion();
            
            using SqlDataReader dr = _dataAccess.EjecutarSQL(cmd);
            return CompletarLista(dr, new List<DomUsuario>());
        }
        finally
        {
            _dataAccess.CerrarConexion();  // ? Siempre cierra la conexi�n
        }
    }
}
```

**Caracter�sticas:**
- ? Try-finally garantiza cierre de conexi�n
- ? Using statements para recursos desechables
- ? IDs gestionados por SQL Server (IDENTITY)
- ? Integridad referencial con Foreign Keys

### Nota para Producci�n

?? **Para sistemas de producci�n, se recomienda:**

```csharp
// Usar BCrypt o Argon2 en lugar de SHA256
using BC = BCrypt.Net.BCrypt;

public string EncriptarProduccion(string textoPlano)
{
    // BCrypt genera salt autom�ticamente
    return BC.HashPassword(textoPlano, BC.GenerateSalt(12));
}

public bool VerificarProduccion(string textoPlano, string hash)
{
    return BC.Verify(textoPlano, hash);
}
```

---

## ?? Casos de Prueba

### Casos de Prueba Positivos (? Deben Funcionar)

#### CP01: Crear Usuario V�lido B�sico

**Datos de entrada:**
```
Nombre: Juan
Apellido: P�rez
Email: juan.perez@ejemplo.com
Clave: Password123 (11 caracteres)
Rol: 1
```

**Resultado esperado:**
- ? Usuario creado exitosamente
- ? Aparece en lista con ID = 1
- ? Estado = Activo
- ? Clave encriptada (hash largo)
- ? FechaCreacion = fecha/hora actual

---

#### CP02: Crear Usuario con Nombres Largos

**Datos de entrada:**
```
Nombre: ABCDEFGHIJKLMNOPQRSTUVWXYZ123456789012345678 (50 chars)
Apellido: ABCDEFGHIJKLMNOPQRSTUVWXYZ12345678901234567890123456789012345678901234567890 (80 chars)
Email: email@test.com
Clave: 12345678901
Rol: 5
```

**Resultado esperado:**
- ? Usuario creado exitosamente
- ? Nombres completos sin truncar

---

#### CP03: Crear M�ltiples Usuarios

**Procedimiento:**
1. Crear usuario1@test.com
2. Crear usuario2@test.com
3. Crear usuario3@test.com

**Resultado esperado:**
- ? 3 usuarios creados
- ? IDs �nicos: 1, 2, 3
- ? Todos en la lista

---

#### CP04: Emails con Formatos V�lidos

**Probar estos emails (uno por uno):**
```
? simple@ejemplo.com
? nombre.apellido@dominio.com.ar
? usuario+etiqueta@ejemplo.org
? 123@456.com
? test_user@mi-dominio.net
? user@sub.domain.example.com
```

**Resultado esperado:**
- ? Todos son aceptados

---

#### CP05: Todos los Roles (0-9)

**Procedimiento:**
Crear 10 usuarios con roles del 0 al 9

**Resultado esperado:**
- ? Todos los roles son v�lidos
- ? 10 usuarios creados

---

### Casos de Prueba Negativos (? Deben Fallar con Error Claro)

#### CN01: Email Duplicado

**Procedimiento:**
1. Crear usuario con email: duplicado@test.com ? ? �xito
2. Intentar crear otro con email: duplicado@test.com ? ? Debe fallar

**Resultado esperado:**
```
? Error: "El email ya est� registrado"
? No se crea el segundo usuario
```

---

#### CN02: Campos Vac�os

**CN02.A - Sin Nombre:**
```
Nombre: [vac�o]
Apellido: Gonz�lez
Email: test@test.com
Clave: 12345678901
Rol: 1
```
**Error esperado:** `"El nombre es obligatorio"`

**CN02.B - Sin Apellido:**
```
Nombre: Juan
Apellido: [vac�o]
Email: test@test.com
Clave: 12345678901
Rol: 1
```
**Error esperado:** `"El apellido es obligatorio"`

**CN02.C - Sin Email:**
```
Nombre: Juan
Apellido: Gonz�lez
Email: [vac�o]
Clave: 12345678901
Rol: 1
```
**Error esperado:** `"El email es obligatorio"`

**CN02.D - Sin Clave:**
```
Nombre: Juan
Apellido: Gonz�lez
Email: test@test.com
Clave: [vac�o]
Rol: 1
```
**Error esperado:** `"La clave es obligatoria"`

---

#### CN03: Email con Formato Inv�lido

**Probar estos emails (deben fallar):**
```
? sinArroba.com
? @sinusuario.com
? usuario@
? usuario @espacio.com
? usuario..doble@punto.com
? usuario
? @@@
? usuario@dominio
```

**Error esperado:** `"El formato del email no es v�lido"`

---

#### CN04: Clave con Longitud Incorrecta

**CN04.A - Clave con 10 caracteres:**
```
Clave: 1234567890
```
**Error esperado:** `"La clave debe tener exactamente 11 caracteres"`

**CN04.B - Clave con 12 caracteres:**
```
Clave: 123456789012
```
**Error esperado:** `"La clave debe tener exactamente 11 caracteres"`

**CN04.C - Clave con 1 car�cter:**
```
Clave: 1
```
**Error esperado:** `"La clave debe tener exactamente 11 caracteres"`

**CN04.D - Clave con 0 caracteres:**
```
Clave: [vac�o]
```
**Error esperado:** `"La clave es obligatoria"`

---

#### CN05: Nombres/Apellidos Excediendo L�mites

**Nota:** El TextBox con MaxLength previene esto, pero el servicio tambi�n valida

**CN05.A - Nombre > 50 caracteres (51):**
Si de alguna forma se env�a un nombre de 51+ caracteres

**Error esperado:** `"El nombre no puede superar los 50 caracteres"`

**CN05.B - Apellido > 80 caracteres (81):**
Si de alguna forma se env�a un apellido de 81+ caracteres

**Error esperado:** `"El apellido no puede superar los 80 caracteres"`

---

### Casos de Prueba de Integraci�n

#### CI01: Flujo Completo de Alta

**Procedimiento:**
1. Iniciar aplicaci�n
2. Verificar que formulario principal se abre maximizado
3. Men�: Administraci�n > Usuarios
4. Verificar que lista est� vac�a
5. Clic en "Nuevo Usuario"
6. Completar datos v�lidos
7. Guardar
8. Verificar mensaje de �xito
9. Verificar que usuario aparece en lista con ID=1
10. Crear segundo usuario
11. Verificar que aparece con ID=2
12. Refrescar lista
13. Verificar que ambos siguen ah�

**Resultado esperado:**
- ? Todo el flujo funciona sin errores
- ? Datos persisten en memoria

---

#### CI02: Prevenci�n de Ventanas Duplicadas

**Procedimiento:**
1. Men�: Administraci�n > Usuarios (abre ventana A)
2. Intentar abrir de nuevo: Administraci�n > Usuarios

**Resultado esperado:**
- ? No se abre segunda ventana
- ? La ventana A se trae al frente (Activate)

---

#### CI03: Cancelar Creaci�n

**Procedimiento:**
1. Clic en "Nuevo Usuario"
2. Completar algunos campos
3. Clic en "Cancelar"

**Resultado esperado:**
- ? Formulario se cierra sin guardar
- ? No se crea usuario
- ? Lista permanece sin cambios

---

#### CI04: Verificaci�n de Encriptaci�n

**Procedimiento:**
1. Crear usuario con clave: "MiClave1234"
2. Crear otro usuario con la misma clave: "MiClave1234"
3. En depurador, inspeccionar `usuario.ClaveEncriptada` de ambos

**Resultado esperado:**
- ? Ambos tienen el mismo hash
- ? El hash NO es "MiClave1234"
- ? El hash es una cadena Base64 larga (ej: "5nY8xR7vK3mP9qW2dF6hL1...")

---

### Matriz de Casos de Prueba

| ID | Descripci�n | Tipo | Prioridad | Estado |
|----|-------------|------|-----------|--------|
| CP01 | Usuario v�lido b�sico | Positivo | Alta | ? |
| CP02 | Nombres largos | Positivo | Media | ? |
| CP03 | M�ltiples usuarios | Positivo | Alta | ? |
| CP04 | Formatos de email | Positivo | Media | ? |
| CP05 | Todos los roles | Positivo | Baja | ? |
| CN01 | Email duplicado | Negativo | Alta | ? |
| CN02 | Campos vac�os | Negativo | Alta | ? |
| CN03 | Email inv�lido | Negativo | Alta | ? |
| CN04 | Clave longitud incorrecta | Negativo | Alta | ? |
| CN05 | Exceder l�mites | Negativo | Media | ? |
| CI01 | Flujo completo | Integraci�n | Alta | ? |
| CI02 | Ventanas duplicadas | Integraci�n | Media | ? |
| CI03 | Cancelar operaci�n | Integraci�n | Media | ? |
| CI04 | Encriptaci�n | Integraci�n | Alta | ? |

---

## ?? Escalabilidad

### C�mo Extender el Sistema

#### 1. Agregar Base de Datos Real

**Paso 1:** Crear nueva implementaci�n del repositorio

```csharp
// REPO/SqlUsuarioRepository.cs
public class SqlUsuarioRepository : IUsuarioRepository
{
    private readonly string _connectionString;

    public SqlUsuarioRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<Usuario> CrearAsync(Usuario usuario)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        
        const string sql = @"
   INSERT INTO Usuarios (Nombre, Apellido, Email, ClaveEncriptada, Estado, Rol, FechaCreacion, UsuarioCreacion)
 VALUES (@Nombre, @Apellido, @Email, @ClaveEncriptada, @Estado, @Rol, @FechaCreacion, @UsuarioCreacion);
      SELECT CAST(SCOPE_IDENTITY() as int);";
        
        usuario.Id = await connection.ExecuteScalarAsync<int>(sql, usuario);
        return usuario;
    }

    // ... otros m�todos
}
```

**Paso 2:** Cambiar registro en DI

```csharp
// Antes
services.AddSingleton<InMemoryContext>();
services.AddScoped<IUsuarioRepository, UsuarioRepository>();

// Despu�s
services.AddScoped<IUsuarioRepository>(sp => 
    new SqlUsuarioRepository("Server=...;Database=..."));
```

**�Listo!** No se modifica ninguna otra capa.

---

#### 2. Agregar Modificaci�n de Usuario

**Paso 1:** Agregar m�todo a interfaz

```csharp
// ABS/Application/IUsuarioService.cs
public interface IUsuarioService
{
    // ...existing methods...
    Task<ResultadoOperacion<Usuario>> ModificarUsuarioAsync(ModificarUsuarioDto dto);
}
```

**Paso 2:** Implementar en servicio

```csharp
// APP/UsuarioService.cs
public async Task<ResultadoOperacion<Usuario>> ModificarUsuarioAsync(ModificarUsuarioDto dto)
{
    // Validaciones
    // Obtener usuario existente
    // Actualizar campos
    // Guardar
    // Retornar resultado
}
```

**Paso 3:** Agregar UI

```csharp
// PeluqueriaSystem/frmModificarUsuario.cs
// Similar a frmAltaUsuario pero pre-poblado
```

---

#### 3. Agregar Tests Unitarios

**Paso 1:** Crear proyecto de tests

```bash
dotnet new xunit -n PeluqueriaSystem.Tests
dotnet add PeluqueriaSystem.Tests package Moq
```

**Paso 2:** Escribir tests

```csharp
[Fact]
public async Task CrearUsuario_ConDatosValidos_DebeRetornarExito()
{
    // Arrange
var mockRepo = new Mock<IUsuarioRepository>();
    var mockEncriptacion = new Mock<IEncriptacionService>();
    mockRepo.Setup(r => r.ExisteEmailAsync(It.IsAny<string>()))
   .ReturnsAsync(false);
    mockEncriptacion.Setup(e => e.Encriptar(It.IsAny<string>()))
           .Returns("hash_encriptado");
 
    var service = new UsuarioService(mockRepo.Object, mockEncriptacion.Object);
    
  var dto = new CrearUsuarioDto
    {
        Nombre = "Juan",
        Apellido = "P�rez",
        Email = "juan@test.com",
 Clave = "12345678901",
        Rol = 1,
        UsuarioCreacion = "SISTEMA"
    };
    
    // Act
    var resultado = await service.CrearUsuarioAsync(dto);
    
// Assert
    Assert.True(resultado.Exitoso);
Assert.Equal("Usuario creado exitosamente", resultado.Mensaje);
    mockRepo.Verify(r => r.CrearAsync(It.IsAny<Usuario>()), Times.Once);
}
```

---

### Patr�n para Nuevas Entidades

**Ejemplo: Agregar gesti�n de Citas**

```
1. DOM/Cita.cs (entidad)
2. ABS/Repositories/ICitaRepository.cs (interfaz)
3. ABS/Application/ICitaService.cs (interfaz servicio)
4. REPO/CitaRepository.cs (implementaci�n repositorio)
5. APP/CitaService.cs (implementaci�n servicio)
6. PeluqueriaSystem/frmCitas.cs (UI listado)
7. PeluqueriaSystem/frmAltaCita.cs (UI alta)
8. Registrar en DI
```

**Seguir el mismo patr�n de Usuario ? Garantiza consistencia**

---

## ? Checklist de Calidad

### Antes de Entregar/Commitear

#### Compilaci�n
- [ ] `dotnet build` sin errores
- [ ] `dotnet build` sin warnings
- [ ] Todas las referencias de proyecto correctas

#### C�digo
- [ ] Sin c�digo comentado
- [ ] Sin TODOs pendientes cr�ticos
- [ ] Sin console.WriteLine() de depuraci�n
- [ ] Nombres de variables descriptivos
- [ ] M�todos peque�os (<20 l�neas idealmente)

#### Arquitectura
- [ ] Flujo de dependencias correcto
- [ ] UI no llama directamente a repositorio
- [ ] APP no depende de Windows Forms
- [ ] Todas las dependencias mediante interfaces

#### Funcionalidad
- [ ] Crear usuario con datos v�lidos funciona
- [ ] Validaciones funcionan correctamente
- [ ] Email duplicado es detectado
- [ ] Clave de 11 caracteres es validada
- [ ] Lista se actualiza despu�s de crear

#### Testing
- [ ] Al menos los casos positivos b�sicos probados manualmente
- [ ] Al menos un caso negativo probado (email duplicado)
- [ ] Flujo completo funciona

#### Documentaci�n
- [ ] README.md actualizado
- [ ] Comentarios XML en clases p�blicas
- [ ] DEVELOPMENT.md refleja arquitectura actual

---

## ?? Referencias y Recursos

### Libros Recomendados
- **Clean Architecture** - Robert C. Martin
- **Clean Code** - Robert C. Martin
- **Design Patterns** - Gang of Four
- **Domain-Driven Design** - Eric Evans

### Enlaces �tiles
- [SOLID Principles](https://en.wikipedia.org/wiki/SOLID)
- [Clean Architecture (art�culo)](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Dependency Injection en .NET](https://docs.microsoft.com/en-us/dotnet/core/extensions/dependency-injection)
- [C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)

---

## ?? Conclusi�n

Este proyecto es un ejemplo completo de:
- ? Arquitectura limpia y escalable
- ? C�digo profesional y mantenible
- ? Aplicaci�n pr�ctica de principios SOLID
- ? Uso correcto de Dependency Injection
- ? Balance entre teor�a y pragmatismo

**Puede servir como plantilla para futuros proyectos Windows Forms con Clean Architecture.**

---

**�ltima actualizaci�n:** $(date)  
**Mantenedor:** Sistema de Gesti�n Peluquer�a  
**Versi�n:** 1.0
