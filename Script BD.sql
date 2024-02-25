-- Creacion de la base de datos
CREATE DATABASE TestMagnetron;
GO

-- Usar la base de datos creada
USE TestMagnetron;
GO

-- Creacion de tablas: 
CREATE TABLE TiposDocumento (
	Id INT PRIMARY KEY NOT NULL,
	Descripcion VARCHAR(30) NOT NULL
);

CREATE TABLE Persona (
    Per_ID INT PRIMARY KEY IDENTITY NOT NULL,
    Per_Nombre VARCHAR(50),
	Per_Apellido VARCHAR(50),
	Per_TipoDocumento INT NOT NULL,
	Per_Documento VARCHAR(20) NOT NULL,
    FOREIGN KEY (Per_TipoDocumento) REFERENCES TiposDocumento(Id)
);
GO

CREATE TABLE Producto (
    Prod_ID INT PRIMARY KEY IDENTITY NOT NULL,
	Prod_Nombre VARCHAR(100) NOT NULL,
    Prod_Descripcion VARCHAR(500) NOT NULL,
	Prod_Precio DECIMAL(10,2) NOT NULL,
	Prod_Costo DECIMAL(10,2) NOT NULL,
	Prod_UM VARCHAR(50) NOT NULL
);
GO

CREATE TABLE Fact_Encabezado (
    FEnc_ID INT PRIMARY KEY IDENTITY NOT NULL,
    FEnc_Numero VARCHAR(20) NOT NULL,
	FEnc_Fecha DATETIME NOT NULL,
	FEnc_Per_ID INT NOT NULL,
    FOREIGN KEY (FEnc_Per_ID) REFERENCES Persona(Per_ID)
);
GO

CREATE TABLE Fact_Detalle (
    FDet_ID INT PRIMARY KEY IDENTITY NOT NULL,
    FDet_Linea VARCHAR(50) NOT NULL,
	FDet_Cantidad INT NOT NULL,
	FDet_Prod_ID INT NOT NULL,
	FDet_FEnc_ID INT NOT NULL,
    FOREIGN KEY (FDet_Prod_ID) REFERENCES Producto(Prod_ID),
	FOREIGN KEY (FDet_FEnc_ID) REFERENCES Fact_Encabezado(FEnc_ID)
);
GO

-- Creacion de vistas
-- 3A TOTAL FACTURADO POR PERSONA
CREATE VIEW Vista_TotalFacturado AS
SELECT P.Per_ID, TD.Descripcion, P.Per_Documento, P.Per_Nombre + ' ' + P.Per_Apellido AS Cliente, ISNULL(SUM(FD.FDet_Cantidad * PR.Prod_Precio), 0) AS TotalFacturado
FROM Persona P
LEFT JOIN Fact_Encabezado F ON P.Per_ID = F.FEnc_Per_ID
LEFT JOIN Fact_Detalle FD ON FD.FDet_FEnc_ID = F.FEnc_ID
LEFT JOIN Producto PR ON PR.Prod_ID = FD.FDet_Prod_ID
LEFT JOIN TiposDocumento TD ON P.Per_TipoDocumento = TD.Id
GROUP BY P.Per_ID, TD.Descripcion, P.Per_Documento, P.Per_Nombre, P.Per_Apellido;
GO

-- 3B PERSONA CON PRODUCTO MAS CARO
CREATE VIEW Vista_PersonaProductoMasCaro AS
SELECT TOP (1) P.Per_ID, TD.Descripcion, P.Per_Documento, P.Per_Nombre + ' ' + P.Per_Apellido AS Cliente, PR.Prod_Nombre AS Producto, PR.Prod_Precio AS PrecioProducto --Se usa TIES para obtener mas de 1 registro (TOP 1) que tenga la puntuacion mas alta
FROM Persona P
INNER JOIN Fact_Encabezado F ON P.Per_ID = F.FEnc_Per_ID
INNER JOIN Fact_Detalle FD ON FD.FDet_FEnc_ID = F.FEnc_ID
INNER JOIN Producto PR ON PR.Prod_ID = FD.FDet_Prod_ID
LEFT JOIN TiposDocumento TD ON P.Per_TipoDocumento = TD.Id
ORDER BY PR.Prod_Precio DESC;
GO

-- 3C PRODUCTOS SEGUN CANTIDAD FACTURADA 
CREATE VIEW Vista_ProductosPorCantidadFacturada AS
SELECT TOP 100 PERCENT PR.Prod_ID, PR.Prod_Nombre, SUM(FD.FDet_Cantidad) AS CantidadFacturada
FROM Producto PR
LEFT JOIN Fact_Detalle FD ON FD.FDet_Prod_ID = PR.Prod_ID
GROUP BY PR.Prod_ID, PR.Prod_Nombre
ORDER BY CantidadFacturada DESC;
GO

-- 3D PRODUCTOS SEGÚN SU UTILIDAD GENERADOS POR FACTURACIÓN
CREATE VIEW Vista_UtilidadPorProducto AS
SELECT PR.Prod_ID, PR.Prod_Nombre, SUM(FD.FDet_Cantidad  * (PR.Prod_Precio - PR.Prod_Costo)) AS UtilidadGenerada
FROM Producto PR
LEFT JOIN Fact_Detalle FD ON FD.FDet_Prod_ID = PR.Prod_ID
GROUP BY PR.Prod_ID, PR.Prod_Nombre;
GO

-- 3E PRODUCTOS Y EL MARGEN DE GANANCIA DE CADA UNO SEGÚN SU FACTURACIÓN.
CREATE VIEW Vista_MargenGananciaPorProducto AS
SELECT PR.Prod_ID, PR.Prod_Nombre, SUM(((FD.FDet_Cantidad  * (PR.Prod_Precio - PR.Prod_Costo))/(FD.FDet_Cantidad * PR.Prod_Precio))) AS MargenGanancia
FROM Producto PR
LEFT JOIN Fact_Detalle FD ON FD.FDet_Prod_ID = PR.Prod_ID
GROUP BY PR.Prod_ID, PR.Prod_Nombre;
GO

--Agregar registros a la BD
INSERT INTO TiposDocumento (Id, Descripcion) VALUES
(1, 'DNI'),
(2, 'Pasaporte'),
(3, 'Cedula de Ciudadania'),
(4, 'Cedula de Extranjeria'),
(5, 'Tarjeta de Identidad'),
(6, 'Certificado de nacimiento')

INSERT INTO Persona (Per_Nombre, Per_Apellido, Per_TipoDocumento, Per_Documento) VALUES
('Juan', 'Pérez', 3, '12345678A'),
('María', 'González', 3, '87654321B'),
('Carlos', 'Rodríguez', 3, '98765432C'),
('Laura', 'Martínez', 3, '23456789D'),
('Pablo', 'Gómez', 4, '34567890E'),
('Ana', 'López', 5, '45678901F'),
('Luis', 'Sánchez', 3, '56789012G'),
('Marta', 'Díaz', 2, '67890123H'),
('Andrés', 'Fernández', 3, '78901234I'),
('Sofía', 'García', 3, '89012345J');

INSERT INTO Producto (Prod_Nombre, Prod_Descripcion, Prod_Precio, Prod_Costo, Prod_UM) VALUES
('Producto 1', 'Descripción del producto 1', 10.50, 5.20, 'KG'),
('Producto 2', 'Descripción del producto 2', 15.75, 7.80, 'CM'),
('Producto 3', 'Descripción del producto 3', 20.00, 9.90, 'LB'),
('Producto 4', 'Descripción del producto 4', 12.25, 6.00, 'LB'),
('Producto 5', 'Descripción del producto 5', 18.90, 8.50, 'LB'),
('Producto 6', 'Descripción del producto 6', 25.50, 11.20, 'KG'),
('Producto 7', 'Descripción del producto 7', 30.75, 14.60, 'KG'),
('Producto 8', 'Descripción del producto 8', 22.80, 10.75, 'MTS'),
('Producto 9', 'Descripción del producto 9', 35.25, 17.00, 'CM'),
('Producto 10', 'Descripción del producto 10', 40.00, 20.50, 'LB');

INSERT INTO Fact_Encabezado (FEnc_Numero, FEnc_Fecha, FEnc_Per_ID) VALUES
('001', DATEADD(DAY, -1, GETDATE()), 1),
('002', DATEADD(DAY, -2, GETDATE()), 1),
('003', DATEADD(DAY, -3, GETDATE()), 3),
('004', DATEADD(DAY, -3, GETDATE()), 2),
('005', DATEADD(DAY, -5, GETDATE()), 5),
('006', DATEADD(DAY, -2, GETDATE()), 5),
('007', DATEADD(DAY, -3, GETDATE()), 9),
('008', DATEADD(DAY, -1, GETDATE()), 8),
('009', DATEADD(DAY, -1, GETDATE()), 9),
('010', DATEADD(DAY, -5, GETDATE()), 10),
('011', DATEADD(DAY, -1, GETDATE()), 4),
('012', DATEADD(DAY, -2, GETDATE()), 2),
('013', DATEADD(DAY, -3, GETDATE()), 3),
('014', DATEADD(DAY, -3, GETDATE()), 4),
('015', DATEADD(DAY, -5, GETDATE()), 5),
('016', DATEADD(DAY, -2, GETDATE()), 6),
('017', DATEADD(DAY, -3, GETDATE()), 7),
('018', DATEADD(DAY, -1, GETDATE()), 7),
('019', DATEADD(DAY, -1, GETDATE()), 6),
('020', DATEADD(DAY, -5, GETDATE()), 10);

INSERT INTO Fact_Detalle (FDet_Linea, FDet_Cantidad, FDet_Prod_ID, FDet_FEnc_ID) VALUES
('Linea 1', 1, 1, 1),
('Linea 2', 2, 2, 2),
('Linea 3', 3, 4, 3),
('Linea 4', 4, 4, 4),
('Linea 5', 5, 5, 5),
('Linea 6', 6, 8, 6),
('Linea 7', 7, 7, 7),
('Linea 8', 8, 8, 8),
('Linea 9', 9, 8, 9),
('Linea 10', 10, 10, 10),
('Linea 11', 1, 3, 11),
('Linea 12', 2, 3, 12),
('Linea 13', 3, 3, 13),
('Linea 14', 4, 1, 14),
('Linea 15', 5, 6, 15),
('Linea 16', 6, 6, 16),
('Linea 17', 7, 5, 17),
('Linea 18', 8, 8, 18),
('Linea 19', 9, 9, 19),
('Linea 20', 10, 10, 20);
