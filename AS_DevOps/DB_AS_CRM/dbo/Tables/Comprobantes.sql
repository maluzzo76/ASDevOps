CREATE TABLE [dbo].[Comprobantes] (
    [Id]                INT             IDENTITY (1000, 1) NOT NULL,
    [TipoComprobanteId] INT             NULL,
    [ClienteId]         INT             NULL,
    [Numero]            NUMERIC (18)    NULL,
    [FechaRegistracion] DATETIME        NULL,
    [FechaVencimiento]  DATETIME        NULL,
    [TotalNeto]         DECIMAL (18, 2) NULL,
    [Iva]               DECIMAL (18, 2) NULL,
    [IIBB]              DECIMAL (18, 2) NULL,
    [TotalBruto]        DECIMAL (18, 2) NULL,
    [FileName]          varchar(max) null,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([ClienteId]) REFERENCES [dbo].[Clientes] ([Id]),
    FOREIGN KEY ([TipoComprobanteId]) REFERENCES [dbo].[TiposComprobante] ([Id])
);

