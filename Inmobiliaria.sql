CREATE DATABASE IF NOT EXISTS `inmobiliaria`;

USE `inmobiliaria`;



CREATE TABLE IF NOT EXISTS `inquilinos` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `DNI` varchar(50) NOT NULL DEFAULT '',
  `Nombre` varchar(50) NOT NULL DEFAULT '',
  `Apellido` varchar(50) NOT NULL DEFAULT '',
  `Telefono` varchar(50) DEFAULT '',
  `Email` varchar(50) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=latin1;

-- Volcando datos para la tabla inmobiliaria.inquilinos: ~2 rows (aproximadamente)
INSERT INTO `inquilinos` (`Id`, `DNI`, `Nombre`, `Apellido`, `Telefono`, `Email`) VALUES
	(10, '30111222', 'Primero', 'Inquilino', '1111', 'inquilino1@mail.com'),
	(11, '33000111', 'Segundo', 'Inquinillo', '22222', 'inquilino2@mail.com');

CREATE TABLE IF NOT EXISTS `propietarios` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `DNI` varchar(50) NOT NULL DEFAULT '0',
  `Nombre` varchar(50) NOT NULL DEFAULT '0',
  `Apellido` varchar(50) NOT NULL DEFAULT '0',
  `Telefono` varchar(50)  DEFAULT '0',
  `Email` varchar(50) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=34 DEFAULT CHARSET=latin1;

-- Volcando datos para la tabla inmobiliaria.propietarios: ~2 rows (aproximadamente)
INSERT INTO `propietarios` (`Id`, `DNI`, `Nombre`, `Apellido`, `Telefono`, `Email`, `Clave`) VALUES
	('42','35456987', 'Josesito', 'Perez', '265712354', 'josesito1@mail.com', NULL),
	('43','36987456', 'Pepito ', 'Calavera', '266489654', 'pepito1@mail.com', NULL);

CREATE TABLE IF NOT EXISTS `inmuebles` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Direccion` varchar(50) NOT NULL,
  `Ambientes` int(11) NOT NULL,
  `Superficie` int(11) NOT NULL,
  `Latitud` decimal(20,6) NOT NULL,
  `Longitud` decimal(20,6) NOT NULL,
  `PropietarioId` int(11) NOT NULL,
  `Tipo` varchar(50) DEFAULT NULL,
  `Precio` decimal(20,6) DEFAULT NULL,
  `Estado` int(11) NOT NULL DEFAULT 1,
  `Uso` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_inmuebles_propietarios` (`PropietarioId`),
  CONSTRAINT `FK_inmuebles_propietarios` FOREIGN KEY (`PropietarioId`) REFERENCES `propietarios` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=45 DEFAULT CHARSET=latin1;

-- Volcando datos para la tabla
INSERT INTO `inmuebles` (`Id`, `Direccion`, `Ambientes`, `Superficie`, `Latitud`, `Longitud`, `PropietarioId`, `Tipo`, `Precio`, `Estado`, `Uso`) VALUES
	(10, 'Mirador 1 C 22', 4, 70, -33.257433, -66.334202, 42, 'Casa', 60000, 1, 'Residencial'),
	(11, 'Mirador 2 C 10', 3, 60, -33.258196,  -66.334191, 43, 'Departamento', 50000, 1, 'Comercial');


-- Volcando estructura para tabla inmobiliaria.contratos
CREATE TABLE IF NOT EXISTS `contratos` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `FechaInicio` datetime NOT NULL,
  `FechaFin` datetime DEFAULT NULL,
  `Precio` decimal(20,6) NOT NULL,
  `InquilinoId` int(11) NOT NULL,
  `InmuebleId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_contratos_inquilinos` (`InquilinoId`),
  KEY `FK_contratos_inmuebles` (`InmuebleId`),
  CONSTRAINT `FK_contratos_inmuebles` FOREIGN KEY (`InmuebleId`) REFERENCES `inmuebles` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_contratos_inquilinos` FOREIGN KEY (`InquilinoId`) REFERENCES `inquilinos` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=53 DEFAULT CHARSET=latin1;


-- Volcando estructura para tabla inmobiliaria.pagos
CREATE TABLE IF NOT EXISTS `pagos` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Mes` int(11) NOT NULL,
  `FechaPagado` datetime DEFAULT NULL,
  `ContratoId` int(11) NOT NULL,
  `Importe` decimal(20,6) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_pagos_contratos` (`ContratoId`),
  CONSTRAINT `FK_pagos_contratos` FOREIGN KEY (`ContratoId`) REFERENCES `contratos` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=113 DEFAULT CHARSET=latin1;


-- Volcando estructura para tabla inmobiliaria.usuarios
CREATE TABLE IF NOT EXISTS `usuarios` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Rol` int(11) NOT NULL,
  `Nombre` varchar(50) DEFAULT NULL,
  `Apellido` varchar(50) DEFAULT NULL,
  `Email` varchar(50) DEFAULT NULL,
  `Clave` varchar(50) DEFAULT NULL,
  `Avatar` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=latin1;

INSERT INTO usuarios (`Id`, `Rol`, `Nombre`, `Apellido`, `Email`, `Clave`, `Avatar`)
VALUES
(1, 1, 'super', 'admin', 'super@admin.com', 'E/zh+45aglQiblqcon2qUcgvgXVFTCTD0TMjxgKd+Jw=', '/Uploads/avatar_1e3242c18-0e60-4e54-90ec-14992097b3a0.jpg'),
(2, 2, 'admin', 'administrador', 'admin@admin.com', '6ugOPWwpWbTbGkQ8fBkqSfLLZYEHnP3rQeGMoU19X2c=', '/Uploads/avatar_2a83acc33-78ad-4c0c-97d4-d812630dd128.jpg'),
(18, 0, 'empleado', 'user', 'user@user.com', '+4cWislMwro46T/A/NDTcbi1PKD9b2O/KsvJE2NMykg=', '/Uploads/sinFoto.png');
