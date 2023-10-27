CREATE TABLE [dbo].[PSprints]
(
	[Id] INT identity(1,1) NOT NULL PRIMARY KEY,
	Proyecto_Id int,
	Nombre varchar(100),
	FechaIncio datetime,
	FechaFin datetime,
	Estado_id int,
	foreign key (Proyecto_Id) references Proyectos(Id),
	foreign key (Estado_id) references PEstados(Id)
)
