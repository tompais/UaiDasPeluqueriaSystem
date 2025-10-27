### Optimización con C# 11 y Nullable Reference Types

**? Sin validaciones redundantes en constructores (DI):**
```csharp
public UsuarioService(IUsuarioRepository repository)
{
    _repository = repository;  // ? Sin validación null
}
```

**¿Por qué es seguro?**
1. **NRT (Nullable Reference Types)**: El compilador advierte si pasas null
2. **DI Container**: `GetRequiredService<T>()` lanza excepción si no está registrado
3. **YAGNI**: No agregamos validaciones que nunca se ejecutarán

**? Sin validaciones redundantes en el repositorio:**

El repositorio es una **capa interna** llamada únicamente por el servicio, que ya validó todos los datos.

```csharp
// ? Antes (redundante)
public Usuario Crear(Usuario usuario)
{
    ArgumentNullException.ThrowIfNull(usuario);  // ? Innecesario
    _context.AgregarUsuario(usuario);
    return usuario;
}

// ? Ahora (confiamos en el servicio)
public Usuario Crear(Usuario usuario)
{
    _context.AgregarUsuario(usuario);
    return usuario;
}
```

**Validaciones que SÍ mantenemos:**
```csharp
// ? En el servicio (capa de aplicación)
public ResultadoOperacion<Usuario> CrearUsuario(CrearUsuarioDto dto)
{
    // Validar entrada del usuario
    if (string.IsNullOrWhiteSpace(dto.Nombre))
        errores.Add("El nombre es obligatorio");
    
    // Validar reglas de negocio
    if (_usuarioRepository.ExisteEmail(dto.Email))
        return ResultadoOperacion<Usuario>.Error("Email ya registrado");
}
```

**Principio aplicado:**
- ? **Validación en la frontera**: Solo donde entran los datos (UI ? Servicio)
- ? **Confianza entre capas**: Capas internas confían en las superiores
- ? **YAGNI**: Sin código defensivo innecesario

---
