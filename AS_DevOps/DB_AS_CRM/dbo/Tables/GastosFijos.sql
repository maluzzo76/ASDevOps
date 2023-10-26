CREATE TABLE [dbo].[GastosFijos] (
    [Id]            INT             IDENTITY (1, 1) NOT NULL,
    [TipoGastoId]   INT             NULL,
    [Descripcion]   VARCHAR (200)   NULL,
    [Importe]       DECIMAL (18, 4) NULL,
    [FechaRegistro] DATETIME        NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([TipoGastoId]) REFERENCES [dbo].[TipoGasto] ([Id])
);

