CREATE TABLE [dbo].[TipoGasto] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [Acronimo] VARCHAR (10)  NULL,
    [Nombre]   VARCHAR (100) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    Cuenta_Id int,
    foreign key (Cuenta_Id) references Plan_Cuentas(Id)
);

