CREATE TABLE [dbo].[Proyectos]
(
	[Id] INT identity(1,1) NOT NULL PRIMARY KEY,
	Nombre varchar(100),
	logo varchar(100),
	Cliente_Id int,
	FOREIGN KEY	(Cliente_Id) references Clientes(Id)
)
