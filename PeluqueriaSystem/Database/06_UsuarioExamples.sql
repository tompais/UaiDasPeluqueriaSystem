-- =============================================
-- Script: Ejemplos de Operaciones en Usuario
-- Descripci�n: Punto 3 del documento - Ejemplos CRUD en Usuario
-- =============================================

USE [PeluSystem];
GO

-- =============================================
-- EJEMPLOS DE OPERACIONES EN USUARIO
-- =============================================

PRINT 'Ejecutando ejemplos de operaciones en Usuario...';
PRINT '';

-- =============================================
-- INSERT: Insertar usuario de ejemplo
-- =============================================

PRINT '1. INSERT - Insertando usuario de ejemplo...';

-- Limpiar tabla si tiene datos
DELETE FROM [dbo].[Usuario];
DBCC CHECKIDENT ('[dbo].[Usuario]', RESEED, 0);
GO

INSERT INTO [dbo].[Usuario] (
    [Apellido], [Nombre], [Email], [Rol], [Estado], [Clave], [DV]
) VALUES (
    'Lennon',
    'Jhon',
    'jlennon@lennon.com',
    1,  -- Rol: Administrador
    1,  -- Estado: Habilitado
    '1234',
    '43534h5jk43h5'
);
GO

PRINT '? Usuario insertado con ID = 1';
GO

-- =============================================
-- SELECT: Consultar todos los usuarios
-- =============================================

PRINT '';
PRINT '2. SELECT - Consultando todos los usuarios...';

SELECT * FROM [dbo].[Usuario];
GO

-- =============================================
-- SELECT: Consultar usuario por ID
-- =============================================

PRINT '';
PRINT '3. SELECT - Consultando usuario con ID = 1...';

SELECT * FROM [dbo].[Usuario] WHERE [ID] = 1;
GO

-- =============================================
-- UPDATE: Actualizar el email
-- =============================================

PRINT '';
PRINT '4. UPDATE - Actualizando email del usuario...';

UPDATE [dbo].[Usuario]
SET [Email] = 'JLennon@Lennon.com.ar'
WHERE [ID] = 1;
GO

PRINT '? Email actualizado: "jlennon@lennon.com" ? "JLennon@Lennon.com.ar"';
GO

-- Verificar actualizaci�n
SELECT [ID], [Nombre], [Apellido], [Email]
FROM [dbo].[Usuario]
WHERE [ID] = 1;
GO

-- =============================================
-- DELETE: Eliminar usuario
-- =============================================

PRINT '';
PRINT '5. DELETE - Eliminando usuario con ID = 1...';

DELETE FROM [dbo].[Usuario]
WHERE [ID] = 1;
GO

PRINT '? Usuario eliminado';
GO

-- Verificar eliminaci�n
SELECT COUNT(*) AS 'Total Usuarios' FROM [dbo].[Usuario];
GO

PRINT '';
PRINT '? Todas las operaciones CRUD ejecutadas correctamente';
GO
