-- =============================================
-- Script 02: Tablas de Clientes, Direcciones y Pedidos
-- Sistema POS Pizzeria
-- =============================================

USE PizzeriaPosDB;
GO

-- Crear tabla Clientes
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Clientes' AND xtype='U')
BEGIN
    CREATE TABLE Clientes (
        Id          INT IDENTITY(1,1) PRIMARY KEY,
        Nombre      NVARCHAR(100)   NOT NULL,
        Apellido    NVARCHAR(100)   NULL,
        Telefono    NVARCHAR(15)    NULL,
        Email       NVARCHAR(150)   NULL,
        CreatedAt   DATETIME        NOT NULL DEFAULT GETDATE(),
        UpdatedAt   DATETIME        NULL,
        Deleted     BIT             NOT NULL DEFAULT 0
    );
END
GO

-- Crear tabla Direcciones
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Direcciones' AND xtype='U')
BEGIN
    CREATE TABLE Direcciones (
        Id              INT IDENTITY(1,1) PRIMARY KEY,
        Calle           NVARCHAR(200)   NOT NULL,
        Ciudad          NVARCHAR(100)   NULL,
        Departamento    NVARCHAR(100)   NULL,
        CodigoPostal    NVARCHAR(20)    NULL,
        EsPrincipal     BIT             NOT NULL DEFAULT 0,
        ClienteId       INT             NOT NULL,
        CreatedAt       DATETIME        NOT NULL DEFAULT GETDATE(),
        UpdatedAt       DATETIME        NULL,
        Deleted         BIT             NOT NULL DEFAULT 0,
        CONSTRAINT FK_Direcciones_Clientes FOREIGN KEY (ClienteId) REFERENCES Clientes(Id)
    );
END
GO

-- Crear tabla PedidosCabecera
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PedidosCabecera' AND xtype='U')
BEGIN
    CREATE TABLE PedidosCabecera (
        Id              INT IDENTITY(1,1) PRIMARY KEY,
        ClienteId       INT             NOT NULL,
        DireccionId     INT             NULL,
        Estado          NVARCHAR(50)    NOT NULL DEFAULT 'Pendiente',
        Total           DECIMAL(10,2)   NOT NULL DEFAULT 0,
        Observaciones   NVARCHAR(250)   NULL,
        CreatedAt       DATETIME        NOT NULL DEFAULT GETDATE(),
        UpdatedAt       DATETIME        NULL,
        Deleted         BIT             NOT NULL DEFAULT 0,
        CONSTRAINT FK_Pedidos_Clientes   FOREIGN KEY (ClienteId)   REFERENCES Clientes(Id),
        CONSTRAINT FK_Pedidos_Direcciones FOREIGN KEY (DireccionId) REFERENCES Direcciones(Id)
    );
END
GO

-- Crear tabla PedidosDetalle
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PedidosDetalle' AND xtype='U')
BEGIN
    CREATE TABLE PedidosDetalle (
        Id                  INT IDENTITY(1,1) PRIMARY KEY,
        PedidoCabeceraId    INT             NOT NULL,
        ProductoId          INT             NOT NULL,
        Cantidad            INT             NOT NULL,
        PrecioUnitario      DECIMAL(10,2)   NOT NULL,
        CreatedAt           DATETIME        NOT NULL DEFAULT GETDATE(),
        UpdatedAt           DATETIME        NULL,
        Deleted             BIT             NOT NULL DEFAULT 0,
        CONSTRAINT FK_Detalle_Cabecera  FOREIGN KEY (PedidoCabeceraId) REFERENCES PedidosCabecera(Id),
        CONSTRAINT FK_Detalle_Productos FOREIGN KEY (ProductoId)        REFERENCES Productos(Id)
    );
END
GO

-- Datos iniciales de ejemplo
INSERT INTO Clientes (Nombre, Apellido, Telefono, Email)
VALUES
    ('Javier',  'Diaz',    '88993514', 'javierjosuediazrios26@gmail.com'),
    ('Maria',   'Lopez',   '99887766', 'maria@email.com'),
    ('Carlos',  'Gomez',   '77665544', 'carlos@email.com');
GO

INSERT INTO Direcciones (Calle, Ciudad, Departamento, EsPrincipal, ClienteId)
VALUES
    ('Barrio San Jose, Casa 15',        'El Paraiso', 'El Paraiso',        1, 1),
    ('Colonia Las Brisas, Bloque 3',    'Tegucigalpa', 'Francisco Morazan', 1, 2);
GO