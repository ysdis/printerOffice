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
  `middleName` VARCHAR(45) NOT NULL DEFAULT '-',
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
  `login` VARCHAR(50) NOT NULL,
  `password` VARCHAR(50) NOT NULL,
  `lastName` VARCHAR(45) NOT NULL,
  `firstName` VARCHAR(45) NOT NULL,
  `middleName` VARCHAR(45) NULL,
  `fired` TINYINT NOT NULL DEFAULT 0,
  `isAdmin` TINYINT NOT NULL DEFAULT 0,
  PRIMARY KEY (`login`))
ENGINE = InnoDB
AUTO_INCREMENT = 2
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `printeroffice`.`orderStatuses`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `printeroffice`.`orderStatuses` ;

CREATE TABLE IF NOT EXISTS `printeroffice`.`orderStatuses` (
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `title` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 4
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `printeroffice`.`services`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `printeroffice`.`services` ;

CREATE TABLE IF NOT EXISTS `printeroffice`.`services` (
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `title` VARCHAR(144) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 17
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `printeroffice`.`orders`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `printeroffice`.`orders` ;

CREATE TABLE IF NOT EXISTS `printeroffice`.`orders` (
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `emplLogin` VARCHAR(50) NOT NULL,
  `custId` VARCHAR(16) NOT NULL,
  `serviceId` INT NOT NULL,
  `startDateTime` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `lastModifiedDateTime` TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `statusId` INT(11) NOT NULL DEFAULT 3,
  `comment` VARCHAR(255) NULL DEFAULT '-',
  `totalPrice` INT NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`),
  INDEX `emplKey_idx` (`emplLogin` ASC) VISIBLE,
  INDEX `statusKey_idx` (`statusId` ASC) VISIBLE,
  INDEX `clientKey_idx` (`custId` ASC) VISIBLE,
  INDEX `serviceKey_idx` (`serviceId` ASC) VISIBLE,
  CONSTRAINT `clientKey`
    FOREIGN KEY (`custId`)
    REFERENCES `printeroffice`.`customers` (`tel`)
    ON UPDATE CASCADE,
  CONSTRAINT `statusKey`
    FOREIGN KEY (`statusId`)
    REFERENCES `printeroffice`.`orderStatuses` (`id`)
    ON UPDATE CASCADE,
  CONSTRAINT `emplKey`
    FOREIGN KEY (`emplLogin`)
    REFERENCES `printeroffice`.`employees` (`login`)
    ON UPDATE CASCADE,
  CONSTRAINT `serviceKey`
    FOREIGN KEY (`serviceId`)
    REFERENCES `printeroffice`.`services` (`id`)
    ON DELETE RESTRICT
    ON UPDATE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 12
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `printeroffice`.`deviceTypes`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `printeroffice`.`deviceTypes` ;

CREATE TABLE IF NOT EXISTS `printeroffice`.`deviceTypes` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `title` VARCHAR(50) NOT NULL,
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
  `deviceManufacId` INT NOT NULL,
  `deviceTypeId` INT NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `DevTypeKey_idx` (`deviceTypeId` ASC) VISIBLE,
  INDEX `DevManKey_idx` (`deviceManufacId` ASC) VISIBLE,
  CONSTRAINT `DevTypeKey`
    FOREIGN KEY (`deviceTypeId`)
    REFERENCES `printeroffice`.`deviceTypes` (`id`)
    ON DELETE RESTRICT
    ON UPDATE CASCADE,
  CONSTRAINT `DevManKey`
    FOREIGN KEY (`deviceManufacId`)
    REFERENCES `printeroffice`.`deviceManufacs` (`id`)
    ON DELETE RESTRICT
    ON UPDATE CASCADE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `printeroffice`.`storageStatuses`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `printeroffice`.`storageStatuses` ;

CREATE TABLE IF NOT EXISTS `printeroffice`.`storageStatuses` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `title` VARCHAR(50) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `printeroffice`.`destinations`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `printeroffice`.`destinations` ;

CREATE TABLE IF NOT EXISTS `printeroffice`.`destinations` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `title` VARCHAR(50) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `printeroffice`.`storage`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `printeroffice`.`storage` ;

CREATE TABLE IF NOT EXISTS `printeroffice`.`storage` (
  `sn` VARCHAR(50) NOT NULL,
  `deviceId` INT NOT NULL,
  `statusId` INT NOT NULL DEFAULT 1,
  `destinationId` INT NOT NULL DEFAULT 1,
  `price` INT NOT NULL DEFAULT 0,
  PRIMARY KEY (`sn`),
  INDEX `deviceKey_idx` (`deviceId` ASC) VISIBLE,
  INDEX `storStatKey_idx` (`statusId` ASC) VISIBLE,
  INDEX `destKet_idx` (`destinationId` ASC) VISIBLE,
  CONSTRAINT `deviceKey`
    FOREIGN KEY (`deviceId`)
    REFERENCES `printeroffice`.`devices` (`id`)
    ON DELETE RESTRICT
    ON UPDATE CASCADE,
  CONSTRAINT `storStatKey`
    FOREIGN KEY (`statusId`)
    REFERENCES `printeroffice`.`storageStatuses` (`id`)
    ON DELETE RESTRICT
    ON UPDATE CASCADE,
  CONSTRAINT `destKet`
    FOREIGN KEY (`destinationId`)
    REFERENCES `printeroffice`.`destinations` (`id`)
    ON DELETE RESTRICT
    ON UPDATE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 89
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `printeroffice`.`orderitems`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `printeroffice`.`orderitems` ;

CREATE TABLE IF NOT EXISTS `printeroffice`.`orderitems` (
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `orderId` INT NOT NULL,
  `storageSn` VARCHAR(50) NOT NULL,
  `price` INT NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`),
  INDEX `orderKey_idx` (`orderId` ASC) VISIBLE,
  CONSTRAINT `orderKey`
    FOREIGN KEY (`orderId`)
    REFERENCES `printeroffice`.`orders` (`id`)
    ON UPDATE CASCADE,
  CONSTRAINT `snKey`
    FOREIGN KEY (`storageSn`)
    REFERENCES `printeroffice`.`storage` (`sn`)
    ON DELETE RESTRICT
    ON UPDATE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 39
DEFAULT CHARACTER SET = utf8;

USE `printeroffice` ;

-- -----------------------------------------------------
-- Placeholder table for view `printeroffice`.`storageDetail`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `printeroffice`.`storageDetail` (`sn` INT, `'Модель'` INT, `'Статус'` INT, `'Предназначение'` INT, `'Стоимость'` INT);

-- -----------------------------------------------------
-- Placeholder table for view `printeroffice`.`ordersDetailed`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `printeroffice`.`ordersDetailed` (`id` INT, `'ФИО сотрудника'` INT, `'Номер клиента'` INT, `'Услуга'` INT, `'Дата заказа'` INT, `'Статус'` INT, `'Общая стоимость'` INT);

-- -----------------------------------------------------
-- Placeholder table for view `printeroffice`.`customersDetailed`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `printeroffice`.`customersDetailed` (`'ФИО клиента'` INT, `'Номер телефона'` INT, `'Дата рождения'` INT, `'Адрес проживания'` INT);

-- -----------------------------------------------------
-- Placeholder table for view `printeroffice`.`devicesDetailed`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `printeroffice`.`devicesDetailed` (`id` INT, `'Модель'` INT, `'Производитель'` INT, `'Тип устройства'` INT);

-- -----------------------------------------------------
-- Placeholder table for view `printeroffice`.`employeesDetailed`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `printeroffice`.`employeesDetailed` (`login` INT, `'ФИО сотрудника'` INT, `'Статус'` INT, `'Администратор'` INT);

-- -----------------------------------------------------
-- Placeholder table for view `printeroffice`.`devicesSelection`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `printeroffice`.`devicesSelection` (`sn` INT, `'Модель'` INT, `'Тип'` INT, `'Статус'` INT, `'Предназначение'` INT, `'Стоимость'` INT);

-- -----------------------------------------------------
-- View `printeroffice`.`storageDetail`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `printeroffice`.`storageDetail`;
DROP VIEW IF EXISTS `printeroffice`.`storageDetail` ;
USE `printeroffice`;
CREATE  OR REPLACE VIEW `storageDetail` AS
SELECT
	sn,
	devices.model AS 'Модель',
    storageStatuses.title AS 'Статус',
    destinations.title AS 'Предназначение',
    CONCAT(format(price, 0), ' ₽') AS 'Стоимость'
FROM 
	storage
JOIN storagestatuses ON storagestatuses.id = storage.statusId
JOIN destinations ON destinations.id = storage.destinationId
JOIN devices ON devices.id = storage.deviceId;

-- -----------------------------------------------------
-- View `printeroffice`.`ordersDetailed`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `printeroffice`.`ordersDetailed`;
DROP VIEW IF EXISTS `printeroffice`.`ordersDetailed` ;
USE `printeroffice`;
CREATE  OR REPLACE VIEW `ordersDetailed` AS SELECT
	orders.id,
    CONCAT(employees.lastName, ' ', LEFT(employees.firstName, 1), ' ', LEFT(employees.middleName, 1)) AS 'ФИО сотрудника',
    custId AS 'Номер клиента',
    services.title AS 'Услуга',
    date_format(orders.startDateTime, '%d %M %k:%i') AS 'Дата заказа',
    orderStatuses.title AS 'Статус',
    CONCAT(format(orders.totalPrice, 0), ' ₽') AS 'Общая стоимость'
FROM orders
	JOIN services ON orders.serviceId = services.id
    JOIN orderStatuses ON orders.statusId = orderStatuses.id
    JOIN employees ON employees.login = orders.emplLogin
ORDER BY 5;

-- -----------------------------------------------------
-- View `printeroffice`.`customersDetailed`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `printeroffice`.`customersDetailed`;
DROP VIEW IF EXISTS `printeroffice`.`customersDetailed` ;
USE `printeroffice`;
CREATE  OR REPLACE VIEW `customersDetailed` AS SELECT 
	concat(lastName, ' ', firstName, ' ', middleName) AS 'ФИО клиента',
    tel AS 'Номер телефона',
    date_format(birthdate, '%d %M %Y') AS 'Дата рождения',
    address AS 'Адрес проживания'
FROM customers
ORDER BY 1;

-- -----------------------------------------------------
-- View `printeroffice`.`devicesDetailed`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `printeroffice`.`devicesDetailed`;
DROP VIEW IF EXISTS `printeroffice`.`devicesDetailed` ;
USE `printeroffice`;
CREATE  OR REPLACE VIEW `devicesDetailed` AS SELECT
	devices.id,
    devices.model AS 'Модель',
    deviceManufacs.title AS 'Производитель',
    deviceTypes.title AS 'Тип устройства'
FROM devices
JOIN deviceManufacs ON deviceManufacs.id = devices.deviceManufacId
JOIN deviceTypes ON deviceTypes.id = devices.deviceTypeId;

-- -----------------------------------------------------
-- View `printeroffice`.`employeesDetailed`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `printeroffice`.`employeesDetailed`;
DROP VIEW IF EXISTS `printeroffice`.`employeesDetailed` ;
USE `printeroffice`;
CREATE  OR REPLACE VIEW `employeesDetailed` AS SELECT
	login,
    CONCAT(lastName, ' ', firstName, ' ', middleName) AS 'ФИО сотрудника',
    CASE
		WHEN fired = 1 THEN 'Уволен(а)'
        WHEN fired = 0 THEN 'Работает'
	END AS 'Статус',
    CASE
		WHEN isAdmin = 1 THEN 'Да'
        WHEN isAdmin = 0 THEN ''
	END AS 'Администратор'
FROM
	employees;

-- -----------------------------------------------------
-- View `printeroffice`.`devicesSelection`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `printeroffice`.`devicesSelection`;
DROP VIEW IF EXISTS `printeroffice`.`devicesSelection` ;
USE `printeroffice`;
CREATE  OR REPLACE VIEW `devicesSelection` AS SELECT
	sn,
	devices.model AS 'Модель',
    deviceTypes.title AS 'Тип',
    storageStatuses.title AS 'Статус',
    destinations.title AS 'Предназначение',
    storage.price AS 'Стоимость'
FROM 
	storage
JOIN storagestatuses ON storagestatuses.id = storage.statusId
JOIN destinations ON destinations.id = storage.destinationId
JOIN devices ON devices.id = storage.deviceId
JOIN deviceTypes ON devices.deviceTypeId = deviceTypes.id;
USE `printeroffice`;

DELIMITER $$

USE `printeroffice`$$
DROP TRIGGER IF EXISTS `printeroffice`.`orders_BEFORE_INSERT` $$
USE `printeroffice`$$
CREATE DEFINER = CURRENT_USER TRIGGER `printeroffice`.`orders_BEFORE_INSERT` BEFORE INSERT ON `orders` FOR EACH ROW
BEGIN
	DECLARE isEmplWork TINYINT;
    SELECT fired INTO isEmplWork FROM employees WHERE employees.login = NEW.emplLogin;
    
	SET NEW.startDateTime = NOW();
        
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
    
    IF NEW.serviceId != OLD.serviceId THEN -- Когда изменился вид услуги
		SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Изменение вида оказываемой услуги невозможно!';
    END IF;
        
	IF NEW.custId != OLD.custId THEN -- Когда изменился клиент
		SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Изменение клиента невозможно!';
    END IF;
    
	IF NEW.startDateTime != OLD.startDateTime THEN -- Когда изменилась дата заказа
		SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Изменение даты принятия заказа невозможно!';
    END IF;
    
    IF OLD.statusId = 3 THEN -- Когда старый статус "Выполняется"
		IF NEW.statusId = 1 THEN -- Когда новый статус "Отменён"
			IF OLD.serviceId = 3 THEN -- Когда "Продажа"
				UPDATE storage SET statusId = 1 WHERE sn IN(SELECT storageSn FROM orderitems WHERE orderId = OLD.id); -- Вернули на склад все устройства
            END IF;
            IF OLD.serviceId = 1 or OLD.serviceId = 2 THEN -- Когда "Ремонт","Заправка"
				UPDATE storage SET statusId = 2 WHERE sn IN(SELECT storageSn FROM orderitems WHERE orderId = OLD.id); -- Вернули клиентам их сломанные устройства
            END IF;
            -- При отмене возврата ничего не произойдёт, кроме изменения статуса заказа на возврат
		ELSE
			IF NEW.statusId = 2 THEN -- Когда новый статус "Завершён"
				IF OLD.serviceId = 3 or OLD.serviceId = 1 or OLD.serviceId = 2 THEN -- Когда "Продажа"
					UPDATE storage SET statusId = 2 WHERE sn IN(SELECT storageSn FROM orderitems WHERE orderId = OLD.id); -- Выдали с склада все устройства заказа
				END IF;
                IF OLD.serviceId = 1 or OLD.serviceId = 2 THEN -- Когда "Ремонт","Заправка"
					UPDATE storage SET statusId = 2 WHERE sn IN(SELECT storageSn FROM orderitems WHERE orderId = OLD.id); -- Вернули клиентам их сломанные устройства
				END IF;
				IF OLD.serviceId = 4 THEN -- Когда "Возврат"
					UPDATE storage SET statusId = 1 WHERE sn IN(SELECT storageSn FROM orderitems WHERE orderId = OLD.id); -- Вернули купленные устройства на склад
				END IF;
			END IF;
        END IF;
	ELSE
		SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'После отмены или завершения изменение статуса заказа невозможно!';
    END IF;
    
	SELECT fired INTO isEmplWork FROM employees WHERE employees.login = NEW.emplLogin;
	IF isEmplWork = 1 THEN
		SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Уволенные сотрудники не могут выполнять заказы!';
	END IF;
END$$


USE `printeroffice`$$
DROP TRIGGER IF EXISTS `printeroffice`.`storage_BEFORE_INSERT` $$
USE `printeroffice`$$
CREATE DEFINER = CURRENT_USER TRIGGER `printeroffice`.`storage_BEFORE_INSERT` BEFORE INSERT ON `storage` FOR EACH ROW
BEGIN
	IF NEW.destinationId = 2 THEN -- Когда предназначение хранение
		SET NEW.statusId = 2;
    END IF;
END$$


USE `printeroffice`$$
DROP TRIGGER IF EXISTS `printeroffice`.`storage_BEFORE_UPDATE` $$
USE `printeroffice`$$
CREATE DEFINER = CURRENT_USER TRIGGER `printeroffice`.`storage_BEFORE_UPDATE` BEFORE UPDATE ON `storage` FOR EACH ROW
BEGIN
	IF OLD.statusId != NEW.statusId and NEW.statusId = 2 and OLD.destinationId = 2 THEN -- Если устройство на хранении уходит со склада
		SET NEW.price = 0; -- Обнуляем его стоимость
	END IF;
END$$


USE `printeroffice`$$
DROP TRIGGER IF EXISTS `printeroffice`.`orderitems_BEFORE_INSERT` $$
USE `printeroffice`$$
CREATE DEFINER = CURRENT_USER TRIGGER `printeroffice`.`orderitems_BEFORE_INSERT` BEFORE INSERT ON `orderitems` FOR EACH ROW
BEGIN
    DECLARE statusOfOrder INT;
    DECLARE serviceOfOrder INT;
    DECLARE storageSnStatus INT;
    DECLARE storageSnDestination INT;
    
    SELECT statusId INTO statusOfOrder FROM orders WHERE id = NEW.orderId;
	SELECT serviceId INTO serviceOfOrder FROM orders WHERE id = NEW.orderId;
    SELECT statusId INTO storageSnStatus FROM storage WHERE sn = NEW.storageSn;
    SELECT destinationId INTO storageSnDestination FROM storage WHERE sn = NEW.storageSn;
    
    IF statusOfOrder != 3 THEN
		SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Добавление услуг в завершенный/отмененный заказ невозможна!';
    END IF;
    
    IF serviceOfOrder = 3 THEN -- Когда "Продажа"
		IF storageSnStatus = 1 and storageSnDestination = 1 THEN -- Если товар в наличии и предназначение "Реализация"
			UPDATE storage SET statusId = 3 WHERE sn = NEW.storageSn; -- Резервируется на складе
            SET NEW.price = (SELECT price FROM storage WHERE sn = NEW.storageSn); -- Цена на товар сохраняется в заказе
        ELSE
			SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Товар отсутсвует или зарезервирован!';
        END IF;
	END IF;
    
	IF serviceOfOrder = 1 or serviceOfOrder = 2 THEN -- Когда "Ремонт","Заправка"
		IF storageSnStatus = 2 and storageSnDestination = 2 THEN -- Если товар отсутствует и предназначение "Хранение"
			UPDATE storage SET statusId = 1 WHERE sn = NEW.storageSn; -- Попадает на склад
            SET NEW.price = (SELECT price FROM storage WHERE sn = NEW.storageSn); -- Цена за работу сохраняется в заказе
        ELSE
			SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Ошибка, устройство не может быть добавлено к заказу (Уже есть на складе, предназначен для реализации)!';
        END IF;
	END IF;
    
	IF serviceOfOrder = 4 THEN -- Когда "Возврат"
		IF storageSnStatus = 1 or storageSnStatus = 3 or storageSnDestination = 2 THEN -- Если товар присутствует или зарезервирован
			SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Ошибка, устройство не может быть возвращено!';
        END IF;
	END IF;
    
    -- Обновляем общую стоимость заказа
    UPDATE orders SET totalPrice = totalPrice + NEW.price WHERE id = NEW.orderId;
END$$


USE `printeroffice`$$
DROP TRIGGER IF EXISTS `printeroffice`.`orderitems_BEFORE_UPDATE` $$
USE `printeroffice`$$
CREATE DEFINER = CURRENT_USER TRIGGER `printeroffice`.`orderitems_BEFORE_UPDATE` BEFORE UPDATE ON `orderitems` FOR EACH ROW
BEGIN   
	IF NEW.storageSn != OLD.storageSn THEN
		SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Изменение устройств заказа невозможно!';
	END IF;
    
	IF NEW.orderId != OLD.orderId THEN
		SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Изменение номера заказа услуги невозможно!';
	END IF;
    
    -- Обновляем общую стоимость заказа
    UPDATE orders SET totalPrice = totalPrice + NEW.price WHERE id = NEW.orderId;
END$$


USE `printeroffice`$$
DROP TRIGGER IF EXISTS `printeroffice`.`orderitems_BEFORE_DELETE` $$
USE `printeroffice`$$
CREATE DEFINER = CURRENT_USER TRIGGER `printeroffice`.`orderitems_BEFORE_DELETE` BEFORE DELETE ON `orderitems` FOR EACH ROW
BEGIN
	DECLARE statusOfOrder INT;
    DECLARE serviceOfOrder INT;
	SELECT statusId INTO statusOfOrder FROM orders WHERE id = OLD.orderId;
    SELECT serviceId INTO serviceOfOrder FROM orders WHERE id = OLD.orderId;
    
	IF statusOfOrder != 3 THEN
		SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Удаление услуг в завершенном/отмененном заказ невозможно!';
    END IF;
    
    IF serviceOfOrder = 3 THEN -- Когда "Продажа"
		UPDATE storage SET statusId = 1 WHERE sn = OLD.storageSn; -- Резервация снимается
	END IF;
    
	IF serviceOfOrder = 1 or serviceOfOrder = 2 THEN -- Когда "Ремонт","Заправка"
		UPDATE storage SET statusId = 2 WHERE sn = OLD.storageSn; -- Уходит со склада, так как запись будет удалена
	END IF;
    
	-- Когда возврат, ничего не делаем, так как возвращение устройств на склад произойдет только после завершения заказа
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
INSERT INTO `printeroffice`.`employees` (`login`, `password`, `lastName`, `firstName`, `middleName`, `fired`, `isAdmin`) VALUES ('test', 'test', 'Калугин', 'Виктор', 'Иванович', DEFAULT, DEFAULT);
INSERT INTO `printeroffice`.`employees` (`login`, `password`, `lastName`, `firstName`, `middleName`, `fired`, `isAdmin`) VALUES ('admin', 'admin', 'Администрор', 'ИС', '-', DEFAULT, 1);

COMMIT;


-- -----------------------------------------------------
-- Data for table `printeroffice`.`orderStatuses`
-- -----------------------------------------------------
START TRANSACTION;
USE `printeroffice`;
INSERT INTO `printeroffice`.`orderStatuses` (`id`, `title`) VALUES (1, 'Отменён');
INSERT INTO `printeroffice`.`orderStatuses` (`id`, `title`) VALUES (2, 'Завершён');
INSERT INTO `printeroffice`.`orderStatuses` (`id`, `title`) VALUES (3, 'Выполняется');

COMMIT;


-- -----------------------------------------------------
-- Data for table `printeroffice`.`services`
-- -----------------------------------------------------
START TRANSACTION;
USE `printeroffice`;
INSERT INTO `printeroffice`.`services` (`id`, `title`) VALUES (1, 'Ремонт');
INSERT INTO `printeroffice`.`services` (`id`, `title`) VALUES (2, 'Заправка');
INSERT INTO `printeroffice`.`services` (`id`, `title`) VALUES (3, 'Продажа');
INSERT INTO `printeroffice`.`services` (`id`, `title`) VALUES (4, 'Возврат');

COMMIT;


-- -----------------------------------------------------
-- Data for table `printeroffice`.`orders`
-- -----------------------------------------------------
START TRANSACTION;
USE `printeroffice`;
INSERT INTO `printeroffice`.`orders` (`id`, `emplLogin`, `custId`, `serviceId`, `startDateTime`, `lastModifiedDateTime`, `statusId`, `comment`, `totalPrice`) VALUES (1, 'test', '+79402231549', 3, '2020-03-03 10:00:00', NULL, 3, '-', DEFAULT);
INSERT INTO `printeroffice`.`orders` (`id`, `emplLogin`, `custId`, `serviceId`, `startDateTime`, `lastModifiedDateTime`, `statusId`, `comment`, `totalPrice`) VALUES (2, 'test', '+79396795895', 3, '2020-03-05 09:30:00', NULL, 3, '-', DEFAULT);
INSERT INTO `printeroffice`.`orders` (`id`, `emplLogin`, `custId`, `serviceId`, `startDateTime`, `lastModifiedDateTime`, `statusId`, `comment`, `totalPrice`) VALUES (3, 'admin', '+79929236423', 2, '2020-03-04 13:20:00', NULL, 3, '-', DEFAULT);
INSERT INTO `printeroffice`.`orders` (`id`, `emplLogin`, `custId`, `serviceId`, `startDateTime`, `lastModifiedDateTime`, `statusId`, `comment`, `totalPrice`) VALUES (4, 'admin', '+79439467818', 2, '2020-03-03 15:00:00', NULL, 3, '-', DEFAULT);
INSERT INTO `printeroffice`.`orders` (`id`, `emplLogin`, `custId`, `serviceId`, `startDateTime`, `lastModifiedDateTime`, `statusId`, `comment`, `totalPrice`) VALUES (5, 'test', '+79815174981', 2, '2020-03-06 12:00:00', NULL, 3, '-', DEFAULT);

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
INSERT INTO `printeroffice`.`deviceManufacs` (`id`, `title`) VALUES (2, 'Brother');
INSERT INTO `printeroffice`.`deviceManufacs` (`id`, `title`) VALUES (3, 'Canon');
INSERT INTO `printeroffice`.`deviceManufacs` (`id`, `title`) VALUES (4, 'Xerox');
INSERT INTO `printeroffice`.`deviceManufacs` (`id`, `title`) VALUES (5, 'Samsung');
INSERT INTO `printeroffice`.`deviceManufacs` (`id`, `title`) VALUES (6, 'Sakura');
INSERT INTO `printeroffice`.`deviceManufacs` (`id`, `title`) VALUES (7, 'Sony');
INSERT INTO `printeroffice`.`deviceManufacs` (`id`, `title`) VALUES (8, 'Profline');

COMMIT;


-- -----------------------------------------------------
-- Data for table `printeroffice`.`devices`
-- -----------------------------------------------------
START TRANSACTION;
USE `printeroffice`;
INSERT INTO `printeroffice`.`devices` (`id`, `model`, `deviceManufacId`, `deviceTypeId`) VALUES (1, 'Laser Jet M4001', 1, 2);
INSERT INTO `printeroffice`.`devices` (`id`, `model`, `deviceManufacId`, `deviceTypeId`) VALUES (2, 'HP Photocartridge Pro D200', 1, 1);

COMMIT;


-- -----------------------------------------------------
-- Data for table `printeroffice`.`storageStatuses`
-- -----------------------------------------------------
START TRANSACTION;
USE `printeroffice`;
INSERT INTO `printeroffice`.`storageStatuses` (`id`, `title`) VALUES (1, 'В наличии');
INSERT INTO `printeroffice`.`storageStatuses` (`id`, `title`) VALUES (2, 'Отсутствует');
INSERT INTO `printeroffice`.`storageStatuses` (`id`, `title`) VALUES (3, 'Зарезервирован');

COMMIT;


-- -----------------------------------------------------
-- Data for table `printeroffice`.`destinations`
-- -----------------------------------------------------
START TRANSACTION;
USE `printeroffice`;
INSERT INTO `printeroffice`.`destinations` (`id`, `title`) VALUES (1, 'Реализация');
INSERT INTO `printeroffice`.`destinations` (`id`, `title`) VALUES (2, 'Хранение');

COMMIT;


-- -----------------------------------------------------
-- Data for table `printeroffice`.`storage`
-- -----------------------------------------------------
START TRANSACTION;
USE `printeroffice`;
INSERT INTO `printeroffice`.`storage` (`sn`, `deviceId`, `statusId`, `destinationId`, `price`) VALUES ('LJ6533', 1, 1, 1, 12000);
INSERT INTO `printeroffice`.`storage` (`sn`, `deviceId`, `statusId`, `destinationId`, `price`) VALUES ('LJ6534', 1, 1, 1, 15000);
INSERT INTO `printeroffice`.`storage` (`sn`, `deviceId`, `statusId`, `destinationId`, `price`) VALUES ('LJ6535', 1, 1, 1, 17000);
INSERT INTO `printeroffice`.`storage` (`sn`, `deviceId`, `statusId`, `destinationId`, `price`) VALUES ('LJ6536', 1, 1, 1, 13500);
INSERT INTO `printeroffice`.`storage` (`sn`, `deviceId`, `statusId`, `destinationId`, `price`) VALUES ('LJ6537', 1, 1, 1, 30000);
INSERT INTO `printeroffice`.`storage` (`sn`, `deviceId`, `statusId`, `destinationId`, `price`) VALUES ('LJ6538', 1, 1, 1, 2000);
INSERT INTO `printeroffice`.`storage` (`sn`, `deviceId`, `statusId`, `destinationId`, `price`) VALUES ('HPD001', 2, 2, 2, 2500);
INSERT INTO `printeroffice`.`storage` (`sn`, `deviceId`, `statusId`, `destinationId`, `price`) VALUES ('HPD002', 2, 2, 2, 1700);
INSERT INTO `printeroffice`.`storage` (`sn`, `deviceId`, `statusId`, `destinationId`, `price`) VALUES ('HPD003', 2, 2, 2, 1400);
INSERT INTO `printeroffice`.`storage` (`sn`, `deviceId`, `statusId`, `destinationId`, `price`) VALUES ('HPD004', 2, 2, 2, 1800);

COMMIT;


-- -----------------------------------------------------
-- Data for table `printeroffice`.`orderitems`
-- -----------------------------------------------------
START TRANSACTION;
USE `printeroffice`;
INSERT INTO `printeroffice`.`orderitems` (`id`, `orderId`, `storageSn`, `price`) VALUES (1, 1, 'LJ6533', DEFAULT);
INSERT INTO `printeroffice`.`orderitems` (`id`, `orderId`, `storageSn`, `price`) VALUES (2, 1, 'LJ6534', DEFAULT);
INSERT INTO `printeroffice`.`orderitems` (`id`, `orderId`, `storageSn`, `price`) VALUES (3, 1, 'LJ6535', DEFAULT);
INSERT INTO `printeroffice`.`orderitems` (`id`, `orderId`, `storageSn`, `price`) VALUES (4, 2, 'LJ6536', DEFAULT);
INSERT INTO `printeroffice`.`orderitems` (`id`, `orderId`, `storageSn`, `price`) VALUES (5, 3, 'HPD001', DEFAULT);
INSERT INTO `printeroffice`.`orderitems` (`id`, `orderId`, `storageSn`, `price`) VALUES (6, 4, 'HPD002', DEFAULT);
INSERT INTO `printeroffice`.`orderitems` (`id`, `orderId`, `storageSn`, `price`) VALUES (7, 5, 'HPD003', DEFAULT);

COMMIT;

