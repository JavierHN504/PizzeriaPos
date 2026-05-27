USE PizzeriaPosDB;
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Usuarios' AND xtype='U')
BEGIN
    CREATE TABLE Usuarios (
        Id              INT IDENTITY(1,1) PRIMARY KEY,
        NombreUsuario   NVARCHAR(50)    NOT NULL UNIQUE,
        NombreCompleto  NVARCHAR(100)   NOT NULL,
        PasswordHash    NVARCHAR(MAX)   NOT NULL,
        Activo          BIT             NOT NULL DEFAULT 1,
        CreatedAt       DATETIME        NOT NULL DEFAULT GETDATE(),
        UpdatedAt       DATETIME        NULL,
        Deleted         BIT             NOT NULL DEFAULT 0
    );
END
GO