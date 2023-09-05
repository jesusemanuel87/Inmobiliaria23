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
  `Clave` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=34 DEFAULT CHARSET=latin1;

-- Volcando datos para la tabla inmobiliaria.propietarios: ~2 rows (aproximadamente)
INSERT INTO `propietarios` (`Id`, `DNI`, `Nombre`, `Apellido`, `Telefono`, `Email`, `Clave`) VALUES
	('40','35456987', 'Josesito', 'Perez', '265712354', 'josesito1@mail.com', NULL),
	('41','36987456', 'Pepito ', 'Calavera', '266489654', 'pepito1@mail.com', NULL);

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