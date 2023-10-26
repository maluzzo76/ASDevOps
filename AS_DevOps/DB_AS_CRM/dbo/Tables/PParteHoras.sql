CREATE TABLE [dbo].[PParteHoras]
(
	[Id] INT identity(1,1) NOT NULL PRIMARY KEY,
	UserName varchar(128),
	Tarea_Id int,
	Fecha datetime,
	Horas int,
	foreign key (Tarea_Id) references PTareas(Id),
)
