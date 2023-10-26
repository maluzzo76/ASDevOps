CREATE TABLE [dbo].[Clientes] (
    [Id]             INT             IDENTITY (1000, 1) NOT NULL,
    [RazonSocial]    VARCHAR (100)   NULL,
    [NombreContacto] VARCHAR (255)   NULL,
    [Telefono]       VARCHAR (100)   NULL,
    [Email]          VARCHAR (500)   NULL,
    [ValorHora]      DECIMAL (18, 4) NULL,
    [FechaDeAcuerdo] DATETIME        NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

