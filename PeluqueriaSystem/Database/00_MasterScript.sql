-- =============================================
-- Script Maestro: Ejecución Completa
-- Descripción: Ejecuta todos los scripts en el orden correcto
-- Uso: Ejecutar este script para crear la BD completa
-- =============================================

PRINT '========================================';
PRINT 'SISTEMA DE GESTIÓN PELUQUERÍA';
PRINT 'Script de Creación de Base de Datos';
PRINT '========================================';
PRINT '';
GO

-- =============================================
-- PASO 1: CREAR BASE DE DATOS
-- =============================================
PRINT '>>> PASO 1: CREANDO BASE DE DATOS...';
PRINT '';

:r 01_CreateDatabase.sql

PRINT '';
PRINT '========================================';
GO

-- =============================================
-- PASO 2: CREAR TABLAS
-- =============================================
PRINT '>>> PASO 2: CREANDO TABLAS...';
PRINT '';

:r 02_CreateTables.sql

PRINT '';
PRINT '========================================';
GO

-- =============================================
-- PASO 3: CREAR RELACIONES (FOREIGN KEYS)
-- =============================================
PRINT '>>> PASO 3: CREANDO RELACIONES...';
PRINT '';

:r 03_CreateForeignKeys.sql

PRINT '';
PRINT '========================================';
GO

-- =============================================
-- PASO 4: INSERTAR DATOS INICIALES
-- =============================================
PRINT '>>> PASO 4: INSERTANDO DATOS INICIALES...';
PRINT '';

:r 04_SeedData.sql

PRINT '';
PRINT '========================================';
GO

-- =============================================
-- PASO 5: OPERACIONES CRUD (PUNTOS 6, 7, 8)
-- =============================================
PRINT '>>> PASO 5: EJECUTANDO OPERACIONES CRUD...';
PRINT '';

:r 05_CRUDOperations.sql

PRINT '';
PRINT '========================================';
GO

-- =============================================
-- PASO 6: EJEMPLOS EN TABLA USUARIO
-- =============================================
PRINT '>>> PASO 6: EJECUTANDO EJEMPLOS EN USUARIO...';
PRINT '';

:r 06_UsuarioExamples.sql

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
PRINT 'Base de datos: PeluqueriaSystem';
PRINT 'Tablas creadas: Rol, Estado, Usuario';
PRINT 'Relaciones: FK_Usuario_Rol, FK_Usuario_Estado';
PRINT '';
PRINT 'Estado de las tablas:';
PRINT '  - Rol: 4 registros (Administrador, Supervisor, Peluquero, Cajero)';
PRINT '  - Estado: 2 registros (Habilitado, Baja)';
PRINT '  - Usuario: 0 registros (tabla vacía tras ejemplos)';
PRINT '';
PRINT '========================================';
GO
