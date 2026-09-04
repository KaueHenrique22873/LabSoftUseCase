CREATE DATABASE dbTasks;
GO
USE dbTasks;
GO

-- Tabela Funcionario
CREATE TABLE Funcionario (
    Codigo INT IDENTITY(1,1) PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL,
    Cargo VARCHAR(50) NOT NULL
);
GO

-- Tabela Tarefa
CREATE TABLE Tarefa (
    Codigo INT IDENTITY(1,1) PRIMARY KEY,
    Descricao VARCHAR(200) NOT NULL,
    DataPlanejada DATETIME NOT NULL,
    DataIniciada DATETIME NULL,
    DataFinalizada DATETIME NULL,
    DataCancelada DATETIME NULL,
    StatusTarefa VARCHAR(30) NOT NULL,
    Prazo VARCHAR(20) NOT NULL,
    FuncionarioId INT NOT NULL,
    CONSTRAINT FK_Tarefa_Funcionario FOREIGN KEY (FuncionarioId) 
        REFERENCES Funcionario(Codigo)
);
GO

-- Tabela Incidente
CREATE TABLE Incidente (
    Codigo INT IDENTITY(1,1) PRIMARY KEY,
    DescricaoProblema VARCHAR(250) NOT NULL,
    DataIncidente DATETIME NOT NULL,
    Solucao VARCHAR(250) NULL,
    Resolvido BIT NOT NULL DEFAULT 0,
    FuncionarioId INT NOT NULL,
    CONSTRAINT FK_Incidente_Funcionario FOREIGN KEY (FuncionarioId) 
        REFERENCES Funcionario(Codigo)
);
GO

-- Tabela Departamento
CREATE TABLE Departamento (
    Codigo INT IDENTITY(1,1) PRIMARY KEY,
    Descricao VARCHAR(250) NOT NULL,
    Ativo BIT NULL DEFAULT 1
);
GO

-- Tabela Projetos
CREATE TABLE Projetos (
    Codigo INT IDENTITY(1,1) PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL,
    Descricao VARCHAR(250) NULL,
    DataInicio DATETIME NULL,
    DataFim DATETIME NULL
);
GO