-- =============================================
-- Script: Creación de Base de Datos PeluqueriaSystem
-- Descripción: Script idempotente para crear la base de datos
-- Autor: Sistema de Gestión Peluquería
-- Fecha: 2024
-- =============================================

USE master;
GO

-- Verificar si la base de datos existe y eliminarla
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'PeluqueriaSystem')
BEGIN
    ALTER DATABASE [PeluqueriaSystem] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [PeluqueriaSystem];
    PRINT '? Base de datos PeluqueriaSystem eliminada correctamente';
END
GO

-- Crear la base de datos
CREATE DATABASE [PeluqueriaSystem];
GO

PRINT '? Base de datos PeluqueriaSystem creada correctamente';
GO

-- Usar la base de datos recién creada
USE [PeluqueriaSystem];
GO

PRINT '? Contexto cambiado a PeluqueriaSystem';
GO
