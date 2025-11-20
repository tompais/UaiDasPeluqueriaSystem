-- =============================================
-- Script: Creación de Tablas para Patentes y Familias (Patrón Composite)
-- Descripción: Crea las tablas Opciones, Familia y FamiliaElemento
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
-- 3. TABLA FAMILIAELEMENTOS (Relación Composite)
-- =============================================
PRINT 'Creando tabla FamiliaElemento...';

-- Eliminar tabla si existe
IF OBJECT_ID('[dbo].[FamiliaElemento]', 'U') IS NOT NULL
BEGIN
    DROP TABLE [dbo].[FamiliaElemento];
    PRINT '  ✓ Tabla FamiliaElemento eliminada (ya existía)';
END
GO

-- Crear tabla FamiliaElemento
-- Esta tabla permite la relación muchos a muchos entre Familias y Elementos
-- Un elemento puede ser una Patente (Opciones) o una Familia (recursivo)
CREATE TABLE [dbo].[FamiliaElemento] (
    [IDFamilia] INT NOT NULL,
    [IDElemento] INT NOT NULL,
    CONSTRAINT [PK_FamiliaElemento] PRIMARY KEY CLUSTERED ([IDFamilia] ASC, [IDElemento] ASC)
    WITH (
        PAD_INDEX = OFF,
        STATISTICS_NORECOMPUTE = OFF,
        IGNORE_DUP_KEY = OFF,
        ALLOW_ROW_LOCKS = ON,
        ALLOW_PAGE_LOCKS = ON,
        OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
    ) ON [PRIMARY],
    CONSTRAINT [FK_FamiliaElemento_Familia] FOREIGN KEY ([IDFamilia]) 
        REFERENCES [dbo].[Familia]([ID])
        ON DELETE CASCADE
        ON UPDATE CASCADE
) ON [PRIMARY];
GO

PRINT '✓ Tabla FamiliaElemento creada correctamente';
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
PRINT '  - FamiliaElemento: Relación composite';
PRINT '';
PRINT '========================================';
GO
