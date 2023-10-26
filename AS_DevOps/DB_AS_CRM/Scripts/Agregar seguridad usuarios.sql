DECLARE @UI NVARCHAR(128), @USERNAME VARCHAR(100) = 'mariano.aluzzo@alusoft.com.ar'

SET @UI = (SELECT Id FROM AspNetUsers WHERE [USERNAME] = @USERNAME)


-- insert menu finanzas --
declare @idMf int
insert into MenuSecurity ([User_Id],Nombre,IsActivo,Orden) values(@UI,'Finanzas',0,1)
set @idMf = @@IDENTITY
insert into ItemMenuSecurity (Menu_Id,Nombre, IsActivo, Orden) values (@idMf,'Plan de Cuentas',0,1)
insert into ItemMenuSecurity (Menu_Id,Nombre, IsActivo, Orden) values (@idMf,'Asientos', 0,2)
insert into ItemMenuSecurity (Menu_Id,Nombre, IsActivo, Orden) values (@idMf,'Gastos',0,5)
insert into ItemMenuSecurity (Menu_Id,Nombre, IsActivo, Orden) values (@idMf,'Comprobantes',0,4)
insert into ItemMenuSecurity (Menu_Id,Nombre, IsActivo, Orden) values (@idMf,'Certificaciones',0,3)

-- insert menu CRM --
declare @idCRM int
insert into MenuSecurity ([User_Id],Nombre,IsActivo,Orden) values(@UI,'CRM',0,0)
set @idCRM = @@IDENTITY
insert into ItemMenuSecurity (Menu_Id,Nombre,IsActivo, Orden) values (@idCRM,'Leads',0,0)
insert into ItemMenuSecurity (Menu_Id,Nombre,IsActivo, Orden) values (@idCRM,'Clientes',0,1)
insert into ItemMenuSecurity (Menu_Id,Nombre,IsActivo, Orden) values (@idCRM,'Acuerdos',0,2)

-- insert menu Configuracion --
declare @idConf int
insert into MenuSecurity ([User_Id],Nombre,IsActivo,Orden) values(@UI,'Configuración',0,4)
set @idConf = @@IDENTITY
insert into ItemMenuSecurity (Menu_Id,Nombre,IsActivo, Orden) values (@idConf,'Tipos de Gastos',0,0)
insert into ItemMenuSecurity (Menu_Id,Nombre,IsActivo, Orden) values (@idConf,'Tipo de Comprobantes',0,1)

-- insert menu Devops --
declare @idDev int
insert into MenuSecurity ([User_Id],Nombre,IsActivo,Orden) values(@UI,'DevOps',0,3)
set @idDev = @@IDENTITY
insert into ItemMenuSecurity (Menu_Id,Nombre,IsActivo, Orden) values (@idDev,'Proyectos',0,0)


--select * from MenuSecurity
--update MenuSecurity set IsActivo = 1
--select * from ItemMenuSecurity
--update ItemMenuSecurity set IsActivo = 1

