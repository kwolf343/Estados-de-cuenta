CREATE DATABASE estadosCuenta;
GO

USE estadosCuenta;
GO

CREATE TABLE estadoCuenta (
    id INT PRIMARY KEY IDENTITY(1,1),
    NumeroTarjeta VARCHAR(20) NOT NULL,
    Nombres VARCHAR(100) NOT NULL,
    Apellidos VARCHAR(100) NOT NULL,
    Cuenta INT NOT NULL,
    Limite DECIMAL(18, 2) NOT NULL,
    Status INT NOT NULL CHECK (Status IN (0, 1)),
    PorcentajeInteresConfigurable DECIMAL(18, 2) NOT NULL,
    PorcentajeConfigurableSaldoMinimo DECIMAL(18, 2) NOT NULL
);

CREATE TABLE detalleEstadoCuenta (
    id INT PRIMARY KEY IDENTITY(1,1),
    idEstadoCuenta INT NOT NULL,
    Descripcion VARCHAR(200) NOT NULL,
    Fecha DATE NOT NULL,
    Monto DECIMAL(18, 2) NOT NULL,
    Accion INT NOT NULL CHECK (Accion IN (1, 2)),
    NumAutorizacion INT NOT NULL
);

ALTER TABLE detalleEstadoCuenta
ADD CONSTRAINT FK_estadoCuenta_detalleEstadoCuenta FOREIGN KEY (idEstadoCuenta)
REFERENCES estadoCuenta(id);
GO

-- Procedimientos almacenados --

-- Inserts
CREATE PROCEDURE InsertarEstadoCuenta
    @NumeroTarjeta VARCHAR(20),
    @Nombres VARCHAR(100),
    @Apellidos VARCHAR(100),
    @Cuenta INT,
    @Limite DECIMAL(18, 2),
    @Status INT,
    @PorcentajeInteresConfigurable DECIMAL(18, 2),
    @PorcentajeConfigurableSaldoMinimo DECIMAL(18, 2)
AS
BEGIN
    INSERT INTO estadoCuenta (NumeroTarjeta, Nombres, Apellidos, Cuenta, Limite, Status, PorcentajeInteresConfigurable, PorcentajeConfigurableSaldoMinimo)
    VALUES (@NumeroTarjeta, @Nombres, @Apellidos, @Cuenta, @Limite, @Status, @PorcentajeInteresConfigurable, @PorcentajeConfigurableSaldoMinimo);

    SELECT SCOPE_IDENTITY() AS Id;
END;
GO

CREATE PROCEDURE InsertarDetalleEstadoCuenta
    @idEstadoCuenta INT,
    @Descripcion VARCHAR(200),
    @Fecha DATE,
    @Monto DECIMAL(18, 2),
    @Accion INT,
    @NumAutorizacion INT
AS
BEGIN
    INSERT INTO detalleEstadoCuenta (idEstadoCuenta, Descripcion, Fecha, Monto, Accion, NumAutorizacion)
    VALUES (@idEstadoCuenta, @Descripcion, @Fecha, @Monto, @Accion, @NumAutorizacion);

    SELECT SCOPE_IDENTITY() AS Id;
END;
GO

-- Selects
CREATE PROCEDURE SeleccionarTodosEstadoCuenta
AS
BEGIN
    SELECT 
        ec.id,
        ec.NumeroTarjeta,
        ec.Nombres,
        ec.Apellidos,
        ec.Cuenta,
        ec.Limite,
        ec.Status,
        COALESCE((
            SELECT 
                SUM(CASE WHEN dec.Accion = 1 THEN dec.Monto ELSE 0 END) - 
                SUM(CASE WHEN dec.Accion = 2 THEN dec.Monto ELSE 0 END)
            FROM 
                detalleEstadoCuenta dec
            WHERE 
                dec.idEstadoCuenta = ec.id
        ), 0) AS Saldo
    FROM 
        estadoCuenta ec;
END;
GO

CREATE PROCEDURE SeleccionarEstadoCuentaPorID
    @id INT
AS
BEGIN
    SELECT 
        ec.id,
        ec.NumeroTarjeta,
        ec.Nombres,
        ec.Apellidos,
        ec.Cuenta,
        ec.Limite,
        ec.Status,
        COALESCE((
            SELECT 
                SUM(CASE WHEN dec.Accion = 1 THEN dec.Monto ELSE 0 END) - 
                SUM(CASE WHEN dec.Accion = 2 THEN dec.Monto ELSE 0 END)
            FROM 
                detalleEstadoCuenta dec
            WHERE 
                dec.idEstadoCuenta = ec.id
        ), 0) AS Saldo
    FROM 
        estadoCuenta ec
    WHERE
        ec.id = @id;
END;
GO

CREATE PROCEDURE SeleccionarTodosDetalleEstadoCuenta
AS
BEGIN
    SELECT * FROM detalleEstadoCuenta;
END;
GO

CREATE PROCEDURE SeleccionarDetalleEstadoCuentaPorIDEstadoCuenta
    @EstadoCuentaID INT
AS
BEGIN
    SELECT * FROM detalleEstadoCuenta WHERE idEstadoCuenta = @EstadoCuentaID;
END;
GO

-- Inserts de estados de cuenta de ejemplo
EXEC InsertarEstadoCuenta 
    @NumeroTarjeta = '1345 **** **** 6788',
    @Nombres = 'Juan',
    @Apellidos = 'P�rez',
    @Cuenta = 123456789,
    @Limite = 5000.00,
    @Status = 1,
    @PorcentajeInteresConfigurable = 0.15,
    @PorcentajeConfigurableSaldoMinimo = 0.02;

EXEC InsertarEstadoCuenta 
    @NumeroTarjeta = '4896 **** **** 5568',
    @Nombres = 'David',
    @Apellidos = 'Castro',
    @Cuenta = 567894556,
    @Limite = 2000.00,
    @Status = 1,
    @PorcentajeInteresConfigurable = 0.25,
    @PorcentajeConfigurableSaldoMinimo = 0.05;