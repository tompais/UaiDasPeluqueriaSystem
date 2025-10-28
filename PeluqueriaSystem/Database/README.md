# ?? Scripts de Base de Datos - PeluSystem

Esta carpeta contiene todos los scripts SQL necesarios para crear y configurar la base de datos del Sistema de Gestión de Peluquería.

---

## ?? Resumen Ejecutivo

Se han creado **8 archivos SQL** que cumplen con **todos los requerimientos** de la Parte 2 del proyecto.

**Estado:** ? **PARTE 2 COMPLETADA AL 100%**

---

## ?? Lista de Scripts

| # | Archivo | Descripción | Ejecutable |
|---|---------|-------------|-----------|
| **0a** | `00_MasterScript.sql` | Script maestro | ? Sí (requiere SQLCMD Mode) |
| **0b** | `00_CompleteScript_Standalone.sql` | Script completo | ? Sí ? RECOMENDADO |
| **1** | `01_CreateDatabase.sql` | Crea PeluSystem | ? Sí |
| **2** | `02_CreateTables.sql` | Rol, Estado, Usuario | ? Sí |
| **3** | `03_CreateForeignKeys.sql` | Foreign Keys | ? Sí |
| **4** | `04_SeedData.sql` | Datos iniciales | ? Sí |
| **5** | `05_CRUDOperations.sql` | CRUD (Puntos 6,7,8) | ? Sí |
| **6** | `06_UsuarioExamples.sql` | Ejemplos Usuario | ? Sí |

---

## ?? Cómo Usar - 3 Opciones

### ? Opción 1: Script Standalone (MÁS FÁCIL)

```sql
-- Abrir en SSMS: 00_CompleteScript_Standalone.sql
-- Presionar F5
-- ¡Listo!
```

? No requiere configuración especial

---

### Opción 2: Script Maestro

```sql
-- 1. Query ? SQLCMD Mode (activar)
-- 2. Abrir: 00_MasterScript.sql
-- 3. F5
```

? Scripts modulares

---

### Opción 3: Scripts Individuales

Ejecutar en orden: 01 ? 02 ? 03 ? 04 ? 05 ? 06

---

## ? Cumplimiento de Requerimientos

| Punto | Descripción | Estado | Archivo |
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

### Usuario

- ID (IDENTITY)
- Nombre, Apellido, Email
- Rol ? FK Rol.ID
- Estado ? FK Estado.ID
- Clave, DV
- Usuario_Agregar, Usuario_Modificar
- Fecha_Agregar (default GETDATE()), Fecha_Modificar

---

## ?? Características

? **Idempotente**: Ejecutable múltiples veces sin error
? **Feedback visual**: Mensajes durante ejecución
? **FK Relations**: Integridad referencial
? **Mejoras aplicadas**: ID autoincremental, defaults, nombres corregidos

---

## ? Verificación

```sql
USE PeluSystem;

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

## ?? Solución de Problemas

### Error: "Incorrect syntax near ':'"
- **Solución**: Activar SQLCMD Mode o usar `00_CompleteScript_Standalone.sql`

### Error: "Cannot drop database"
- **Solución**: El script ya incluye `SET SINGLE_USER WITH ROLLBACK IMMEDIATE`

---

## ?? Próximos Pasos

1. ? Parte 2: Base de datos creada
2. ?? Parte 3: Conectar C# a SQL Server
3. ?? Parte 4: Migrar InMemoryContext
4. ?? Parte 5: CRUD con BD real

---

## ?? Diferencias C# vs BD

| Aspecto | C# (Parte 1) | BD (Parte 2) |
|---------|--------------|--------------|
| Roles | Enum | Tabla (4 registros) |
| Estados | Enum | Tabla (2 registros) |
| Persistencia | InMemory | SQL Server |
| IDs | Código | IDENTITY |

---

## ?? Conclusión

? Todos los requerimientos completados
? Scripts profesionales e idempotentes
? Listo para Parte 3 (conexión C#)

---

**Autor:** GitHub Copilot  
**Proyecto:** Sistema Gestión Peluquería  
**Versión:** 1.0  
**Estado:** ? Completado
