-- =============================================
-- Script 01: Creacion de Base de Datos y Tabla Productos
-- Sistema POS Pizzeria
-- =============================================

-- Crear la base de datos
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'PizzeriaPosDB')
BEGIN
    CREATE DATABASE PizzeriaPosDB;
END
GO

USE PizzeriaPosDB;
GO

-- Crear tabla Productos
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Productos' AND xtype='U')
BEGIN
    CREATE TABLE Productos (
        Id          INT IDENTITY(1,1) PRIMARY KEY,
        Nombre      NVARCHAR(100)   NOT NULL,
        Descripcion NVARCHAR(250)   NULL,
        Precio      DECIMAL(10,2)   NOT NULL,
        Categoria   NVARCHAR(50)    NOT NULL,
        Disponible  BIT             NOT NULL DEFAULT 1,
        CreatedAt   DATETIME        NOT NULL DEFAULT GETDATE(),
        UpdatedAt   DATETIME        NULL,
        Deleted     BIT             NOT NULL DEFAULT 0
    );
END
GO

-- Datos iniciales de ejemplo
INSERT INTO Productos (Nombre, Descripcion, Precio, Categoria, Disponible)
VALUES
    ('Pizza Normal',     'Pizza clasica con tomate y mozzarella', 150.00, 'Pizza',  1),
    ('Pizza Peperoni',   'Pizza con peperoni y queso',           175.00, 'Pizza',  1),
    ('Pizza Hawaiana',   'Pizza con jamon y pina',                160.00, 'Pizza',  1),
    ('Pizza BBQ',        'Pizza con pollo BBQ y cebolla',         180.00, 'Pizza',  1),
    ('Coca Cola 500ml',  'Refresco de cola',                       35.00, 'Bebida', 1),
    ('Agua 600ml',       'Agua purificada',                        20.00, 'Bebida', 1),
    ('Pan de Ajo',       'Pan tostado con ajo y mantequilla',      45.00, 'Extra',  1);
GO