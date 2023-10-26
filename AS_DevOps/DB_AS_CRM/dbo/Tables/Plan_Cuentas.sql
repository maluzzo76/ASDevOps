CREATE TABLE [dbo].[Plan_Cuentas]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1),	
	Codigo varchar(100),
	Nombre varchar(255),
	Numero int,
	IsImputable bit default(0),
	IsResultado bit default(0)
)
