CREATE DATABASE dbTasks;
GO
USE dbTasks;
GO

-- Tabela Departamento
CREATE TABLE Departamento (
    Codigo INT IDENTITY(1,1) PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL,
    Sigla VARCHAR(10) NOT NULL
);
GO

-- Tabela Funcionario
CREATE TABLE Funcionario (
    Codigo INT IDENTITY(1,1) PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL,
    Cargo VARCHAR(50) NOT NULL,
    DepartamentoId INT NOT NULL,
    CONSTRAINT FK_Funcionario_Departamento FOREIGN KEY (DepartamentoId) 
        REFERENCES Departamento(Codigo)
);
GO

CREATE TABLE Incidente (
    Codigo INT IDENTITY(1,1) PRIMARY KEY,
    DescricaoProblema VARCHAR(250) NOT NULL,
    DataIncidente DATETIME NOT NULL,
    Solucao VARCHAR(250) NULL,
    Resolvido VARCHAR(3) NOT NULL -- 'sim' ou 'nao'
);
GO

CREATE TABLE Projeto (
    Codigo INT IDENTITY(1,1) PRIMARY KEY,
    NomeProjeto VARCHAR(200) NOT NULL,
    Orcamento DECIMAL(12,2) NOT NULL,
    Status VARCHAR(30) NOT NULL -- 'Em Planejamento', 'Em Andamento', 'Concluido'
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

-- Inserindo Dados Iniciais para Teste

INSERT INTO Departamento (Nome, Sigla) VALUES 
('Tecnologia da Informacao', 'TI'),
('Recursos Humanos', 'RH'),
('Financeiro', 'FIN');

INSERT INTO Funcionario (Nome, Cargo, DepartamentoId) VALUES 
('Carlos Silva', 'Desenvolvedor Senior', 1),
('Ana Oliveira', 'Analista de QA', 1),
('Roberto Santos', 'Gerente de RH', 2);

INSERT INTO Tarefa (Descricao, DataPlanejada, DataIniciada, DataFinalizada, DataCancelada, StatusTarefa, Prazo, FuncionarioId) VALUES 
('Criar tela de Login', '2026-08-10', '2026-08-01', NULL, NULL, 'Em Andamento', 'Em dia', 1),
('Homologar Release 1.0', '2026-08-05', NULL, NULL, NULL, 'Pendente', 'Em atraso', 2);


GO