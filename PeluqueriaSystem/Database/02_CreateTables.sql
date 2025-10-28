-- =============================================
-- Script: Creación de Tablas
-- Descripción: Crea las tablas Rol, Estado y Usuario con sus restricciones
-- =============================================

USE [PeluSystem];
GO

-- =============================================
-- 1. TABLA ROL
-- =============================================
PRINT 'Creando tabla Rol...';

-- Eliminar tabla si existe
IF OBJECT_ID('[dbo].[Rol]', 'U') IS NOT NULL
BEGIN
    DROP TABLE [dbo].[Rol];
 PRINT '  ? Tabla Rol eliminada (ya existía)';
END
GO

-- Crear tabla Rol
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

-- =============================================
-- 2. TABLA ESTADO
-- =============================================
PRINT 'Creando tabla Estado...';

-- Eliminar tabla si existe
IF OBJECT_ID('[dbo].[Estado]', 'U') IS NOT NULL
BEGIN
    DROP TABLE [dbo].[Estado];
    PRINT '  ? Tabla Estado eliminada (ya existía)';
END
GO

-- Crear tabla Estado
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

-- =============================================
-- 3. TABLA USUARIO
-- =============================================
PRINT 'Creando tabla Usuario...';

-- Eliminar tabla si existe
IF OBJECT_ID('[dbo].[Usuario]', 'U') IS NOT NULL
BEGIN
    DROP TABLE [dbo].[Usuario];
    PRINT '  ? Tabla Usuario eliminada (ya existía)';
END
GO

-- Crear tabla Usuario con mejoras aplicadas:
-- - ID autoincremental con IDENTITY(1,1)
-- - Fecha_Agregar con valor por defecto GETDATE()
-- - Corrección de nombre: Usuario_Modificar (antes Usaurio_modificar)
CREATE TABLE [dbo].[Usuario] (
    [ID] INT NOT NULL IDENTITY(1,1),
    [Apellido] VARCHAR(80) NULL,
    [Nombre] VARCHAR(50) NULL,
    [Email] VARCHAR(180) NULL,
    [Rol] INT NOT NULL,
    [Estado] INT NOT NULL,
    [Clave] VARCHAR(11) NULL,
  [DV] VARCHAR(50) NULL,
    [Usuario_Agregar] INT NOT NULL,
    [Usuario_Modificar] INT NOT NULL,
    [Fecha_Agregar] DATETIME NOT NULL DEFAULT GETDATE(),
    [Fecha_Modificar] DATETIME NOT NULL,
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
GO
