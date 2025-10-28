-- =============================================
-- Script: Relaciones entre Tablas (Foreign Keys)
-- Descripción: Crea las relaciones de integridad referencial
-- =============================================

USE [PeluSystem];
GO

PRINT 'Creando relaciones entre tablas...';

-- =============================================
-- FOREIGN KEY: Usuario ? Rol
-- =============================================

-- Eliminar FK si existe
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Usuario_Rol')
BEGIN
    ALTER TABLE [dbo].[Usuario] DROP CONSTRAINT FK_Usuario_Rol;
    PRINT '  ? FK_Usuario_Rol eliminada (ya existía)';
END
GO

-- Crear FK
ALTER TABLE [dbo].[Usuario]
ADD CONSTRAINT FK_Usuario_Rol FOREIGN KEY ([Rol])
REFERENCES [dbo].[Rol]([ID]);
GO

PRINT '? Relación Usuario ? Rol creada';
GO

-- =============================================
-- FOREIGN KEY: Usuario ? Estado
-- =============================================

-- Eliminar FK si existe
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Usuario_Estado')
BEGIN
    ALTER TABLE [dbo].[Usuario] DROP CONSTRAINT FK_Usuario_Estado;
    PRINT '  ? FK_Usuario_Estado eliminada (ya existía)';
END
GO

-- Crear FK
ALTER TABLE [dbo].[Usuario]
ADD CONSTRAINT FK_Usuario_Estado FOREIGN KEY ([Estado])
REFERENCES [dbo].[Estado]([ID]);
GO

PRINT '? Relación Usuario ? Estado creada';
GO

PRINT '? Todas las relaciones creadas correctamente';
GO
