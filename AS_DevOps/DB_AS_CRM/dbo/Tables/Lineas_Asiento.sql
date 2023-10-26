CREATE TABLE [dbo].[Lineas_Asiento]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1),
	Asiento_Id int,
	Cuenta_Id int,
	Concepto varchar(255),
	Debe decimal(18,4),
	Haber decimal(18,4),
	foreign key (Asiento_Id) references Asientos(Id),
	foreign key (Cuenta_Id) references Plan_Cuentas(Id)
)
