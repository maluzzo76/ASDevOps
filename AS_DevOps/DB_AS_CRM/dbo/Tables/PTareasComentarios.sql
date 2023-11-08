CREATE TABLE [dbo].[PTareasComentarios]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1),
	Tarea_Id int,
	Usuario_Id nvarchar(128),
	Descripcion varchar(max),
	Fecha datetime,
	foreign key (Tarea_Id) references PTareas(Id),
	foreign key (Usuario_Id) references AspNetUsers(Id)
)
