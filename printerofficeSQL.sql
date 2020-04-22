-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema printeroffice
-- -----------------------------------------------------
DROP SCHEMA IF EXISTS `printeroffice` ;

-- -----------------------------------------------------
-- Schema printeroffice
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `printeroffice` DEFAULT CHARACTER SET utf8 ;
USE `printeroffice` ;

-- -----------------------------------------------------
-- Table `printeroffice`.`customers`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `printeroffice`.`customers` ;

CREATE TABLE IF NOT EXISTS `printeroffice`.`customers` (
  `tel` VARCHAR(16) NOT NULL,
  `lastName` VARCHAR(45) NOT NULL,
  `firstName` VARCHAR(45) NOT NULL,
  `middleName` VARCHAR(45) NULL DEFAULT '-',
  `birthdate` DATE NULL,
  `address` VARCHAR(200) NOT NULL,
  PRIMARY KEY (`tel`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `printeroffice`.`employees`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `printeroffice`.`employees` ;

CREATE TABLE IF NOT EXISTS `printeroffice`.`employees` (
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `lastName` VARCHAR(45) NOT NULL,
  `firstName` VARCHAR(45) NOT NULL,
  `middleName` VARCHAR(45) NOT NULL,
  `fired` TINYINT NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 2
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `printeroffice`.`statuses`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `printeroffice`.`statuses` ;

CREATE TABLE IF NOT EXISTS `printeroffice`.`statuses` (
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `title` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 4
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `printeroffice`.`orders`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `printeroffice`.`orders` ;

CREATE TABLE IF NOT EXISTS `printeroffice`.`orders` (
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `emplId` INT(11) NOT NULL,
  `custId` VARCHAR(16) NOT NULL,
  `datetime` DATETIME NOT NULL,
  `totalPrice` INT(11) NULL,
  `statusId` INT(11) NOT NULL DEFAULT 3,
  `comment` VARCHAR(255) NULL DEFAULT '-',
  PRIMARY KEY (`id`),
  INDEX `emplKey_idx` (`emplId` ASC) VISIBLE,
  INDEX `statusKey_idx` (`statusId` ASC) VISIBLE,
  INDEX `clientKey_idx` (`custId` ASC) VISIBLE,
  CONSTRAINT `clientKey`
    FOREIGN KEY (`custId`)
    REFERENCES `printeroffice`.`customers` (`tel`)
    ON UPDATE CASCADE,
  CONSTRAINT `statusKey`
    FOREIGN KEY (`statusId`)
    REFERENCES `printeroffice`.`statuses` (`id`),
  CONSTRAINT `вщсKey`
    FOREIGN KEY (`emplId`)
    REFERENCES `printeroffice`.`employees` (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 12
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `printeroffice`.`deviceTypes`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `printeroffice`.`deviceTypes` ;

CREATE TABLE IF NOT EXISTS `printeroffice`.`deviceTypes` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `title` VARCHAR(144) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `printeroffice`.`deviceManufacs`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `printeroffice`.`deviceManufacs` ;

CREATE TABLE IF NOT EXISTS `printeroffice`.`deviceManufacs` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `title` VARCHAR(50) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `printeroffice`.`devices`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `printeroffice`.`devices` ;

CREATE TABLE IF NOT EXISTS `printeroffice`.`devices` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `model` VARCHAR(144) NOT NULL,
  `deviceTypeId` INT NOT NULL,
  `deviceManufac` INT NOT NULL,
  `price` INT NOT NULL,
  `number` INT NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `printerKey_idx` (`deviceTypeId` ASC) VISIBLE,
  INDEX `devManKey_idx` (`deviceManufac` ASC) VISIBLE,
  CONSTRAINT `devTypeKey`
    FOREIGN KEY (`deviceTypeId`)
    REFERENCES `printeroffice`.`deviceTypes` (`id`)
    ON DELETE RESTRICT
    ON UPDATE CASCADE,
  CONSTRAINT `devManKey`
    FOREIGN KEY (`deviceManufac`)
    REFERENCES `printeroffice`.`deviceManufacs` (`id`)
    ON DELETE RESTRICT
    ON UPDATE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 89
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `printeroffice`.`servicesTypes`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `printeroffice`.`servicesTypes` ;

CREATE TABLE IF NOT EXISTS `printeroffice`.`servicesTypes` (
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `title` VARCHAR(144) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 17
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `printeroffice`.`orderitems`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `printeroffice`.`orderitems` ;

CREATE TABLE IF NOT EXISTS `printeroffice`.`orderitems` (
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `orderId` INT(11) NOT NULL,
  `deviceId` INT NOT NULL,
  `serviceId` INT NOT NULL,
  `statusId` INT NOT NULL DEFAULT 3,
  PRIMARY KEY (`id`),
  INDEX `itemId_idx` (`deviceId` ASC) VISIBLE,
  INDEX `orderKey` (`orderId` ASC) VISIBLE,
  INDEX `serviceId_idx` (`serviceId` ASC) VISIBLE,
  INDEX `statusListKey_idx` (`statusId` ASC) VISIBLE,
  CONSTRAINT `storeId`
    FOREIGN KEY (`deviceId`)
    REFERENCES `printeroffice`.`devices` (`id`)
    ON UPDATE CASCADE,
  CONSTRAINT `orderKey`
    FOREIGN KEY (`orderId`)
    REFERENCES `printeroffice`.`orders` (`id`)
    ON UPDATE CASCADE,
  CONSTRAINT `serviceId`
    FOREIGN KEY (`serviceId`)
    REFERENCES `printeroffice`.`servicesTypes` (`id`)
    ON DELETE RESTRICT
    ON UPDATE CASCADE,
  CONSTRAINT `statusListKey`
    FOREIGN KEY (`statusId`)
    REFERENCES `printeroffice`.`statuses` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
AUTO_INCREMENT = 39
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `printeroffice`.`supplies`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `printeroffice`.`supplies` ;

CREATE TABLE IF NOT EXISTS `printeroffice`.`supplies` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `datetime` DATETIME NULL,
  `deviceId` INT NOT NULL,
  `price` INT NOT NULL,
  `number` INT NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `deviceKeySup_idx` (`deviceId` ASC) VISIBLE,
  CONSTRAINT `deviceKeySup`
    FOREIGN KEY (`deviceId`)
    REFERENCES `printeroffice`.`devices` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

USE `printeroffice`;

DELIMITER $$

USE `printeroffice`$$
DROP TRIGGER IF EXISTS `printeroffice`.`orders_BEFORE_INSERT` $$
USE `printeroffice`$$
CREATE DEFINER = CURRENT_USER TRIGGER `printeroffice`.`orders_BEFORE_INSERT` BEFORE INSERT ON `orders` FOR EACH ROW
BEGIN
	DECLARE isEmplWork TINYINT;
    SELECT fired INTO isEmplWork FROM employees WHERE employees.id = NEW.emplId;
    
	SET NEW.datetime = NOW();
    
	IF isEmplWork = 1 THEN
		SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Уволенные сотрудники не могут выполнять заказы!';
	END IF;
END$$


USE `printeroffice`$$
DROP TRIGGER IF EXISTS `printeroffice`.`orders_BEFORE_UPDATE` $$
USE `printeroffice`$$
CREATE DEFINER = CURRENT_USER TRIGGER `printeroffice`.`orders_BEFORE_UPDATE` BEFORE UPDATE ON `orders` FOR EACH ROW
BEGIN
	DECLARE isEmplWork TINYINT;
    
    IF OLD.statusId = 3 THEN
		IF NEW.statusId = 1 or NEW.statusId = 2 THEN
			UPDATE orderitems SET statusId = NEW.statusId WHERE orderId = NEW.id;
		END IF;
	ELSE
		SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'После отмены или завершения изменение статуса услуги невозможно!';
    END IF;
    
    SET NEW.datetime = NOW();
    
	SELECT fired INTO isEmplWork FROM employees WHERE employees.id = NEW.emplId;
	IF isEmplWork = 1 THEN
		SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Уволенные сотрудники не могут выполнять заказы!';
	END IF;
END$$


USE `printeroffice`$$
DROP TRIGGER IF EXISTS `printeroffice`.`orderitems_BEFORE_INSERT` $$
USE `printeroffice`$$
CREATE DEFINER = CURRENT_USER TRIGGER `printeroffice`.`orderitems_BEFORE_INSERT` BEFORE INSERT ON `orderitems` FOR EACH ROW
BEGIN
	DECLARE numberOfDevices INT;
    SELECT devices.number INTO numberOfDevices FROM devices WHERE devices.id = NEW.deviceId; 
    
	IF NEW.serviceId = 1 or NEW.serviceId = 2 THEN
		UPDATE devices SET number = number + 1 WHERE devices.id = NEw.deviceId;
	ELSE 
		IF NEW.serviceId = 3 and numberOfDevices > 0 THEN
			UPDATE devices SET number = number - 1 WHERE devices.id = NEW.deviceId;
		ELSE
			SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Устройство отсутствует в наличии!';
        END IF;
    END IF;
END$$


USE `printeroffice`$$
DROP TRIGGER IF EXISTS `printeroffice`.`orderitems_BEFORE_UPDATE` $$
USE `printeroffice`$$
CREATE DEFINER = CURRENT_USER TRIGGER `printeroffice`.`orderitems_BEFORE_UPDATE` BEFORE UPDATE ON `orderitems` FOR EACH ROW
BEGIN
	IF NEW.serviceId != OLD.serviceId THEN
		SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Изменение типа услуги невозможно!';
	END IF;
    
	IF NEW.deviceId != OLD.deviceId THEN
		SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Изменение идентификатора устройства, по которому оказывается услуга, невозможно!';
	END IF;
    
    IF OLD.statusId = 3 THEN
		IF NEW.statusId = 1 or NEW.statusId = 2 THEN
			UPDATE devices SET number = number - 1 WHERE devices.id = OLD.deviceId;
		END IF;
	ELSE
		SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'После отмены или завершения изменение статуса услуги невозможно!';
    END IF;
END$$


USE `printeroffice`$$
DROP TRIGGER IF EXISTS `printeroffice`.`supplies_BEFORE_INSERT` $$
USE `printeroffice`$$
CREATE DEFINER = CURRENT_USER TRIGGER `supplies_BEFORE_INSERT` BEFORE INSERT ON `supplies` FOR EACH ROW
BEGIN
	UPDATE devices SET number = NEW.number WHERE id = NEW.deviceId;
    SET NEW.datetime = NOW();
END$$


DELIMITER ;

SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;

-- -----------------------------------------------------
-- Data for table `printeroffice`.`customers`
-- -----------------------------------------------------
START TRANSACTION;
USE `printeroffice`;
INSERT INTO `printeroffice`.`customers` (`tel`, `lastName`, `firstName`, `middleName`, `birthdate`, `address`) VALUES ('+79396795895', 'Шведова', 'Мерриджэйн', 'Ильинична', '1998-01-03', '347420, г. Богучар, ул. Николощеповский 1-й пер, дом 30, квартира 181');
INSERT INTO `printeroffice`.`customers` (`tel`, `lastName`, `firstName`, `middleName`, `birthdate`, `address`) VALUES ('+79402231549', 'Новицкий', 'Добровид', 'Дмитриевич', '1993-07-01', '630541, г. Калининское, ул. Парковая 7-я, дом 62, квартира 441');
INSERT INTO `printeroffice`.`customers` (`tel`, `lastName`, `firstName`, `middleName`, `birthdate`, `address`) VALUES ('+79929236423', 'Сысолятин', 'Роман', 'Ильич', '1987-11-04', '610046, г. Конышевка, ул. Новотихвинская, дом 75, квартира 273');
INSERT INTO `printeroffice`.`customers` (`tel`, `lastName`, `firstName`, `middleName`, `birthdate`, `address`) VALUES ('+79439467818', 'Омарова', 'Согра', 'Георгиевна', '1976-05-09', '242467, г. Кемерово, ул. Лесозаводская, дом 80, квартира 326');
INSERT INTO `printeroffice`.`customers` (`tel`, `lastName`, `firstName`, `middleName`, `birthdate`, `address`) VALUES ('+79815174981', 'Железная', 'Лаурианна', 'Петровна', '1994-10-01', '623741, г. Фирово, ул. Волгоградский пр-кт, дом 19, квартира 419');

COMMIT;


-- -----------------------------------------------------
-- Data for table `printeroffice`.`employees`
-- -----------------------------------------------------
START TRANSACTION;
USE `printeroffice`;
INSERT INTO `printeroffice`.`employees` (`id`, `lastName`, `firstName`, `middleName`, `fired`) VALUES (1, 'Калугин', 'Виктор', 'Иванович', DEFAULT);

COMMIT;


-- -----------------------------------------------------
-- Data for table `printeroffice`.`statuses`
-- -----------------------------------------------------
START TRANSACTION;
USE `printeroffice`;
INSERT INTO `printeroffice`.`statuses` (`id`, `title`) VALUES (1, 'Отменён');
INSERT INTO `printeroffice`.`statuses` (`id`, `title`) VALUES (2, 'Завершён');
INSERT INTO `printeroffice`.`statuses` (`id`, `title`) VALUES (3, 'Выполняется');

COMMIT;


-- -----------------------------------------------------
-- Data for table `printeroffice`.`deviceTypes`
-- -----------------------------------------------------
START TRANSACTION;
USE `printeroffice`;
INSERT INTO `printeroffice`.`deviceTypes` (`id`, `title`) VALUES (1, 'Картридж');
INSERT INTO `printeroffice`.`deviceTypes` (`id`, `title`) VALUES (2, 'Принтер');

COMMIT;


-- -----------------------------------------------------
-- Data for table `printeroffice`.`deviceManufacs`
-- -----------------------------------------------------
START TRANSACTION;
USE `printeroffice`;
INSERT INTO `printeroffice`.`deviceManufacs` (`id`, `title`) VALUES (1, 'HP');
INSERT INTO `printeroffice`.`deviceManufacs` (`id`, `title`) VALUES (2, 'ProfiLine');
INSERT INTO `printeroffice`.`deviceManufacs` (`id`, `title`) VALUES (3, 'Brother');
INSERT INTO `printeroffice`.`deviceManufacs` (`id`, `title`) VALUES (4, 'Sakura');
INSERT INTO `printeroffice`.`deviceManufacs` (`id`, `title`) VALUES (5, 'Canon');
INSERT INTO `printeroffice`.`deviceManufacs` (`id`, `title`) VALUES (6, 'Samsung');
INSERT INTO `printeroffice`.`deviceManufacs` (`id`, `title`) VALUES (7, 'Xerox');
INSERT INTO `printeroffice`.`deviceManufacs` (`id`, `title`) VALUES (8, 'Sony');

COMMIT;


-- -----------------------------------------------------
-- Data for table `printeroffice`.`devices`
-- -----------------------------------------------------
START TRANSACTION;
USE `printeroffice`;
INSERT INTO `printeroffice`.`devices` (`id`, `model`, `deviceTypeId`, `deviceManufac`, `price`, `number`) VALUES (1, 'LaserJet Pro 400 M401dn', 2, 1, 12000, 0);
INSERT INTO `printeroffice`.`devices` (`id`, `model`, `deviceTypeId`, `deviceManufac`, `price`, `number`) VALUES (2, 'LaserJet Pro P1102', 2, 1, 15000, 0);
INSERT INTO `printeroffice`.`devices` (`id`, `model`, `deviceTypeId`, `deviceManufac`, `price`, `number`) VALUES (3, 'LaserJet Pro 400 M425dn', 2, 1, 17000, 0);
INSERT INTO `printeroffice`.`devices` (`id`, `model`, `deviceTypeId`, `deviceManufac`, `price`, `number`) VALUES (4, 'Color LaserJet CP5225', 2, 1, 13500, 0);
INSERT INTO `printeroffice`.`devices` (`id`, `model`, `deviceTypeId`, `deviceManufac`, `price`, `number`) VALUES (5, 'DesignJet 111', 2, 1, 30000, 0);
INSERT INTO `printeroffice`.`devices` (`id`, `model`, `deviceTypeId`, `deviceManufac`, `price`, `number`) VALUES (6, 'C728D', 1, 2, 2000, 0);
INSERT INTO `printeroffice`.`devices` (`id`, `model`, `deviceTypeId`, `deviceManufac`, `price`, `number`) VALUES (7, 'CF280X', 1, 3, 2500, 0);
INSERT INTO `printeroffice`.`devices` (`id`, `model`, `deviceTypeId`, `deviceManufac`, `price`, `number`) VALUES (8, 'CE285A', 1, 5, 1700, 0);
INSERT INTO `printeroffice`.`devices` (`id`, `model`, `deviceTypeId`, `deviceManufac`, `price`, `number`) VALUES (9, 'LC567XLBK', 1, 3, 1400, 0);
INSERT INTO `printeroffice`.`devices` (`id`, `model`, `deviceTypeId`, `deviceManufac`, `price`, `number`) VALUES (10, 'CE041A', 1, 4, 1800, 0);

COMMIT;

-- -----------------------------------------------------
-- Data for table `printeroffice`.`supplies`
-- -----------------------------------------------------
START TRANSACTION;
USE `printeroffice`;
INSERT INTO `printeroffice`.`supplies` (`id`, `datetime`, `deviceId`, `price`, `number`) VALUES (1, NULL, 1, 100000, 10);
INSERT INTO `printeroffice`.`supplies` (`id`, `datetime`, `deviceId`, `price`, `number`) VALUES (2, NULL, 2, 200000, 10);
INSERT INTO `printeroffice`.`supplies` (`id`, `datetime`, `deviceId`, `price`, `number`) VALUES (3, NULL, 3, 120000, 10);
INSERT INTO `printeroffice`.`supplies` (`id`, `datetime`, `deviceId`, `price`, `number`) VALUES (4, NULL, 4, 231000, 10);
INSERT INTO `printeroffice`.`supplies` (`id`, `datetime`, `deviceId`, `price`, `number`) VALUES (5, NULL, 5, 100000, 10);

COMMIT;

-- -----------------------------------------------------
-- Data for table `printeroffice`.`servicesTypes`
-- -----------------------------------------------------
START TRANSACTION;
USE `printeroffice`;
INSERT INTO `printeroffice`.`servicesTypes` (`id`, `title`) VALUES (1, 'Ремонт');
INSERT INTO `printeroffice`.`servicesTypes` (`id`, `title`) VALUES (2, 'Заправка');
INSERT INTO `printeroffice`.`servicesTypes` (`id`, `title`) VALUES (3, 'Поставка');

COMMIT;

-- -----------------------------------------------------
-- Data for table `printeroffice`.`orders`
-- -----------------------------------------------------
START TRANSACTION;
USE `printeroffice`;
INSERT INTO `printeroffice`.`orders` (`id`, `emplId`, `custId`, `datetime`, `totalPrice`, `statusId`, `comment`) VALUES (1, 1, '+79402231549', '2020-03-03 10:00:00', 3800, 3, NULL);
INSERT INTO `printeroffice`.`orders` (`id`, `emplId`, `custId`, `datetime`, `totalPrice`, `statusId`, `comment`) VALUES (2, 1, '+79396795895', '2020-03-05 09:30:00', 1000, 3, NULL);
INSERT INTO `printeroffice`.`orders` (`id`, `emplId`, `custId`, `datetime`, `totalPrice`, `statusId`, `comment`) VALUES (3, 1, '+79929236423', '2020-03-04 13:20:00', 1000, 3, NULL);
INSERT INTO `printeroffice`.`orders` (`id`, `emplId`, `custId`, `datetime`, `totalPrice`, `statusId`, `comment`) VALUES (4, 1, '+79439467818', '2020-03-03 15:00:00', 3000, 3, NULL);
INSERT INTO `printeroffice`.`orders` (`id`, `emplId`, `custId`, `datetime`, `totalPrice`, `statusId`, `comment`) VALUES (5, 1, '+79815174981', '2020-03-06 12:00:00', 3000, 3, NULL);

COMMIT;

-- -----------------------------------------------------
-- Data for table `printeroffice`.`orderitems`
-- -----------------------------------------------------
START TRANSACTION;
USE `printeroffice`;
INSERT INTO `printeroffice`.`orderitems` (`id`, `orderId`, `deviceId`, `serviceId`, `statusId`) VALUES (1, 1, 1, 1, DEFAULT);
INSERT INTO `printeroffice`.`orderitems` (`id`, `orderId`, `deviceId`, `serviceId`, `statusId`) VALUES (2, 1, 3, 1, DEFAULT);
INSERT INTO `printeroffice`.`orderitems` (`id`, `orderId`, `deviceId`, `serviceId`, `statusId`) VALUES (3, 1, 4, 1, DEFAULT);
INSERT INTO `printeroffice`.`orderitems` (`id`, `orderId`, `deviceId`, `serviceId`, `statusId`) VALUES (4, 2, 1, 2, DEFAULT);
INSERT INTO `printeroffice`.`orderitems` (`id`, `orderId`, `deviceId`, `serviceId`, `statusId`) VALUES (5, 3, 1, 2, DEFAULT);
INSERT INTO `printeroffice`.`orderitems` (`id`, `orderId`, `deviceId`, `serviceId`, `statusId`) VALUES (6, 4, 3, 3, DEFAULT);
INSERT INTO `printeroffice`.`orderitems` (`id`, `orderId`, `deviceId`, `serviceId`, `statusId`) VALUES (7, 5, 5, 3, DEFAULT);

COMMIT;