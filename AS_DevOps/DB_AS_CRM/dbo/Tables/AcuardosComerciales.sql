CREATE TABLE [dbo].[AcuardosComerciales] (
    [Id]            INT             IDENTITY (1, 1) NOT NULL,
    [ClienteId]     INT             NULL,
    [ValorHora]     DECIMAL (18, 4) NULL,
    [HorasVendidas] INT             NULL,
    [ImporteTotal]  DECIMAL (18, 4) NULL,
    [Fecha]         DATETIME        NULL,
    [IsActiva]      BIT             NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([ClienteId]) REFERENCES [dbo].[Clientes] ([Id])
);


GO

CREATE TRIGGER dbo.tr_AcuerdosComercialesCreate
ON dbo.AcuardosComerciales
FOR insert
as 
	declare @Id int, @clienteId int, @valorHora decimal(18,4),@isValida bit, @fecha datetime
	select @id = Id, @clienteId = ClienteId, @valorHora = ValorHora, @isValida = IsActiva, @fecha = Fecha from inserted

  update AcuardosComerciales set ImporteTotal = ValorHora * HorasVendidas where id = @Id
  if @isValida = 1
  begin
   update Clientes set ValorHora = @valorHora, FechaDeAcuerdo = @fecha where Id = @clienteId
  end
GO
 CREATE TRIGGER dbo.tr_AcuerdosComercialesUpdate
ON dbo.AcuardosComerciales
after update
as 
	declare @Id int, @clienteId int, @valorHora decimal(18,4),@isValida bit, @fecha datetime
	select @id = Id, @clienteId = ClienteId, @valorHora = ValorHora, @isValida = IsActiva, @fecha = Fecha from inserted

  update AcuardosComerciales set ImporteTotal = ValorHora * HorasVendidas where id = @Id
  if @isValida = 1
  begin
   update Clientes set ValorHora = @valorHora, FechaDeAcuerdo = @fecha where Id = @clienteId
  end