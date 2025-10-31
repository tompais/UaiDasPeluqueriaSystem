# ?? Scripts de Base de Datos - PeluqueriaSystem

Esta carpeta contiene todos los scripts SQL necesarios para crear y configurar la base de datos del Sistema de Gesti�n de Peluquer�a.

---

## ?? Resumen Ejecutivo

Se han creado **8 archivos SQL** que cumplen con **todos los requerimientos** de las Partes 2, 3 y 4 del proyecto.

**Estado:** ? **PARTE 4 COMPLETADA AL 100%**

### ? Cambios en Parte 4

- Estructura de tabla `Usuario` simplificada
- Eliminadas columnas: `Usuario_Agregar`, `Usuario_Modificar`, `Fecha_Modificar`
- Base de datos renombrada de `PeluSystem` a `PeluqueriaSystem`
- Integraci�n completa con C# mediante arquitectura de N capas

---

## ?? Lista de Scripts

| # | Archivo | Descripci�n | Ejecutable |
|---|---------|-------------|-----------|
| **0a** | `00_MasterScript.sql` | Script maestro | ? S� (requiere SQLCMD Mode) |
| **0b** | `00_CompleteScript_Standalone.sql` | Script completo | ? S� ? RECOMENDADO |
| **1** | `01_CreateDatabase.sql` | Crea PeluqueriaSystem | ? S� |
| **2** | `02_CreateTables.sql` | Rol, Estado, Usuario (simplificado) | ? S� |
| **3** | `03_CreateForeignKeys.sql` | Foreign Keys | ? S� |
| **4** | `04_SeedData.sql` | Datos iniciales | ? S� |
| **5** | `05_CRUDOperations.sql` | CRUD (Puntos 6,7,8) | ? S� |
| **6** | `06_UsuarioExamples.sql` | Ejemplos Usuario | ? S� |

---

## ?? C�mo Usar - 3 Opciones

### ? Opci�n 1: Script Standalone (M�S F�CIL)

```sql
-- Abrir en SSMS: 00_CompleteScript_Standalone.sql
-- Presionar F5
-- �Listo!
```

? No requiere configuraci�n especial

---

### Opci�n 2: Script Maestro

```sql
-- 1. Query ? SQLCMD Mode (activar)
-- 2. Abrir: 00_MasterScript.sql
-- 3. F5
```

? Scripts modulares

---

### Opci�n 3: Scripts Individuales

Ejecutar en orden: 01 ? 02 ? 03 ? 04 ? 05 ? 06

---

## ? Cumplimiento de Requerimientos

| Punto | Descripci�n | Estado | Archivo |
|-------|-------------|--------|---------|
| **1** | Tabla Usuario | ? | `02_CreateTables.sql` |
| **2** | Consultas Usuario | ? | `06_UsuarioExamples.sql` |
| **3** | Tablas Rol/Estado | ? | `02_CreateTables.sql` |
| **4** | Foreign Keys | ? | `03_CreateForeignKeys.sql` |
| **5** | INSERT iniciales | ? Copilot | `04_SeedData.sql` |
| **6** | UPDATE Rol | ? Copilot | `05_CRUDOperations.sql` |
| **7** | DELETE Estado | ? Copilot | `05_CRUDOperations.sql` |
| **8** | SELECT Rol | ? Copilot | `05_CRUDOperations.sql` |

---

## ?? Estructura de Tablas

### Rol (4 registros)

| ID | Detalle |
|----|---------|
| 1 | Administrador |
| 2 | Supervisor |
| 3 | Peluquero |
| 4 | Cajero |

### Estado (2 registros)

| ID | Detalle |
|----|---------|
| 1 | Habilitado |
| 2 | Baja |

### Usuario (Simplificado en Parte 4)

- ID (IDENTITY)
- Nombre, Apellido, Email
- Rol ? FK Rol.ID
- Estado ? FK Estado.ID
- Clave, DV
- Fecha_Agregar (default GETDATE())

**Nota:** Se eliminaron las columnas `Usuario_Agregar`, `Usuario_Modificar` y `Fecha_Modificar` para simplificar la estructura seg�n los requerimientos de la Parte 4.

---

## ?? Caracter�sticas

? **Idempotente**: Ejecutable m�ltiples veces sin error
? **Feedback visual**: Mensajes durante ejecuci�n
? **FK Relations**: Integridad referencial
? **Mejoras aplicadas**: ID autoincremental, defaults, nombres corregidos

---

## ? Verificaci�n

```sql
USE PeluqueriaSystem;

-- Verificar tablas (3 esperadas)
SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE';

-- Verificar datos
SELECT * FROM Rol;    -- 4 registros
SELECT * FROM Estado; -- 2 registros

-- Verificar FKs (2 esperadas)
SELECT fk.name, OBJECT_NAME(fk.parent_object_id) AS Tabla
FROM sys.foreign_keys AS fk;
```

---

## ?? Soluci�n de Problemas

### Error: "Incorrect syntax near ':'"
- **Soluci�n**: Activar SQLCMD Mode o usar `00_CompleteScript_Standalone.sql`

### Error: "Cannot drop database"
- **Soluci�n**: El script ya incluye `SET SINGLE_USER WITH ROLLBACK IMMEDIATE`

---

## ?? Arquitectura de Integraci�n (Parte 4)

### Capas Implementadas

| Capa | Clase | Descripci�n |
|------|-------|-------------|
| **DOM** | `domUsuario` | Entidad de dominio con propiedades del usuario |
| **CONTEXT** | `dalSQLServer` | Manejo de conexi�n y ejecuci�n SQL |
| **REPO** | `repoUsuario` | Clase est�tica con `Traer()` y `CompletarLista()` |
| **APP** | `appUsuario` | Wrapper que llama al repositorio |
| **UI** | `FormUsuarios` | Formulario para mostrar usuarios |

### Configuraci�n de Conexi�n

La cadena de conexi�n puede configurarse mediante:

1. **Variable de entorno** (recomendado para producci�n):
   ```bash
   set PELUQUERIA_CONNECTIONSTRING=Data Source=SERVER;Initial Catalog=PeluqueriaSystem;Integrated Security=True
   ```

2. **Valor por defecto** (desarrollo):
   ```
   Data Source=DESKTOP-02DP0JO;Initial Catalog=PeluqueriaSystem;Integrated Security=True
   ```

---

## ?? Diferencias C# vs BD

| Aspecto | C# (Parte 1) | BD (Parte 2) |
|---------|--------------|--------------|
| Roles | Enum | Tabla (4 registros) |
| Estados | Enum | Tabla (2 registros) |
| Persistencia | InMemory | SQL Server |
| IDs | C�digo | IDENTITY |

---

## ?? Conclusi�n

? Todos los requerimientos de Parte 4 completados
? Scripts profesionales e idempotentes
? Integraci�n completa con arquitectura N capas
? Conexi�n C# ? SQL Server funcionando
? Estructura simplificada y alineada con el c�digo

---

**Autor:** GitHub Copilot  
**Proyecto:** Sistema Gesti�n Peluquer�a  
**Versi�n:** 2.0 (Parte 4)  
**Estado:** ? Completado
