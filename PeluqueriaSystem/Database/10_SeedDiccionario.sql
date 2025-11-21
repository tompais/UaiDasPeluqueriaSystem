-- =============================================
-- Script: Seed Data para Diccionario
-- Descripción: Inserta datos de ejemplo para traducciones
-- =============================================

USE [PeluSystem];
GO

PRINT 'Insertando datos de ejemplo en Diccionario...';

-- IDIdioma = 1: Español
-- IDIdioma = 2: Inglés
-- IDIdioma = 3: Portugués

-- Palabras comunes en Español
INSERT INTO [dbo].[Diccionario] ([IDIdioma], [PalabraOriginal], [PalabraIdioma]) VALUES
    (1, 'Usuario', 'Usuario'),
    (1, 'Nombre', 'Nombre'),
    (1, 'Apellido', 'Apellido'),
    (1, 'Email', 'Email'),
    (1, 'Clave', 'Clave'),
    (1, 'Estado', 'Estado'),
    (1, 'Rol', 'Rol'),
    (1, 'Activo', 'Activo'),
    (1, 'Inactivo', 'Inactivo'),
    (1, 'Guardar', 'Guardar'),
    (1, 'Cancelar', 'Cancelar'),
    (1, 'Aceptar', 'Aceptar'),
    (1, 'Eliminar', 'Eliminar'),
    (1, 'Modificar', 'Modificar'),
    (1, 'Nuevo', 'Nuevo'),
    (1, 'Buscar', 'Buscar'),
    (1, 'Administración', 'Administración'),
    (1, 'Usuarios', 'Usuarios');

-- Palabras comunes en Inglés
INSERT INTO [dbo].[Diccionario] ([IDIdioma], [PalabraOriginal], [PalabraIdioma]) VALUES
    (2, 'Usuario', 'User'),
    (2, 'Nombre', 'Name'),
    (2, 'Apellido', 'Last Name'),
    (2, 'Email', 'Email'),
    (2, 'Clave', 'Password'),
    (2, 'Estado', 'Status'),
    (2, 'Rol', 'Role'),
    (2, 'Activo', 'Active'),
    (2, 'Inactivo', 'Inactive'),
    (2, 'Guardar', 'Save'),
    (2, 'Cancelar', 'Cancel'),
    (2, 'Aceptar', 'Accept'),
    (2, 'Eliminar', 'Delete'),
    (2, 'Modificar', 'Modify'),
    (2, 'Nuevo', 'New'),
    (2, 'Buscar', 'Search'),
    (2, 'Administración', 'Administration'),
    (2, 'Usuarios', 'Users');

-- Palabras comunes en Portugués
INSERT INTO [dbo].[Diccionario] ([IDIdioma], [PalabraOriginal], [PalabraIdioma]) VALUES
    (3, 'Usuario', 'Usuário'),
    (3, 'Nombre', 'Nome'),
    (3, 'Apellido', 'Sobrenome'),
    (3, 'Email', 'Email'),
    (3, 'Clave', 'Senha'),
    (3, 'Estado', 'Estado'),
    (3, 'Rol', 'Função'),
    (3, 'Activo', 'Ativo'),
    (3, 'Inactivo', 'Inativo'),
    (3, 'Guardar', 'Salvar'),
    (3, 'Cancelar', 'Cancelar'),
    (3, 'Aceptar', 'Aceitar'),
    (3, 'Eliminar', 'Excluir'),
    (3, 'Modificar', 'Modificar'),
    (3, 'Nuevo', 'Novo'),
    (3, 'Buscar', 'Pesquisar'),
    (3, 'Administración', 'Administração'),
    (3, 'Usuarios', 'Usuários');

PRINT '✓ Datos de ejemplo insertados correctamente';
GO
