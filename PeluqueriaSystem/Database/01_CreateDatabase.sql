-- =============================================
-- Script: Creación de Base de Datos PeluSystem
-- Descripción: Script idempotente para crear la base de datos
-- Autor: Sistema de Gestión Peluquería
-- Fecha: 2024
-- =============================================

USE master;
GO

-- Verificar si la base de datos existe y eliminarla
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'PeluSystem')
BEGIN
    ALTER DATABASE [PeluSystem] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [PeluSystem];
    PRINT '? Base de datos PeluSystem eliminada correctamente';
END
GO

-- Crear la base de datos
CREATE DATABASE [PeluSystem];
GO

PRINT '? Base de datos PeluSystem creada correctamente';
GO

-- Usar la base de datos recién creada
USE [PeluSystem];
GO

PRINT '? Contexto cambiado a PeluSystem';
GO
