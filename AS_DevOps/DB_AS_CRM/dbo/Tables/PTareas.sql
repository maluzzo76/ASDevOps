﻿CREATE TABLE [dbo].[PTareas]
(
	[Id] INT Identity(1,1) NOT NULL PRIMARY KEY,
	Nombre varchar(100),
	Usuario_Id nvarchar(128),
	Estado_Id int,
	Objetivo_Id int,
	Sprint_Id int,
	FechaIncio datetime,
	FechaFinalizado datetime,
	FechaEntrega datetime,
	HorasEstimadas int,
	Detalle varchar(max),
	foreign key (Usuario_Id) references AspNetUsers(Id),
	foreign key (Estado_Id) references PEstados(Id),
	foreign key (Objetivo_Id) references Pobjetivos(Id),
	foreign key (Sprint_Id) references PSprints(Id),
)
