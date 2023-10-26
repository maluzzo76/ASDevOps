CREATE TABLE [dbo].[TiposComprobante] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [Acronimo] VARCHAR (10)  NULL,
    [Nombre]   VARCHAR (100) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

