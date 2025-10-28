-- =============================================
-- Script: Operaciones CRUD - Ejercicios
-- Descripción: Puntos 6, 7 y 8 del documento de requerimientos
-- =============================================

USE [PeluSystem];
GO

-- =============================================
-- PUNTO 6: MODIFICAR EN LA TABLA ROL
-- Cambiar 'supervisor' a 'Supervisor' (ID = 2)
-- =============================================

PRINT 'Punto 6: Modificando registro en Rol...';

UPDATE [dbo].[Rol]
SET [Detalle] = 'Supervisor'
WHERE [ID] = 2;
GO

PRINT '? Registro con ID = 2 actualizado: "supervisor" ? "Supervisor"';
GO

-- Verificar cambio
SELECT [ID], [Detalle] 
FROM [dbo].[Rol] 
WHERE [ID] = 2;
GO

-- =============================================
-- PUNTO 7: BORRAR EN LA TABLA ESTADO
-- Eliminar el registro con ID = 3 ('Temporal')
-- =============================================

PRINT '';
PRINT 'Punto 7: Eliminando registro en Estado...';

DELETE FROM [dbo].[Estado]
WHERE [ID] = 3;
GO

PRINT '? Registro con ID = 3 eliminado ("Temporal")';
GO

-- Verificar eliminación
SELECT [ID], [Detalle] 
FROM [dbo].[Estado];
GO

-- =============================================
-- PUNTO 8: CONSULTA EN LA TABLA ROL
-- Obtener el Detalle del registro con ID = 4
-- =============================================

PRINT '';
PRINT 'Punto 8: Consultando registro en Rol con ID = 4...';

SELECT [Detalle]
FROM [dbo].[Rol]
WHERE [ID] = 4;
GO

PRINT '? Consulta ejecutada correctamente';
GO

-- =============================================
-- RESUMEN FINAL
-- =============================================

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
GO
