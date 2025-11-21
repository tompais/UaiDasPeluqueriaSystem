-- =============================================
-- Script: Creación de Tablas para Patentes y Familias (Patrón Composite)
-- Descripción: Crea las tablas Opciones, Familia, FamiliaPatente y FamiliaFamilia
-- Parte 6 - Usuario, Familia y Patente
-- =============================================

USE [PeluSystem];
GO

-- =============================================
-- 1. TABLA OPCIONES (Patentes)
-- =============================================
PRINT 'Creando tabla Opciones...';

-- Eliminar tabla si existe
IF OBJECT_ID('[dbo].[Opciones]', 'U') IS NOT NULL
BEGIN
    DROP TABLE [dbo].[Opciones];
    PRINT '  ✓ Tabla Opciones eliminada (ya existía)';
END
GO

-- Crear tabla Opciones
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

PRINT '✓ Tabla Opciones creada correctamente';
GO

-- =============================================
-- 2. TABLA FAMILIA
-- =============================================
PRINT 'Creando tabla Familia...';

-- Eliminar tabla si existe
IF OBJECT_ID('[dbo].[Familia]', 'U') IS NOT NULL
BEGIN
    DROP TABLE [dbo].[Familia];
    PRINT '  ✓ Tabla Familia eliminada (ya existía)';
END
GO

-- Crear tabla Familia
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

PRINT '✓ Tabla Familia creada correctamente';
GO

-- =============================================
-- 3. TABLA FAMILIAPATENTE (Relación Familia-Patente)
-- =============================================
PRINT 'Creando tabla FamiliaPatente...';

-- Eliminar tabla si existe
IF OBJECT_ID('[dbo].[FamiliaPatente]', 'U') IS NOT NULL
BEGIN
    DROP TABLE [dbo].[FamiliaPatente];
    PRINT '  ✓ Tabla FamiliaPatente eliminada (ya existía)';
END
GO

-- Crear tabla FamiliaPatente
-- Relación muchos a muchos entre Familia y Opciones (Patentes)
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

PRINT '✓ Tabla FamiliaPatente creada correctamente';
GO

-- =============================================
-- 4. TABLA FAMILIAFAMILIA (Relación Familia-Familia recursiva)
-- =============================================
PRINT 'Creando tabla FamiliaFamilia...';

-- Eliminar tabla si existe
IF OBJECT_ID('[dbo].[FamiliaFamilia]', 'U') IS NOT NULL
BEGIN
    DROP TABLE [dbo].[FamiliaFamilia];
    PRINT '  ✓ Tabla FamiliaFamilia eliminada (ya existía)';
END
GO

-- Crear tabla FamiliaFamilia
-- Relación muchos a muchos auto-referencial (Familia puede contener otras Familias)
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

PRINT '✓ Tabla FamiliaFamilia creada correctamente';
GO

-- =============================================
-- RESUMEN
-- =============================================
PRINT '';
PRINT '========================================';
PRINT '✓ TABLAS DE PATENTES Y FAMILIAS CREADAS';
PRINT '========================================';
PRINT '';
PRINT 'Tablas creadas:';
PRINT '  - Opciones (Patentes): Permisos individuales';
PRINT '  - Familia: Agrupación de permisos';
PRINT '  - FamiliaPatente: Relación Familia-Patente';
PRINT '  - FamiliaFamilia: Relación Familia-Familia (recursiva)';
PRINT '';
PRINT '========================================';
GO
