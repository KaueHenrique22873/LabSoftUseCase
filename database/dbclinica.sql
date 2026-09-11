use dbclinica;
go

Create Table Paciente(
	
	Codigo INT IDENTITY(1,1) PRIMARY KEY ,
	Nome varchar(200) Not Null,
	CPF varchar(200) Not Null,
	Telefone varchar(200) Not Null,
	DataNascimento date Not Null

);

go

Create Table Medico(
	
	Codigo INT IDENTITY(1,1) PRIMARY KEY ,
	Nome varchar(200) Not Null,
	CRM varchar(200) Not Null,
	Especialidade varchar(200) Not Null
);

go

Create Table Consulta (

	Codigo INT IDENTITY(1,1) PRIMARY KEY ,
	DataHora datetime Not Null,
	StatusConsulta varchar(200) Not Null,
	PacienteId INT NOT NULL,
    MedicoId INT NOT NULL,
	Constraint FK_Consulta_Paciente FOREIGN KEY	(PacienteID)
		REFERENCES Paciente(Codigo),

	Constraint FK_Consulta_Medico FOREIGN KEY (MedicoID)
		REFERENCES Medico(Codigo),
);

go