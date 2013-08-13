SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

DROP SCHEMA IF EXISTS `sam_db` ;
CREATE SCHEMA IF NOT EXISTS `sam_db` DEFAULT CHARACTER SET latin1 COLLATE latin1_swedish_ci ;
USE `sam_db` ;

-- -----------------------------------------------------
-- Table `sam_db`.`Papel`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `sam_db`.`Papel` ;

CREATE  TABLE IF NOT EXISTS `sam_db`.`Papel` (
  `IdPapel` INT NOT NULL AUTO_INCREMENT ,
  `Descricao` VARCHAR(45) NOT NULL ,
  `Ativo` TINYINT(1) NOT NULL ,
  PRIMARY KEY (`IdPapel`) )
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `sam_db`.`Empresa`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `sam_db`.`Empresa` ;

CREATE  TABLE IF NOT EXISTS `sam_db`.`Empresa` (
  `IdEmpresa` INT NOT NULL AUTO_INCREMENT ,
  `NomeFantasia` VARCHAR(100) NOT NULL ,
  `RazaoSocial` VARCHAR(45) NULL ,
  `Dominio` VARCHAR(45) NOT NULL ,
  `Ativo` TINYINT(1) NOT NULL ,
  `Cnpj` VARCHAR(45) NOT NULL ,
  `Telefone1` VARCHAR(20) NULL ,
  `Telefone2` VARCHAR(45) NULL ,
  `Logo` VARCHAR(45) NULL ,
  `Segmento` VARCHAR(45) NULL ,
  `Site` VARCHAR(45) NULL ,
  PRIMARY KEY (`IdEmpresa`) )
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `sam_db`.`Usuario`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `sam_db`.`Usuario` ;

CREATE  TABLE IF NOT EXISTS `sam_db`.`Usuario` (
  `IdUsuario` INT NOT NULL AUTO_INCREMENT ,
  `Nome` VARCHAR(100) NOT NULL ,
  `Login` VARCHAR(100) NOT NULL ,
  `Senha` VARCHAR(100) NOT NULL ,
  `Email` VARCHAR(100) NOT NULL ,
  `ChaveRedefinirSenha` VARCHAR(100) NULL ,
  `Ativo` TINYINT(1) NOT NULL ,
  `IdPapel` INT NULL ,
  `IdEmpresa` INT NOT NULL ,
  PRIMARY KEY (`IdUsuario`) ,
  INDEX `fk_Usuario_Papel1_idx` (`IdPapel` ASC) ,
  INDEX `fk_Usuario_Empresa1_idx` (`IdEmpresa` ASC) ,
  CONSTRAINT `fk_Usuario_Papel1`
    FOREIGN KEY (`IdPapel` )
    REFERENCES `sam_db`.`Papel` (`IdPapel` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Usuario_Empresa1`
    FOREIGN KEY (`IdEmpresa` )
    REFERENCES `sam_db`.`Empresa` (`IdEmpresa` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `sam_db`.`Cargo`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `sam_db`.`Cargo` ;

CREATE  TABLE IF NOT EXISTS `sam_db`.`Cargo` (
  `IdCargo` INT NOT NULL AUTO_INCREMENT ,
  `Descricao` VARCHAR(45) NOT NULL ,
  `Ativo` TINYINT(1) NOT NULL ,
  `IdEmpresa` INT NOT NULL ,
  PRIMARY KEY (`IdCargo`) ,
  INDEX `fk_Cargo_Empresa1_idx` (`IdEmpresa` ASC) ,
  CONSTRAINT `fk_Cargo_Empresa1`
    FOREIGN KEY (`IdEmpresa` )
    REFERENCES `sam_db`.`Empresa` (`IdEmpresa` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `sam_db`.`Departamento`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `sam_db`.`Departamento` ;

CREATE  TABLE IF NOT EXISTS `sam_db`.`Departamento` (
  `IdDepartamento` INT NOT NULL AUTO_INCREMENT ,
  `Descricao` VARCHAR(45) NOT NULL ,
  `Ativo` TINYINT(1) NOT NULL ,
  `IdEmpresa` INT NOT NULL ,
  PRIMARY KEY (`IdDepartamento`) ,
  INDEX `fk_Departamento_Empresa1_idx` (`IdEmpresa` ASC) ,
  CONSTRAINT `fk_Departamento_Empresa1`
    FOREIGN KEY (`IdEmpresa` )
    REFERENCES `sam_db`.`Empresa` (`IdEmpresa` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `sam_db`.`Endereco`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `sam_db`.`Endereco` ;

CREATE  TABLE IF NOT EXISTS `sam_db`.`Endereco` (
  `IdEndereco` INT NOT NULL AUTO_INCREMENT ,
  `Endereco` VARCHAR(100) NULL ,
  `CEP` VARCHAR(8) NULL ,
  PRIMARY KEY (`IdEndereco`) )
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `sam_db`.`Funcionario`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `sam_db`.`Funcionario` ;

CREATE  TABLE IF NOT EXISTS `sam_db`.`Funcionario` (
  `IdFuncionario` INT NOT NULL AUTO_INCREMENT ,
  `Telefone` VARCHAR(30) NULL ,
  `Celular` VARCHAR(30) NULL ,
  `Foto` VARCHAR(500) NULL ,
  `Matricula` VARCHAR(20) NULL ,
  `Sexo` VARCHAR(1) NULL ,
  `Skype` VARCHAR(45) NULL ,
  `DataNascimento` DATETIME NULL ,
  `DataFuncao` DATETIME NULL ,
  `IdUsuario` INT NOT NULL ,
  `IdCargo` INT NULL ,
  `IdDepartamento` INT NULL ,
  `IdGestor` INT NULL ,
  PRIMARY KEY (`IdFuncionario`) ,
  INDEX `fk_Funcionario_Usuario1_idx` (`IdUsuario` ASC) ,
  INDEX `fk_Funcionario_Cargo1_idx` (`IdCargo` ASC) ,
  INDEX `fk_Funcionario_Departamento1_idx` (`IdDepartamento` ASC) ,
  CONSTRAINT `fk_Funcionario_Usuario1`
    FOREIGN KEY (`IdUsuario` )
    REFERENCES `sam_db`.`Usuario` (`IdUsuario` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Funcionario_Cargo1`
    FOREIGN KEY (`IdCargo` )
    REFERENCES `sam_db`.`Cargo` (`IdCargo` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Funcionario_Departamento1`
    FOREIGN KEY (`IdDepartamento` )
    REFERENCES `sam_db`.`Departamento` (`IdDepartamento` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `sam_db`.`Hierarquia`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `sam_db`.`Hierarquia` ;

CREATE  TABLE IF NOT EXISTS `sam_db`.`Hierarquia` (
  `IdHierarquia` INT NOT NULL AUTO_INCREMENT ,
  `IdUsuario` INT NOT NULL ,
  `IdUsuarioPai` INT NULL ,
  PRIMARY KEY (`IdHierarquia`) ,
  INDEX `fk_Hierarquia_Usuario1_idx` (`IdUsuario` ASC) ,
  CONSTRAINT `fk_Hierarquia_Usuario1`
    FOREIGN KEY (`IdUsuario` )
    REFERENCES `sam_db`.`Usuario` (`IdUsuario` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `sam_db`.`Filial`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `sam_db`.`Filial` ;

CREATE  TABLE IF NOT EXISTS `sam_db`.`Filial` (
  `IdFilial` INT NOT NULL ,
  `NomeFantasia` VARCHAR(45) NULL ,
  `RazaoSocial` VARCHAR(45) NULL ,
  `Cnpj` VARCHAR(45) NULL ,
  `Telefone1` VARCHAR(45) NULL ,
  `Telefone2` VARCHAR(45) NULL ,
  `IdEmpresa` INT NOT NULL ,
  PRIMARY KEY (`IdFilial`) ,
  INDEX `fk_Filial_Empresa1_idx` (`IdEmpresa` ASC) ,
  CONSTRAINT `fk_Filial_Empresa1`
    FOREIGN KEY (`IdEmpresa` )
    REFERENCES `sam_db`.`Empresa` (`IdEmpresa` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;



SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;

-- -----------------------------------------------------
-- Data for table `sam_db`.`Papel`
-- -----------------------------------------------------
START TRANSACTION;
USE `sam_db`;
INSERT INTO `sam_db`.`Papel` (`IdPapel`, `Descricao`, `Ativo`) VALUES (NULL, 'Administrador do sistema', 1);
INSERT INTO `sam_db`.`Papel` (`IdPapel`, `Descricao`, `Ativo`) VALUES (NULL, 'Administrador da empresa', 1);
INSERT INTO `sam_db`.`Papel` (`IdPapel`, `Descricao`, `Ativo`) VALUES (NULL, 'Avaliador', 1);
INSERT INTO `sam_db`.`Papel` (`IdPapel`, `Descricao`, `Ativo`) VALUES (NULL, 'Colaborador', 1);

COMMIT;

-- -----------------------------------------------------
-- Data for table `sam_db`.`Empresa`
-- -----------------------------------------------------
START TRANSACTION;
USE `sam_db`;
INSERT INTO `sam_db`.`Empresa` (`IdEmpresa`, `NomeFantasia`, `RazaoSocial`, `Dominio`, `Ativo`, `Cnpj`, `Telefone1`, `Telefone2`, `Logo`, `Segmento`, `Site`) VALUES (NULL, 'GMail', NULL, 'gmail.com', 1, NULL, NULL, NULL, NULL, NULL, NULL);

COMMIT;

-- -----------------------------------------------------
-- Data for table `sam_db`.`Usuario`
-- -----------------------------------------------------
START TRANSACTION;
USE `sam_db`;
INSERT INTO `sam_db`.`Usuario` (`IdUsuario`, `Nome`, `Login`, `Senha`, `Email`, `ChaveRedefinirSenha`, `Ativo`, `IdPapel`, `IdEmpresa`) VALUES (1, 'Wellington Gasparin', 'gasparin@gmail.com', '1000:pkajg4Y8BmORuv7BlGIC5SAtmGYtzC86:JfpbDhPqKj0wBk+IXHFIFUe60gga7HGS', 'gasparin@gmail.com', NULL, 1, 1, 1);
INSERT INTO `sam_db`.`Usuario` (`IdUsuario`, `Nome`, `Login`, `Senha`, `Email`, `ChaveRedefinirSenha`, `Ativo`, `IdPapel`, `IdEmpresa`) VALUES (2, 'Michel Lopes', 'michelcl.ti@gmail.com', '1000:pkajg4Y8BmORuv7BlGIC5SAtmGYtzC86:JfpbDhPqKj0wBk+IXHFIFUe60gga7HGS', 'michelcl.ti@gmail.com', NULL, 1, 1, 1);

COMMIT;

-- -----------------------------------------------------
-- Data for table `sam_db`.`Cargo`
-- -----------------------------------------------------
START TRANSACTION;
USE `sam_db`;
INSERT INTO `sam_db`.`Cargo` (`IdCargo`, `Descricao`, `Ativo`, `IdEmpresa`) VALUES (NULL, 'Cargo 1', 1, 1);
INSERT INTO `sam_db`.`Cargo` (`IdCargo`, `Descricao`, `Ativo`, `IdEmpresa`) VALUES (NULL, 'Cargo 2', 1, 1);
INSERT INTO `sam_db`.`Cargo` (`IdCargo`, `Descricao`, `Ativo`, `IdEmpresa`) VALUES (NULL, 'Cargo 3', 1, 1);

COMMIT;

-- -----------------------------------------------------
-- Data for table `sam_db`.`Departamento`
-- -----------------------------------------------------
START TRANSACTION;
USE `sam_db`;
INSERT INTO `sam_db`.`Departamento` (`IdDepartamento`, `Descricao`, `Ativo`, `IdEmpresa`) VALUES (NULL, 'Departamento 1', 1, 1);
INSERT INTO `sam_db`.`Departamento` (`IdDepartamento`, `Descricao`, `Ativo`, `IdEmpresa`) VALUES (NULL, 'Departamento 2', 1, 1);
INSERT INTO `sam_db`.`Departamento` (`IdDepartamento`, `Descricao`, `Ativo`, `IdEmpresa`) VALUES (NULL, 'Departamento 3', 1, 1);

COMMIT;

-- -----------------------------------------------------
-- Data for table `sam_db`.`Funcionario`
-- -----------------------------------------------------
START TRANSACTION;
USE `sam_db`;
INSERT INTO `sam_db`.`Funcionario` (`IdFuncionario`, `Telefone`, `Celular`, `Foto`, `Matricula`, `Sexo`, `Skype`, `DataNascimento`, `DataFuncao`, `IdUsuario`, `IdCargo`, `IdDepartamento`, `IdGestor`) VALUES (NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, 1, NULL);
INSERT INTO `sam_db`.`Funcionario` (`IdFuncionario`, `Telefone`, `Celular`, `Foto`, `Matricula`, `Sexo`, `Skype`, `DataNascimento`, `DataFuncao`, `IdUsuario`, `IdCargo`, `IdDepartamento`, `IdGestor`) VALUES (NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 2, NULL, NULL, NULL);

COMMIT;
