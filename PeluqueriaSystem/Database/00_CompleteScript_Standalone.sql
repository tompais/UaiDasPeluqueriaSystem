-- =============================================
-- Script Completo Standalone (Sin SQLCMD Mode)
-- Descripci�n: Todo en un solo archivo para ejecuci�n directa
-- Uso: Ejecutar directamente en SSMS sin SQLCMD Mode
-- =============================================

PRINT '========================================';
PRINT 'SISTEMA DE GESTI�N PELUQUER�A';
PRINT 'Script de Creaci�n de Base de Datos';
PRINT 'Versi�n: Standalone (Sin SQLCMD)';
PRINT '========================================';
PRINT '';
GO

-- =============================================
-- PASO 1: CREAR BASE DE DATOS
-- =============================================
PRINT '>>> PASO 1: CREANDO BASE DE DATOS...';
PRINT '';

USE master;
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = N'PeluSystem')
BEGIN
    ALTER DATABASE [PeluSystem] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [PeluSystem];
    PRINT '? Base de datos PeluSystem eliminada correctamente';
END
GO

CREATE DATABASE [PeluSystem];
GO

PRINT '? Base de datos PeluSystem creada correctamente';
PRINT '? Contexto cambiado a PeluSystem';
GO

USE [PeluSystem];
GO

PRINT '';
PRINT '========================================';
GO

-- =============================================
-- PASO 2: CREAR TABLAS
-- =============================================
PRINT '>>> PASO 2: CREANDO TABLAS...';
PRINT '';
PRINT 'Creando tabla Rol...';

IF OBJECT_ID('[dbo].[Rol]', 'U') IS NOT NULL
BEGIN
    DROP TABLE [dbo].[Rol];
    PRINT '  ? Tabla Rol eliminada (ya exist�a)';
END
GO

CREATE TABLE [dbo].[Rol] (
    [ID] INT NOT NULL IDENTITY(1,1),
    [Detalle] VARCHAR(50) NOT NULL,
    CONSTRAINT [PK_Rol] PRIMARY KEY CLUSTERED ([ID] ASC)
    WITH (
        PAD_INDEX = OFF,
  STATISTICS_NORECOMPUTE = OFF,
        IGNORE_DUP_KEY = OFF,
     ALLOW_ROW_LOCKS = ON,
  ALLOW_PAGE_LOCKS = ON,
        OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
    ) ON [PRIMARY]
) ON [PRIMARY];
GO

PRINT '? Tabla Rol creada correctamente';
GO

PRINT 'Creando tabla Estado...';

IF OBJECT_ID('[dbo].[Estado]', 'U') IS NOT NULL
BEGIN
 DROP TABLE [dbo].[Estado];
    PRINT '  ? Tabla Estado eliminada (ya exist�a)';
END
GO

CREATE TABLE [dbo].[Estado] (
    [ID] INT NOT NULL IDENTITY(1,1),
    [Detalle] VARCHAR(50) NOT NULL,
    CONSTRAINT [PK_Estado] PRIMARY KEY CLUSTERED ([ID] ASC)
    WITH (
        PAD_INDEX = OFF,
   STATISTICS_NORECOMPUTE = OFF,
 IGNORE_DUP_KEY = OFF,
ALLOW_ROW_LOCKS = ON,
        ALLOW_PAGE_LOCKS = ON,
        OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
    ) ON [PRIMARY]
) ON [PRIMARY];
GO

PRINT '? Tabla Estado creada correctamente';
GO

PRINT 'Creando tabla Usuario...';

IF OBJECT_ID('[dbo].[Usuario]', 'U') IS NOT NULL
BEGIN
    DROP TABLE [dbo].[Usuario];
    PRINT '  ? Tabla Usuario eliminada (ya exist�a)';
END
GO

CREATE TABLE [dbo].[Usuario] (
    [ID] INT NOT NULL IDENTITY(1,1),
    [Apellido] VARCHAR(80) NULL,
    [Nombre] VARCHAR(50) NULL,
    [Email] VARCHAR(180) NULL,
    [Rol] INT NOT NULL,
    [Estado] INT NOT NULL,
    [Clave] VARCHAR(64) NULL,
    [DV] VARCHAR(50) NULL,
    [Fecha_Agregar] DATETIME NOT NULL DEFAULT GETDATE(),`n    [FechaModificacion] DATETIME NULL,
    CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED (
        [ID] ASC
    ) WITH (
        PAD_INDEX = OFF,
        STATISTICS_NORECOMPUTE = OFF,
        IGNORE_DUP_KEY = OFF,
        ALLOW_ROW_LOCKS = ON,
        ALLOW_PAGE_LOCKS = ON,
        OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
    ) ON [PRIMARY]
) ON [PRIMARY];
GO

PRINT '? Tabla Usuario creada correctamente';
PRINT '';
PRINT '========================================';
GO

-- =============================================
-- PASO 3: CREAR RELACIONES
-- =============================================
PRINT '>>> PASO 3: CREANDO RELACIONES...';
PRINT '';
PRINT 'Creando relaciones entre tablas...';

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Usuario_Rol')
BEGIN
    ALTER TABLE [dbo].[Usuario] DROP CONSTRAINT FK_Usuario_Rol;
    PRINT '  ? FK_Usuario_Rol eliminada (ya exist�a)';
END
GO

ALTER TABLE [dbo].[Usuario]
ADD CONSTRAINT FK_Usuario_Rol FOREIGN KEY ([Rol])
REFERENCES [dbo].[Rol]([ID]);
GO

PRINT '? Relaci�n Usuario ? Rol creada';
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Usuario_Estado')
BEGIN
    ALTER TABLE [dbo].[Usuario] DROP CONSTRAINT FK_Usuario_Estado;
    PRINT '  ? FK_Usuario_Estado eliminada (ya exist�a)';
END
GO

ALTER TABLE [dbo].[Usuario]
ADD CONSTRAINT FK_Usuario_Estado FOREIGN KEY ([Estado])
REFERENCES [dbo].[Estado]([ID]);
GO

PRINT '? Relaci�n Usuario ? Estado creada';
PRINT '? Todas las relaciones creadas correctamente';
PRINT '';
PRINT '========================================';
GO

-- =============================================
-- PASO 4: INSERTAR DATOS INICIALES (PUNTO 5)
-- =============================================
PRINT '>>> PASO 4: INSERTANDO DATOS INICIALES...';
PRINT '';
PRINT 'Insertando registros iniciales en Rol...';

DELETE FROM [dbo].[Rol];
DBCC CHECKIDENT ('[dbo].[Rol]', RESEED, 0);
GO

INSERT INTO [dbo].[Rol] ([Detalle]) VALUES
('Administrador'),
('supervisor'),
('Peluquero'),
('Cajero');
GO

PRINT '? 4 registros insertados en Rol';
GO

PRINT 'Insertando registros iniciales en Estado...';

DELETE FROM [dbo].[Estado];
DBCC CHECKIDENT ('[dbo].[Estado]', RESEED, 0);
GO

INSERT INTO [dbo].[Estado] ([Detalle]) VALUES
('Habilitado'),
('Baja'),
('Temporal');
GO

PRINT '? 3 registros insertados en Estado';
PRINT '';
PRINT '=== DATOS INSERTADOS EN ROL ===';
SELECT * FROM [dbo].[Rol];

PRINT '';
PRINT '=== DATOS INSERTADOS EN ESTADO ===';
SELECT * FROM [dbo].[Estado];
PRINT '';
PRINT '========================================';
GO

-- =============================================
-- PASO 5: OPERACIONES CRUD (PUNTOS 6, 7, 8)
-- =============================================
PRINT '>>> PASO 5: EJECUTANDO OPERACIONES CRUD...';
PRINT '';

-- PUNTO 6: UPDATE
PRINT 'Punto 6: Modificando registro en Rol...';

UPDATE [dbo].[Rol]
SET [Detalle] = 'Supervisor'
WHERE [ID] = 2;
GO

PRINT '? Registro con ID = 2 actualizado: "supervisor" ? "Supervisor"';
GO

SELECT [ID], [Detalle] 
FROM [dbo].[Rol] 
WHERE [ID] = 2;
GO

-- PUNTO 7: DELETE
PRINT '';
PRINT 'Punto 7: Eliminando registro en Estado...';

DELETE FROM [dbo].[Estado]
WHERE [ID] = 3;
GO

PRINT '? Registro con ID = 3 eliminado ("Temporal")';
GO

SELECT [ID], [Detalle] 
FROM [dbo].[Estado];
GO

-- PUNTO 8: SELECT
PRINT '';
PRINT 'Punto 8: Consultando registro en Rol con ID = 4...';

SELECT [Detalle]
FROM [dbo].[Rol]
WHERE [ID] = 4;
GO

PRINT '? Consulta ejecutada correctamente';
PRINT '';
PRINT '========================================';
PRINT 'RESUMEN DE OPERACIONES COMPLETADAS';
PRINT '========================================';
PRINT '? Punto 6: Registro actualizado en Rol (ID=2)';
PRINT '? Punto 7: Registro eliminado en Estado (ID=3)';
PRINT '? Punto 8: Consulta ejecutada en Rol (ID=4)';
PRINT '';
PRINT '=== ESTADO FINAL DE LAS TABLAS ===';

SELECT * FROM [dbo].[Rol];
SELECT * FROM [dbo].[Estado];
PRINT '';
PRINT '========================================';
GO

-- =============================================
-- PASO 6: EJEMPLOS EN USUARIO (PUNTO 3)
-- =============================================
PRINT '>>> PASO 6: EJECUTANDO EJEMPLOS EN USUARIO...';
PRINT '';
PRINT '1. INSERT - Insertando usuario de ejemplo...';

DELETE FROM [dbo].[Usuario];
DBCC CHECKIDENT ('[dbo].[Usuario]', RESEED, 0);
GO

INSERT INTO [dbo].[Usuario] (
    [Apellido], [Nombre], [Email], [Rol], [Estado], [Clave], [DV]
) VALUES (
    'Lennon',
    'Jhon',
    'jlennon@lennon.com',
    1,
    1,
    '81DC9BDB52D04DC20036DBD8313ED055',  -- MD5 hash de '1234'
    '43534h5jk43h5'
);
GO

PRINT '? Usuario insertado con ID = 1';
PRINT '';
PRINT '2. SELECT - Consultando todos los usuarios...';

SELECT * FROM [dbo].[Usuario];
GO

PRINT '';
PRINT '3. SELECT - Consultando usuario con ID = 1...';

SELECT * FROM [dbo].[Usuario] WHERE [ID] = 1;
GO

PRINT '';
PRINT '4. UPDATE - Actualizando email del usuario...';

UPDATE [dbo].[Usuario]
SET [Email] = 'JLennon@Lennon.com.ar'
WHERE [ID] = 1;
GO

PRINT '? Email actualizado: "jlennon@lennon.com" ? "JLennon@Lennon.com.ar"';
GO

SELECT [ID], [Nombre], [Apellido], [Email]
FROM [dbo].[Usuario]
WHERE [ID] = 1;
GO

PRINT '';
PRINT '5. DELETE - Eliminando usuario con ID = 1...';

DELETE FROM [dbo].[Usuario]
WHERE [ID] = 1;
GO

PRINT '? Usuario eliminado';
GO

SELECT COUNT(*) AS 'Total Usuarios' FROM [dbo].[Usuario];
GO

PRINT '';
PRINT '? Todas las operaciones CRUD ejecutadas correctamente';
PRINT '';
PRINT '========================================';
GO

-- =============================================
-- PASO 7: CREAR TABLAS DE PATENTES Y FAMILIAS
-- =============================================
PRINT '>>> PASO 7: CREANDO TABLAS DE PATENTES Y FAMILIAS...';
PRINT '';

-- 1. TABLA OPCIONES (Patentes)
PRINT 'Creando tabla Opciones...';

IF OBJECT_ID('[dbo].[Opciones]', 'U') IS NOT NULL
BEGIN
    DROP TABLE [dbo].[Opciones];
    PRINT '  ? Tabla Opciones eliminada (ya existía)';
END
GO

CREATE TABLE [dbo].[Opciones] (
    [ID] INT NOT NULL IDENTITY(1,1),
    [Nombre] VARCHAR(100) NOT NULL,
    CONSTRAINT [PK_Opciones] PRIMARY KEY CLUSTERED ([ID] ASC)
    WITH (
        PAD_INDEX = OFF,
        STATISTICS_NORECOMPUTE = OFF,
        IGNORE_DUP_KEY = OFF,
        ALLOW_ROW_LOCKS = ON,
        ALLOW_PAGE_LOCKS = ON,
        OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
    ) ON [PRIMARY]
) ON [PRIMARY];
GO

PRINT '? Tabla Opciones creada correctamente';
GO

-- 2. TABLA FAMILIA
PRINT 'Creando tabla Familia...';

IF OBJECT_ID('[dbo].[Familia]', 'U') IS NOT NULL
BEGIN
    DROP TABLE [dbo].[Familia];
    PRINT '  ? Tabla Familia eliminada (ya existía)';
END
GO

CREATE TABLE [dbo].[Familia] (
    [ID] INT NOT NULL IDENTITY(1,1),
    [Nombre] VARCHAR(100) NOT NULL,
    CONSTRAINT [PK_Familia] PRIMARY KEY CLUSTERED ([ID] ASC)
    WITH (
        PAD_INDEX = OFF,
        STATISTICS_NORECOMPUTE = OFF,
        IGNORE_DUP_KEY = OFF,
        ALLOW_ROW_LOCKS = ON,
        ALLOW_PAGE_LOCKS = ON,
        OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
    ) ON [PRIMARY]
) ON [PRIMARY];
GO

PRINT '? Tabla Familia creada correctamente';
GO

-- 3. TABLA FAMILIAPATENTE (Relación Familia-Patente)
PRINT 'Creando tabla FamiliaPatente...';

IF OBJECT_ID('[dbo].[FamiliaPatente]', 'U') IS NOT NULL
BEGIN
    DROP TABLE [dbo].[FamiliaPatente];
    PRINT '  ? Tabla FamiliaPatente eliminada (ya existía)';
END
GO

CREATE TABLE [dbo].[FamiliaPatente] (
    [IDFamilia] INT NOT NULL,
    [IDPatente] INT NOT NULL,
    CONSTRAINT [PK_FamiliaPatente] PRIMARY KEY CLUSTERED ([IDFamilia] ASC, [IDPatente] ASC)
    WITH (
        PAD_INDEX = OFF,
        STATISTICS_NORECOMPUTE = OFF,
        IGNORE_DUP_KEY = OFF,
        ALLOW_ROW_LOCKS = ON,
        ALLOW_PAGE_LOCKS = ON,
        OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
    ) ON [PRIMARY],
    CONSTRAINT [FK_FamiliaPatente_Familia] FOREIGN KEY ([IDFamilia]) 
        REFERENCES [dbo].[Familia]([ID])
        ON DELETE CASCADE
        ON UPDATE CASCADE,
    CONSTRAINT [FK_FamiliaPatente_Opciones] FOREIGN KEY ([IDPatente]) 
        REFERENCES [dbo].[Opciones]([ID])
        ON DELETE CASCADE
        ON UPDATE CASCADE
) ON [PRIMARY];
GO

PRINT '? Tabla FamiliaPatente creada correctamente';
GO

-- 4. TABLA FAMILIAFAMILIA (Relación Familia-Familia recursiva)
PRINT 'Creando tabla FamiliaFamilia...';

IF OBJECT_ID('[dbo].[FamiliaFamilia]', 'U') IS NOT NULL
BEGIN
    DROP TABLE [dbo].[FamiliaFamilia];
    PRINT '  ? Tabla FamiliaFamilia eliminada (ya existía)';
END
GO

CREATE TABLE [dbo].[FamiliaFamilia] (
    [IDFamiliaPadre] INT NOT NULL,
    [IDFamiliaHija] INT NOT NULL,
    CONSTRAINT [PK_FamiliaFamilia] PRIMARY KEY CLUSTERED ([IDFamiliaPadre] ASC, [IDFamiliaHija] ASC)
    WITH (
        PAD_INDEX = OFF,
        STATISTICS_NORECOMPUTE = OFF,
        IGNORE_DUP_KEY = OFF,
        ALLOW_ROW_LOCKS = ON,
        ALLOW_PAGE_LOCKS = ON,
        OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
    ) ON [PRIMARY],
    CONSTRAINT [FK_FamiliaFamilia_Padre] FOREIGN KEY ([IDFamiliaPadre]) 
        REFERENCES [dbo].[Familia]([ID]),
    CONSTRAINT [FK_FamiliaFamilia_Hija] FOREIGN KEY ([IDFamiliaHija]) 
        REFERENCES [dbo].[Familia]([ID])
) ON [PRIMARY];
GO

PRINT '? Tabla FamiliaFamilia creada correctamente';
PRINT '';
PRINT '========================================';
GO

-- =============================================
-- PASO 8: INSERTAR DATOS INICIALES DE PATENTES Y FAMILIAS
-- =============================================
PRINT '>>> PASO 8: INSERTANDO DATOS INICIALES DE PATENTES Y FAMILIAS...';
PRINT '';

-- Insertar Patentes (Opciones)
PRINT 'Insertando patentes iniciales...';

INSERT INTO [dbo].[Opciones] ([Nombre]) VALUES ('Usuario_Alta');
INSERT INTO [dbo].[Opciones] ([Nombre]) VALUES ('Usuario_Baja');
INSERT INTO [dbo].[Opciones] ([Nombre]) VALUES ('Usuario_Modificar');
INSERT INTO [dbo].[Opciones] ([Nombre]) VALUES ('Usuario_Consultar');
INSERT INTO [dbo].[Opciones] ([Nombre]) VALUES ('Cliente_Alta');
INSERT INTO [dbo].[Opciones] ([Nombre]) VALUES ('Cliente_Baja');
INSERT INTO [dbo].[Opciones] ([Nombre]) VALUES ('Cliente_Modificar');
INSERT INTO [dbo].[Opciones] ([Nombre]) VALUES ('Cliente_Consultar');
INSERT INTO [dbo].[Opciones] ([Nombre]) VALUES ('Turno_Alta');
INSERT INTO [dbo].[Opciones] ([Nombre]) VALUES ('Turno_Baja');
INSERT INTO [dbo].[Opciones] ([Nombre]) VALUES ('Turno_Modificar');
INSERT INTO [dbo].[Opciones] ([Nombre]) VALUES ('Turno_Consultar');
INSERT INTO [dbo].[Opciones] ([Nombre]) VALUES ('Reporte_Ventas');
INSERT INTO [dbo].[Opciones] ([Nombre]) VALUES ('Reporte_Turnos');
INSERT INTO [dbo].[Opciones] ([Nombre]) VALUES ('Reporte_Clientes');
INSERT INTO [dbo].[Opciones] ([Nombre]) VALUES ('Sistema_Configuracion');
INSERT INTO [dbo].[Opciones] ([Nombre]) VALUES ('Sistema_Backup');
INSERT INTO [dbo].[Opciones] ([Nombre]) VALUES ('Sistema_Restore');

PRINT '? Patentes insertadas correctamente';
GO

-- Insertar Familias
PRINT 'Insertando familias iniciales...';

INSERT INTO [dbo].[Familia] ([Nombre]) VALUES ('Familia_Usuarios');
INSERT INTO [dbo].[Familia] ([Nombre]) VALUES ('Familia_Clientes');
INSERT INTO [dbo].[Familia] ([Nombre]) VALUES ('Familia_Turnos');
INSERT INTO [dbo].[Familia] ([Nombre]) VALUES ('Familia_Reportes');
INSERT INTO [dbo].[Familia] ([Nombre]) VALUES ('Familia_Sistema');
INSERT INTO [dbo].[Familia] ([Nombre]) VALUES ('Rol_Empleado');
INSERT INTO [dbo].[Familia] ([Nombre]) VALUES ('Rol_Supervisor');
INSERT INTO [dbo].[Familia] ([Nombre]) VALUES ('Rol_Administrador');

PRINT '? Familias insertadas correctamente';
GO

-- Asignar Patentes a Familias
PRINT 'Asignando patentes a familias...';

-- Familia Usuarios
INSERT INTO [dbo].[FamiliaPatente] ([IDFamilia], [IDPatente]) VALUES (1, 1), (1, 2), (1, 3), (1, 4);
-- Familia Clientes
INSERT INTO [dbo].[FamiliaPatente] ([IDFamilia], [IDPatente]) VALUES (2, 5), (2, 6), (2, 7), (2, 8);
-- Familia Turnos
INSERT INTO [dbo].[FamiliaPatente] ([IDFamilia], [IDPatente]) VALUES (3, 9), (3, 10), (3, 11), (3, 12);
-- Familia Reportes
INSERT INTO [dbo].[FamiliaPatente] ([IDFamilia], [IDPatente]) VALUES (4, 13), (4, 14), (4, 15);
-- Familia Sistema
INSERT INTO [dbo].[FamiliaPatente] ([IDFamilia], [IDPatente]) VALUES (5, 16), (5, 17), (5, 18);

PRINT '? Patentes asignadas a familias correctamente';
GO

-- Asignar Familias a Roles (COMPOSITE)
PRINT 'Asignando familias a roles...';

-- Rol Empleado: Clientes y Turnos
INSERT INTO [dbo].[FamiliaFamilia] ([IDFamiliaPadre], [IDFamiliaHija]) VALUES (6, 2), (6, 3);
-- Rol Supervisor: Empleado + Reportes
INSERT INTO [dbo].[FamiliaFamilia] ([IDFamiliaPadre], [IDFamiliaHija]) VALUES (7, 6), (7, 4);
-- Rol Administrador: Supervisor + Usuarios + Sistema
INSERT INTO [dbo].[FamiliaFamilia] ([IDFamiliaPadre], [IDFamiliaHija]) VALUES (8, 7), (8, 1), (8, 5);

PRINT '? Familias asignadas a roles correctamente';
PRINT '';
PRINT '========================================';
GO

-- =============================================
-- PASO 9: CREAR TABLA DICCIONARIO (MULTI-IDIOMA)
-- =============================================
PRINT '';
PRINT '>>> PASO 9: CREANDO TABLA DICCIONARIO...';
PRINT '';

-- Eliminar tabla si existe
IF OBJECT_ID('[dbo].[Diccionario]', 'U') IS NOT NULL
BEGIN
    DROP TABLE [dbo].[Diccionario];
    PRINT '  ? Tabla Diccionario eliminada (ya exist�a)';
END
GO

-- Crear tabla Diccionario
CREATE TABLE [dbo].[Diccionario] (
    [ID] INT NOT NULL IDENTITY(1,1),
    [IDIdioma] INT NOT NULL,
    [PalabraOriginal] VARCHAR(100) NOT NULL,
    [PalabraIdioma] VARCHAR(100) NOT NULL,
    CONSTRAINT [PK_Diccionario] PRIMARY KEY CLUSTERED ([ID] ASC)
    WITH (
        PAD_INDEX = OFF,
        STATISTICS_NORECOMPUTE = OFF,
        IGNORE_DUP_KEY = OFF,
        ALLOW_ROW_LOCKS = ON,
        ALLOW_PAGE_LOCKS = ON,
        OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
    ) ON [PRIMARY]
) ON [PRIMARY];
GO

PRINT '? Tabla Diccionario creada correctamente';
GO

-- =============================================
-- PASO 10: INSERTAR DATOS EN DICCIONARIO
-- =============================================
PRINT '';
PRINT '>>> PASO 10: INSERTANDO DATOS EN DICCIONARIO...';
PRINT '';

-- IDIdioma = 1: Espa�ol
-- IDIdioma = 2: Ingl�s
-- IDIdioma = 3: Portugu�s

-- Palabras comunes en Espa�ol
INSERT INTO [dbo].[Diccionario] ([IDIdioma], [PalabraOriginal], [PalabraIdioma]) VALUES
    (1, 'Usuario', 'Usuario'),
    (1, 'Nombre', 'Nombre'),
    (1, 'Apellido', 'Apellido'),
    (1, 'Email', 'Email'),
    (1, 'Clave', 'Clave'),
    (1, 'Estado', 'Estado'),
    (1, 'Rol', 'Rol'),
    (1, 'Activo', 'Activo'),
    (1, 'Inactivo', 'Inactivo'),
    (1, 'Guardar', 'Guardar'),
    (1, 'Cancelar', 'Cancelar'),
    (1, 'Aceptar', 'Aceptar'),
    (1, 'Eliminar', 'Eliminar'),
    (1, 'Modificar', 'Modificar'),
    (1, 'Nuevo', 'Nuevo'),
    (1, 'Buscar', 'Buscar'),
    (1, 'Administraci�n', 'Administraci�n'),
    (1, 'Usuarios', 'Usuarios');

-- Palabras comunes en Ingl�s
INSERT INTO [dbo].[Diccionario] ([IDIdioma], [PalabraOriginal], [PalabraIdioma]) VALUES
    (2, 'Usuario', 'User'),
    (2, 'Nombre', 'Name'),
    (2, 'Apellido', 'Last Name'),
    (2, 'Email', 'Email'),
    (2, 'Clave', 'Password'),
    (2, 'Estado', 'Status'),
    (2, 'Rol', 'Role'),
    (2, 'Activo', 'Active'),
    (2, 'Inactivo', 'Inactive'),
    (2, 'Guardar', 'Save'),
    (2, 'Cancelar', 'Cancel'),
    (2, 'Aceptar', 'Accept'),
    (2, 'Eliminar', 'Delete'),
    (2, 'Modificar', 'Modify'),
    (2, 'Nuevo', 'New'),
    (2, 'Buscar', 'Search'),
    (2, 'Administraci�n', 'Administration'),
    (2, 'Usuarios', 'Users');

-- Palabras comunes en Portugu�s
INSERT INTO [dbo].[Diccionario] ([IDIdioma], [PalabraOriginal], [PalabraIdioma]) VALUES
    (3, 'Usuario', 'Usu�rio'),
    (3, 'Nombre', 'Nome'),
    (3, 'Apellido', 'Sobrenome'),
    (3, 'Email', 'Email'),
    (3, 'Clave', 'Senha'),
    (3, 'Estado', 'Estado'),
    (3, 'Rol', 'Fun��o'),
    (3, 'Activo', 'Ativo'),
    (3, 'Inactivo', 'Inativo'),
    (3, 'Guardar', 'Salvar'),
    (3, 'Cancelar', 'Cancelar'),
    (3, 'Aceptar', 'Aceitar'),
    (3, 'Eliminar', 'Excluir'),
    (3, 'Modificar', 'Modificar'),
    (3, 'Nuevo', 'Novo'),
    (3, 'Buscar', 'Pesquisar'),
    (3, 'Administraci�n', 'Administra��o'),
    (3, 'Usuarios', 'Usu�rios');

PRINT '? Datos de ejemplo insertados correctamente (54 traducciones)';
PRINT '';
PRINT '========================================';
GO

-- =============================================
-- RESUMEN FINAL
-- =============================================
PRINT '';
PRINT '========================================';
PRINT '??? SCRIPT COMPLETADO EXITOSAMENTE ???';
PRINT '========================================';
PRINT '';
PRINT 'Base de datos: PeluSystem';
PRINT 'Tablas creadas: Rol, Estado, Usuario, Opciones, Familia, FamiliaPatente, FamiliaFamilia, Diccionario';
PRINT 'Relaciones: FK_Usuario_Rol, FK_Usuario_Estado, FK_FamiliaPatente_Familia, FK_FamiliaPatente_Opciones, FK_FamiliaFamilia_Padre, FK_FamiliaFamilia_Hija';
PRINT '';
PRINT 'Estado de las tablas:';
PRINT '  - Rol: 4 registros (Administrador, Supervisor, Peluquero, Cajero)';
PRINT '  - Estado: 2 registros (Habilitado, Baja)';
PRINT '  - Usuario: 0 registros (tabla vac�a tras ejemplos)';
PRINT '  - Opciones: 18 registros (patentes del sistema)';
PRINT '  - Familia: 8 registros (familias de permisos y roles)';
PRINT '  - FamiliaPatente: 18 registros (patentes asignadas a familias)';
PRINT '  - FamiliaFamilia: 6 registros (familias anidadas)';
PRINT '  - Diccionario: 54 registros (18 palabras x 3 idiomas: ES, EN, PT)';
PRINT '';
PRINT '========================================';
PRINT '';
PRINT '�Base de datos lista para usar con soporte multi-idioma!';
PRINT '';
GO
