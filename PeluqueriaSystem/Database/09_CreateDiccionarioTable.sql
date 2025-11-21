-- =============================================
-- Script: Creación de Tabla Diccionario
-- Descripción: Crea la tabla Diccionario para soporte de múltiples idiomas
-- =============================================

USE [PeluSystem];
GO

-- =============================================
-- TABLA DICCIONARIO
-- =============================================
PRINT 'Creando tabla Diccionario...';

-- Eliminar tabla si existe
IF OBJECT_ID('[dbo].[Diccionario]', 'U') IS NOT NULL
BEGIN
    DROP TABLE [dbo].[Diccionario];
    PRINT '  ✓ Tabla Diccionario eliminada (ya existía)';
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

PRINT '✓ Tabla Diccionario creada correctamente';
GO
