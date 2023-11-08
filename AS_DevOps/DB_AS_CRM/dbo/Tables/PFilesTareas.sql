CREATE TABLE [dbo].[PFilesTareas]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1),
	Nombre varchar(250),
	Extencion varchar(20),
	LinkName varchar(128),
	Tarea_Id int,
	foreign Key (Tarea_Id) references PTareas(Id)
)
