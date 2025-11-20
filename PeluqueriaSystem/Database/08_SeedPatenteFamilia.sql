-- =============================================
-- Script: Datos Iniciales para Patentes y Familias
-- Descripción: Inserta datos de ejemplo en Opciones, Familia y FamiliaElemento
-- Parte 6 - Usuario, Familia y Patente
-- =============================================

USE [PeluSystem];
GO

-- =============================================
-- 1. INSERTAR PATENTES (Opciones)
-- =============================================
PRINT 'Insertando patentes iniciales...';

-- Patentes para módulo de Usuarios
INSERT INTO [dbo].[Opciones] ([Nombre]) VALUES ('Usuario_Alta');
INSERT INTO [dbo].[Opciones] ([Nombre]) VALUES ('Usuario_Baja');
INSERT INTO [dbo].[Opciones] ([Nombre]) VALUES ('Usuario_Modificar');
INSERT INTO [dbo].[Opciones] ([Nombre]) VALUES ('Usuario_Consultar');

-- Patentes para módulo de Clientes
INSERT INTO [dbo].[Opciones] ([Nombre]) VALUES ('Cliente_Alta');
INSERT INTO [dbo].[Opciones] ([Nombre]) VALUES ('Cliente_Baja');
INSERT INTO [dbo].[Opciones] ([Nombre]) VALUES ('Cliente_Modificar');
INSERT INTO [dbo].[Opciones] ([Nombre]) VALUES ('Cliente_Consultar');

-- Patentes para módulo de Turnos
INSERT INTO [dbo].[Opciones] ([Nombre]) VALUES ('Turno_Alta');
INSERT INTO [dbo].[Opciones] ([Nombre]) VALUES ('Turno_Baja');
INSERT INTO [dbo].[Opciones] ([Nombre]) VALUES ('Turno_Modificar');
INSERT INTO [dbo].[Opciones] ([Nombre]) VALUES ('Turno_Consultar');

-- Patentes para módulo de Reportes
INSERT INTO [dbo].[Opciones] ([Nombre]) VALUES ('Reporte_Ventas');
INSERT INTO [dbo].[Opciones] ([Nombre]) VALUES ('Reporte_Turnos');
INSERT INTO [dbo].[Opciones] ([Nombre]) VALUES ('Reporte_Clientes');

-- Patentes administrativas
INSERT INTO [dbo].[Opciones] ([Nombre]) VALUES ('Sistema_Configuracion');
INSERT INTO [dbo].[Opciones] ([Nombre]) VALUES ('Sistema_Backup');
INSERT INTO [dbo].[Opciones] ([Nombre]) VALUES ('Sistema_Restore');

PRINT '✓ Patentes insertadas correctamente';
GO

-- =============================================
-- 2. INSERTAR FAMILIAS
-- =============================================
PRINT 'Insertando familias iniciales...';

-- Familias por módulo
INSERT INTO [dbo].[Familia] ([Nombre]) VALUES ('Familia_Usuarios');
INSERT INTO [dbo].[Familia] ([Nombre]) VALUES ('Familia_Clientes');
INSERT INTO [dbo].[Familia] ([Nombre]) VALUES ('Familia_Turnos');
INSERT INTO [dbo].[Familia] ([Nombre]) VALUES ('Familia_Reportes');
INSERT INTO [dbo].[Familia] ([Nombre]) VALUES ('Familia_Sistema');

-- Familias por rol
INSERT INTO [dbo].[Familia] ([Nombre]) VALUES ('Rol_Empleado');
INSERT INTO [dbo].[Familia] ([Nombre]) VALUES ('Rol_Supervisor');
INSERT INTO [dbo].[Familia] ([Nombre]) VALUES ('Rol_Administrador');

PRINT '✓ Familias insertadas correctamente';
GO

-- =============================================
-- 3. ASIGNAR PATENTES A FAMILIAS
-- =============================================
PRINT 'Asignando patentes a familias...';

-- Familia Usuarios: todas las patentes de usuario (P = Patente)
INSERT INTO [dbo].[FamiliaElemento] ([IDFamilia], [IDElemento], [TipoElemento]) 
VALUES (1, 1, 'P'), (1, 2, 'P'), (1, 3, 'P'), (1, 4, 'P');

-- Familia Clientes: todas las patentes de cliente
INSERT INTO [dbo].[FamiliaElemento] ([IDFamilia], [IDElemento], [TipoElemento]) 
VALUES (2, 5, 'P'), (2, 6, 'P'), (2, 7, 'P'), (2, 8, 'P');

-- Familia Turnos: todas las patentes de turno
INSERT INTO [dbo].[FamiliaElemento] ([IDFamilia], [IDElemento], [TipoElemento]) 
VALUES (3, 9, 'P'), (3, 10, 'P'), (3, 11, 'P'), (3, 12, 'P');

-- Familia Reportes: todas las patentes de reporte
INSERT INTO [dbo].[FamiliaElemento] ([IDFamilia], [IDElemento], [TipoElemento]) 
VALUES (4, 13, 'P'), (4, 14, 'P'), (4, 15, 'P');

-- Familia Sistema: patentes administrativas
INSERT INTO [dbo].[FamiliaElemento] ([IDFamilia], [IDElemento], [TipoElemento]) 
VALUES (5, 16, 'P'), (5, 17, 'P'), (5, 18, 'P');

PRINT '✓ Patentes asignadas a familias correctamente';
GO

-- =============================================
-- 4. ASIGNAR FAMILIAS A ROLES (COMPOSITE)
-- =============================================
PRINT 'Asignando familias a roles...';

-- Rol Empleado: Clientes y Turnos (F = Familia)
INSERT INTO [dbo].[FamiliaElemento] ([IDFamilia], [IDElemento], [TipoElemento]) 
VALUES (6, 2, 'F'), (6, 3, 'F');

-- Rol Supervisor: Empleado + Reportes
INSERT INTO [dbo].[FamiliaElemento] ([IDFamilia], [IDElemento], [TipoElemento]) 
VALUES (7, 6, 'F'), (7, 4, 'F');

-- Rol Administrador: Supervisor + Usuarios + Sistema
INSERT INTO [dbo].[FamiliaElemento] ([IDFamilia], [IDElemento], [TipoElemento]) 
VALUES (8, 7, 'F'), (8, 1, 'F'), (8, 5, 'F');

PRINT '✓ Familias asignadas a roles correctamente';
GO

-- =============================================
-- RESUMEN
-- =============================================
PRINT '';
PRINT '========================================';
PRINT '✓ DATOS INICIALES INSERTADOS';
PRINT '========================================';
PRINT '';

-- Mostrar resumen
SELECT 'Opciones (Patentes)' AS Tabla, COUNT(*) AS Total FROM [dbo].[Opciones]
UNION ALL
SELECT 'Familia' AS Tabla, COUNT(*) AS Total FROM [dbo].[Familia]
UNION ALL
SELECT 'FamiliaElemento' AS Tabla, COUNT(*) AS Total FROM [dbo].[FamiliaElemento];

PRINT '';
PRINT '========================================';
GO
