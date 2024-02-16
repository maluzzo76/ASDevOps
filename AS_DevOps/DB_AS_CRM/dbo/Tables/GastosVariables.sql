CREATE TABLE [dbo].[GastosVariables] (
    [Id]            INT             IDENTITY (1, 1) NOT NULL,
    [TipoGastoId]   INT             NULL,
    [Descripcion]   VARCHAR (200)   NULL,
    [Importe]       DECIMAL (18, 4) NULL,
    [FechaRegistro] DATETIME        NULL,
    [Iva]           DECIMAL (18, 4) NULL,
    [IIBB]          DECIMAL (18, 4) NULL,
    [Neto]          DECIMAL (18, 4) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([TipoGastoId]) REFERENCES [dbo].[TipoGasto] ([Id]),
    Cuenta_Id int,
    foreign key (Cuenta_Id) references Plan_Cuentas(Id)
);

