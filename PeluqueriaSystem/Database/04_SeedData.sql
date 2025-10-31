-- =============================================
-- Script: Datos Iniciales (Seed Data)
-- Descripción: Inserta registros iniciales en Rol y Estado
-- Punto 5 del documento de requerimientos
-- =============================================

USE [PeluqueriaSystem];
GO

-- =============================================
-- PUNTO 5: INSERTAR REGISTROS EN ROL
-- =============================================

PRINT 'Insertando registros iniciales en Rol...';

-- Limpiar tabla si tiene datos
DELETE FROM [dbo].[Rol];
DBCC CHECKIDENT ('[dbo].[Rol]', RESEED, 0);
GO

-- Insertar roles
INSERT INTO [dbo].[Rol] ([Detalle]) VALUES
('Administrador'),
('supervisor'),      -- Nota: se corregirá en el siguiente script (Punto 6)
('Peluquero'),
('Cajero');
GO

PRINT '? 4 registros insertados en Rol';
GO

-- =============================================
-- PUNTO 5: INSERTAR REGISTROS EN ESTADO
-- =============================================

PRINT 'Insertando registros iniciales en Estado...';

-- Limpiar tabla si tiene datos
DELETE FROM [dbo].[Estado];
DBCC CHECKIDENT ('[dbo].[Estado]', RESEED, 0);
GO

-- Insertar estados
INSERT INTO [dbo].[Estado] ([Detalle]) VALUES
('Habilitado'),
('Baja'),
('Temporal');   -- Nota: se eliminará en el siguiente script (Punto 7)
GO

PRINT '? 3 registros insertados en Estado';
GO

-- Verificar datos insertados
PRINT '';
PRINT '=== DATOS INSERTADOS EN ROL ===';
SELECT * FROM [dbo].[Rol];

PRINT '';
PRINT '=== DATOS INSERTADOS EN ESTADO ===';
SELECT * FROM [dbo].[Estado];
GO
