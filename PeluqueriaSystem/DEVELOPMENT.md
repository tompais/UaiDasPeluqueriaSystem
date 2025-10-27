# Guía de Desarrollo - Sistema de Peluquería

Esta guía contiene información técnica detallada sobre la arquitectura, implementación y casos de prueba del sistema.

---

## ?? Tabla de Contenidos

1. [Arquitectura Detallada](#arquitectura-detallada)
2. [Principios de Diseño](#principios-de-diseño)
3. [Implementación por Capas](#implementación-por-capas)
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
?  - frmPrincipal (MDI Container)        ?
?  - frmUsuarios (Listado)           ?
?  - frmAltaUsuario (Alta)       ?
?  - DependencyInjectionContainer  ?
???????????????????????????????????????????????????????????????
           ? Usa
   ?
???????????????????????????????????????????????????????????????
?     APP (Lógica de Negocio)        ?
?  - UsuarioService              ?
?  - CrearUsuarioDto       ?
?  - ResultadoOperacion<T>   ?
???????????????????????????????????????????????????????????????
     ? Usa
 ?????????????????????????????????
         ?              ?
??????????????????????????????????????????????????
?   REPO (Repositorio)  ?    ?   SERV (Servicios)     ?
? - UsuarioRepository  ? ? - EncriptacionService  ?
????????????????????????    ??????????????????????????
        ? Usa
   ?
???????????????????????????
? CONTEXT (Datos Memoria) ?
?  - InMemoryContext   ?
???????????????????????????
           ? Usa
      ?
???????????????????????????
?    DOM (Dominio)        ?
?  - Usuario        ?
?  - EstadoUsuario        ?
???????????????????????????
        ?
      ? Todas las capas dependen de
???????????????????????????
?  ABS (Abstracciones) ?
?  - IUsuarioRepository   ?
?  - IUsuarioService      ?
?  - IEncriptacionService ?
???????????????????????????
```

### Proyectos y Responsabilidades

| Proyecto | Tipo | Responsabilidad | Dependencias |
|----------|------|----------------|--------------|
| **DOM** | Class Library | Entidades del dominio | Ninguna |
| **ABS** | Class Library | Interfaces y abstracciones | DOM |
| **SERV** | Class Library | Servicios auxiliares (encriptación) | ABS |
| **CONTEXT** | Class Library | Contexto en memoria | DOM |
| **REPO** | Class Library | Repositorio CRUD | ABS, CONTEXT, DOM |
| **APP** | Class Library | Lógica de negocio | ABS, DOM |
| **PeluqueriaSystem** | WinForms App | Interfaz de usuario | Todos |

### Flujo de Dependencias

```
Regla fundamental: Las dependencias apuntan hacia adentro

Externo ? Interno:
UI ? APP ? REPO/SERV ? CONTEXT/DOM

Todos dependen de ABS (abstracciones)
```

---

## ?? Principios de Diseño

### SOLID - Análisis Detallado

#### Single Responsibility Principle (SRP)

**? Cada clase tiene una única razón para cambiar**

- `Usuario`: Solo representa la entidad del dominio
  - Cambiaría si: Los atributos del usuario cambian
  
- `UsuarioService`: Solo contiene lógica de negocio
  - Cambiaría si: Las reglas de negocio cambian
  
- `UsuarioRepository`: Solo maneja persistencia
  - Cambiaría si: La forma de almacenar datos cambia
  
- `EncriptacionService`: Solo encripta datos
  - Cambiaría si: El algoritmo de encriptación cambia

#### Open/Closed Principle (OCP)

**? Abierto para extensión, cerrado para modificación**

```csharp
// Ejemplo: Agregar nueva implementación sin modificar código existente

// Sin modificar IUsuarioRepository:
public class SqlUsuarioRepository : IUsuarioRepository
{
    // Nueva implementación con SQL Server
}

// Solo cambiar el registro en DI:
services.AddScoped<IUsuarioRepository, SqlUsuarioRepository>();
```

#### Liskov Substitution Principle (LSP)

**? Las implementaciones son sustituibles por sus abstracciones**

```csharp
// Cualquier IUsuarioRepository puede usarse sin romper el código
IUsuarioRepository repo = new UsuarioRepository(context);
// O
IUsuarioRepository repo = new SqlUsuarioRepository(connectionString);
// O
IUsuarioRepository repo = new MongoUsuarioRepository(config);

// El código que usa IUsuarioRepository funciona con cualquiera
```

#### Interface Segregation Principle (ISP)

**? Interfaces específicas y cohesivas**

```csharp
// ? MALO - Interfaz grande
public interface IUsuarioOperations
{
    Task<Usuario> CrearAsync(Usuario usuario);
    Task<Usuario> ModificarAsync(Usuario usuario);
    Task EliminarAsync(int id);
    string EncriptarClave(string clave);
    bool ValidarEmail(string email);
    // ... más métodos
}

// ? BUENO - Interfaces segregadas
public interface IUsuarioRepository { /* Solo operaciones CRUD */ }
public interface IEncriptacionService { /* Solo encriptación */ }
public interface IValidacionService { /* Solo validaciones */ }
```

#### Dependency Inversion Principle (DIP)

**? Módulos de alto nivel no dependen de los de bajo nivel**

```csharp
// APP (alto nivel) no depende de REPO (bajo nivel)
// Ambos dependen de IUsuarioRepository (abstracción)

// APP
public class UsuarioService
{
    private readonly IUsuarioRepository _repository; // ? Abstracción
    
    public UsuarioService(IUsuarioRepository repository)
    {
        _repository = repository;
  }
}

// REPO
public class UsuarioRepository : IUsuarioRepository // ? Implementa abstracción
{
    // ...
}
```

### Clean Architecture

#### Regla de Dependencia

> **"El código fuente debe depender solo de cosas que estén en el mismo nivel o más adentro, nunca hacia afuera"**

**? Implementado correctamente:**
- DOM no depende de nadie
- ABS solo depende de DOM
- APP depende de ABS y DOM (nunca de REPO o UI)
- UI puede depender de todos (está en el círculo externo)

#### Independencia de Frameworks

```csharp
// ? APP no sabe nada de Windows Forms
// Podría usarse en:
// - Aplicación WPF
// - API REST
// - Aplicación de consola
// - Blazor
// Sin modificar APP
```

### Clean Code

#### Nombres Descriptivos

```csharp
// ? BIEN
public async Task<ResultadoOperacion<Usuario>> CrearUsuarioAsync(CrearUsuarioDto dto)

// ? MAL
public async Task<Result<User>> Create(CreateDto d)
```

#### Métodos Pequeños

```csharp
// ? Cada método hace una sola cosa
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
// Una única fuente de verdad para validaciones
private List<string> ValidarCrearUsuarioDto(CrearUsuarioDto dto)
{
 // Todas las validaciones aquí
}

// No hay validaciones duplicadas en:
// - Formulario
// - Controlador
// - Repository
// Solo en el servicio
```

### YAGNI (You Aren't Gonna Need It)

**? Solo lo necesario implementado**

- ? No hay sistema de permisos complicado (no se pidió)
- ? No hay auditoría avanzada (no se pidió)
- ? No hay búsqueda full-text (no se pidió)
- ? Solo alta de usuario (lo que se pidió)

---

## ?? Dependency Injection

### Configuración del Contenedor

```csharp
public static class DependencyInjectionContainer
{
    public static void ConfigurarServicios()
    {
        var services = new ServiceCollection();

        // Singleton - Una instancia durante toda la aplicación
     services.AddSingleton<InMemoryContext>();

      // Scoped - Nueva instancia por solicitud/scope
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
  services.AddScoped<IEncriptacionService, EncriptacionService>();
services.AddScoped<IUsuarioService, UsuarioService>();

        // Transient - Nueva instancia cada vez que se solicita
        services.AddTransient<frmPrincipal>();
     services.AddTransient<frmUsuarios>();
        services.AddTransient<frmAltaUsuario>();

        _serviceProvider = services.BuildServiceProvider();
    }
}
```

### Lifetimes Explicados

| Lifetime | Cuándo usar | Ejemplo en el proyecto |
|----------|-------------|----------------------|
| **Singleton** | Estado compartido durante toda la app | `InMemoryContext` - Mantiene usuarios en memoria |
| **Scoped** | Nueva instancia por operación | Servicios sin estado: `UsuarioService`, `UsuarioRepository` |
| **Transient** | Muy ligeros, sin estado | Formularios Windows Forms |

### Optimización con C# 11 y Nullable Reference Types

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

**¿Por qué es seguro?**
1. **NRT (Nullable Reference Types)**: El compilador advierte si pasas null
2. **DI Container**: `GetRequiredService<T>()` lanza excepción si no está registrado
3. **YAGNI**: No agregamos validaciones que nunca se ejecutarán

**Validaciones que SÍ mantenemos:**
```csharp
// ? En métodos públicos de API (defensive programming)
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
  ? MessageBox("Éxito")     ?      ?                ?   ?
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

4. **Se verifica email único**
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
   - El contexto asigna ID automáticamente

9. **Se retorna resultado exitoso**
   - `ResultadoOperacion<Usuario>.Exito()`
   - Con el usuario creado

10. **UI muestra mensaje**
    - MessageBox con "Usuario creado exitosamente"
    - Cierra formulario
    - Recarga lista de usuarios

---

## ? Validaciones

### Niveles de Validación

#### 1. UI (Prevención)

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

**Objetivo:** Prevenir entrada inválida antes de que llegue al servicio

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
        errores.Add("El formato del email no es válido");

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

**Objetivo:** Defensive programming en fronteras de APIs públicas

### Matriz de Validaciones

| Campo | UI | Servicio | Repositorio | Error si Falla |
|-------|:--:|:--------:|:-----------:|----------------|
| Nombre obligatorio | - | ? | - | "El nombre es obligatorio" |
| Nombre ? 50 | ? | ? | - | "El nombre no puede superar los 50 caracteres" |
| Apellido obligatorio | - | ? | - | "El apellido es obligatorio" |
| Apellido ? 80 | ? | ? | - | "El apellido no puede superar los 80 caracteres" |
| Email obligatorio | - | ? | - | "El email es obligatorio" |
| Email formato válido | - | ? | - | "El formato del email no es válido" |
| Email ? 180 | ? | ? | - | "El email no puede superar los 180 caracteres" |
| Email único | - | ? | ? | "El email ya está registrado" |
| Clave obligatoria | - | ? | - | "La clave es obligatoria" |
| Clave = 11 caracteres | ? | ? | - | "La clave debe tener exactamente 11 caracteres" |
| Rol 0-9 | ? | ? | - | "El rol debe estar entre 0 y 9" |

---

## ?? Seguridad

### Encriptación de Claves

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

**Características:**
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

### Thread Safety

**Problema:** Múltiples threads accediendo al contexto simultáneamente

**Solución:** Lock en todas las operaciones

```csharp
public class InMemoryContext
{
    private readonly List<Usuario> _usuarios = new();
    private int _siguienteId = 1;
    private readonly object _lock = new();

    public void AgregarUsuario(Usuario usuario)
    {
   lock (_lock)  // ? Protección contra race conditions
        {
            usuario.Id = _siguienteId++;
            _usuarios.Add(usuario);
        }
    }

    public Usuario? ObtenerUsuarioPorEmail(string email)
    {
        lock (_lock)  // ? Protección en lectura también
        {
         return _usuarios.FirstOrDefault(u => 
         u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }
    }
}
```

**Garantías:**
- ? Un solo thread a la vez en sección crítica
- ? IDs únicos garantizados
- ? Sin race conditions

### Nota para Producción

?? **Para sistemas de producción, se recomienda:**

```csharp
// Usar BCrypt o Argon2 en lugar de SHA256
using BC = BCrypt.Net.BCrypt;

public string EncriptarProduccion(string textoPlano)
{
    // BCrypt genera salt automáticamente
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

#### CP01: Crear Usuario Válido Básico

**Datos de entrada:**
```
Nombre: Juan
Apellido: Pérez
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

#### CP03: Crear Múltiples Usuarios

**Procedimiento:**
1. Crear usuario1@test.com
2. Crear usuario2@test.com
3. Crear usuario3@test.com

**Resultado esperado:**
- ? 3 usuarios creados
- ? IDs únicos: 1, 2, 3
- ? Todos en la lista

---

#### CP04: Emails con Formatos Válidos

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
- ? Todos los roles son válidos
- ? 10 usuarios creados

---

### Casos de Prueba Negativos (? Deben Fallar con Error Claro)

#### CN01: Email Duplicado

**Procedimiento:**
1. Crear usuario con email: duplicado@test.com ? ? Éxito
2. Intentar crear otro con email: duplicado@test.com ? ? Debe fallar

**Resultado esperado:**
```
? Error: "El email ya está registrado"
? No se crea el segundo usuario
```

---

#### CN02: Campos Vacíos

**CN02.A - Sin Nombre:**
```
Nombre: [vacío]
Apellido: González
Email: test@test.com
Clave: 12345678901
Rol: 1
```
**Error esperado:** `"El nombre es obligatorio"`

**CN02.B - Sin Apellido:**
```
Nombre: Juan
Apellido: [vacío]
Email: test@test.com
Clave: 12345678901
Rol: 1
```
**Error esperado:** `"El apellido es obligatorio"`

**CN02.C - Sin Email:**
```
Nombre: Juan
Apellido: González
Email: [vacío]
Clave: 12345678901
Rol: 1
```
**Error esperado:** `"El email es obligatorio"`

**CN02.D - Sin Clave:**
```
Nombre: Juan
Apellido: González
Email: test@test.com
Clave: [vacío]
Rol: 1
```
**Error esperado:** `"La clave es obligatoria"`

---

#### CN03: Email con Formato Inválido

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

**Error esperado:** `"El formato del email no es válido"`

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

**CN04.C - Clave con 1 carácter:**
```
Clave: 1
```
**Error esperado:** `"La clave debe tener exactamente 11 caracteres"`

**CN04.D - Clave con 0 caracteres:**
```
Clave: [vacío]
```
**Error esperado:** `"La clave es obligatoria"`

---

#### CN05: Nombres/Apellidos Excediendo Límites

**Nota:** El TextBox con MaxLength previene esto, pero el servicio también valida

**CN05.A - Nombre > 50 caracteres (51):**
Si de alguna forma se envía un nombre de 51+ caracteres

**Error esperado:** `"El nombre no puede superar los 50 caracteres"`

**CN05.B - Apellido > 80 caracteres (81):**
Si de alguna forma se envía un apellido de 81+ caracteres

**Error esperado:** `"El apellido no puede superar los 80 caracteres"`

---

### Casos de Prueba de Integración

#### CI01: Flujo Completo de Alta

**Procedimiento:**
1. Iniciar aplicación
2. Verificar que formulario principal se abre maximizado
3. Menú: Administración > Usuarios
4. Verificar que lista está vacía
5. Clic en "Nuevo Usuario"
6. Completar datos válidos
7. Guardar
8. Verificar mensaje de éxito
9. Verificar que usuario aparece en lista con ID=1
10. Crear segundo usuario
11. Verificar que aparece con ID=2
12. Refrescar lista
13. Verificar que ambos siguen ahí

**Resultado esperado:**
- ? Todo el flujo funciona sin errores
- ? Datos persisten en memoria

---

#### CI02: Prevención de Ventanas Duplicadas

**Procedimiento:**
1. Menú: Administración > Usuarios (abre ventana A)
2. Intentar abrir de nuevo: Administración > Usuarios

**Resultado esperado:**
- ? No se abre segunda ventana
- ? La ventana A se trae al frente (Activate)

---

#### CI03: Cancelar Creación

**Procedimiento:**
1. Clic en "Nuevo Usuario"
2. Completar algunos campos
3. Clic en "Cancelar"

**Resultado esperado:**
- ? Formulario se cierra sin guardar
- ? No se crea usuario
- ? Lista permanece sin cambios

---

#### CI04: Verificación de Encriptación

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

| ID | Descripción | Tipo | Prioridad | Estado |
|----|-------------|------|-----------|--------|
| CP01 | Usuario válido básico | Positivo | Alta | ? |
| CP02 | Nombres largos | Positivo | Media | ? |
| CP03 | Múltiples usuarios | Positivo | Alta | ? |
| CP04 | Formatos de email | Positivo | Media | ? |
| CP05 | Todos los roles | Positivo | Baja | ? |
| CN01 | Email duplicado | Negativo | Alta | ? |
| CN02 | Campos vacíos | Negativo | Alta | ? |
| CN03 | Email inválido | Negativo | Alta | ? |
| CN04 | Clave longitud incorrecta | Negativo | Alta | ? |
| CN05 | Exceder límites | Negativo | Media | ? |
| CI01 | Flujo completo | Integración | Alta | ? |
| CI02 | Ventanas duplicadas | Integración | Media | ? |
| CI03 | Cancelar operación | Integración | Media | ? |
| CI04 | Encriptación | Integración | Alta | ? |

---

## ?? Escalabilidad

### Cómo Extender el Sistema

#### 1. Agregar Base de Datos Real

**Paso 1:** Crear nueva implementación del repositorio

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

    // ... otros métodos
}
```

**Paso 2:** Cambiar registro en DI

```csharp
// Antes
services.AddSingleton<InMemoryContext>();
services.AddScoped<IUsuarioRepository, UsuarioRepository>();

// Después
services.AddScoped<IUsuarioRepository>(sp => 
    new SqlUsuarioRepository("Server=...;Database=..."));
```

**¡Listo!** No se modifica ninguna otra capa.

---

#### 2. Agregar Modificación de Usuario

**Paso 1:** Agregar método a interfaz

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
        Apellido = "Pérez",
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

### Patrón para Nuevas Entidades

**Ejemplo: Agregar gestión de Citas**

```
1. DOM/Cita.cs (entidad)
2. ABS/Repositories/ICitaRepository.cs (interfaz)
3. ABS/Application/ICitaService.cs (interfaz servicio)
4. REPO/CitaRepository.cs (implementación repositorio)
5. APP/CitaService.cs (implementación servicio)
6. PeluqueriaSystem/frmCitas.cs (UI listado)
7. PeluqueriaSystem/frmAltaCita.cs (UI alta)
8. Registrar en DI
```

**Seguir el mismo patrón de Usuario ? Garantiza consistencia**

---

## ? Checklist de Calidad

### Antes de Entregar/Commitear

#### Compilación
- [ ] `dotnet build` sin errores
- [ ] `dotnet build` sin warnings
- [ ] Todas las referencias de proyecto correctas

#### Código
- [ ] Sin código comentado
- [ ] Sin TODOs pendientes críticos
- [ ] Sin console.WriteLine() de depuración
- [ ] Nombres de variables descriptivos
- [ ] Métodos pequeños (<20 líneas idealmente)

#### Arquitectura
- [ ] Flujo de dependencias correcto
- [ ] UI no llama directamente a repositorio
- [ ] APP no depende de Windows Forms
- [ ] Todas las dependencias mediante interfaces

#### Funcionalidad
- [ ] Crear usuario con datos válidos funciona
- [ ] Validaciones funcionan correctamente
- [ ] Email duplicado es detectado
- [ ] Clave de 11 caracteres es validada
- [ ] Lista se actualiza después de crear

#### Testing
- [ ] Al menos los casos positivos básicos probados manualmente
- [ ] Al menos un caso negativo probado (email duplicado)
- [ ] Flujo completo funciona

#### Documentación
- [ ] README.md actualizado
- [ ] Comentarios XML en clases públicas
- [ ] DEVELOPMENT.md refleja arquitectura actual

---

## ?? Referencias y Recursos

### Libros Recomendados
- **Clean Architecture** - Robert C. Martin
- **Clean Code** - Robert C. Martin
- **Design Patterns** - Gang of Four
- **Domain-Driven Design** - Eric Evans

### Enlaces Útiles
- [SOLID Principles](https://en.wikipedia.org/wiki/SOLID)
- [Clean Architecture (artículo)](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Dependency Injection en .NET](https://docs.microsoft.com/en-us/dotnet/core/extensions/dependency-injection)
- [C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)

---

## ?? Conclusión

Este proyecto es un ejemplo completo de:
- ? Arquitectura limpia y escalable
- ? Código profesional y mantenible
- ? Aplicación práctica de principios SOLID
- ? Uso correcto de Dependency Injection
- ? Balance entre teoría y pragmatismo

**Puede servir como plantilla para futuros proyectos Windows Forms con Clean Architecture.**

---

**Última actualización:** $(date)  
**Mantenedor:** Sistema de Gestión Peluquería  
**Versión:** 1.0
