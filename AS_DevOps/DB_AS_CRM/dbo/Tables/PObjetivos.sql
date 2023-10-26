CREATE TABLE [dbo].[PObjetivos]
(
	[Id] INT identity(1,1) NOT NULL PRIMARY KEY,
	Nombre varchar(100),
	FechaIncio datetime,
	FechaEntrega datetime,
	Aprovador varchar(100),
	Estado_Id int,
	Proyecto_Id int,
	foreign key (Estado_Id) references PEstados(Id),
	foreign key (Proyecto_Id) references Proyectos(Id)
)
