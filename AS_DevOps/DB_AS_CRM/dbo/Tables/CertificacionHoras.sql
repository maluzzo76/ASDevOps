CREATE TABLE [dbo].[CertificacionHoras]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1),
	Fecha datetime,
	ClienteId int,
	ComprobanteId int,
	HorasACertificar decimal(18,4),
	HorasCertificadas decimal(18,4),
	Saldo decimal(18,4),
	foreign key (ClienteId) references dbo.Clientes(Id),
	foreign key (ComprobanteId) references dbo.Comprobantes(Id),
)
