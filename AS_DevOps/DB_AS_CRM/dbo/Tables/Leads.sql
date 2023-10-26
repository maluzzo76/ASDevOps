CREATE TABLE [dbo].[Leads] (
    [Id]                    INT            IDENTITY (1000, 1) NOT NULL,
    [Razon_Social]          VARCHAR (255)  NULL,
    [CUIT]                  VARCHAR (255)  NULL,
    [Telefono_Razon_Social] VARCHAR (255)  NULL,
    [Email_Razon_Social]    VARCHAR (255)  NULL,
    [Nombre_Contacto]       VARCHAR (255)  NULL,
    [Cargo_Contacto]        VARCHAR (255)  NULL,
    [Telefono_Contacto]     VARCHAR (255)  NULL,
    [Telefono2_Contacto]    VARCHAR (255)  NULL,
    [Email_Contacto]        VARCHAR (255)  NULL,
    [Email2_Contacto]       VARCHAR (255)  NULL,
    [Informacion_Fiscal]    VARCHAR (8000) NULL,
    [Provincia]             VARCHAR (255)  NULL,
    [Localidad]             VARCHAR (255)  NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

