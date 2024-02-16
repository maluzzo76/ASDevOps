CREATE PROCEDURE [dbo].[Sp_GetHorasCertificacionByProyectoId]
	@idp int 
AS
select 
'Task' Tarea,
t.Id,
t.Nombre,
convert(char,FechaFinalizado,103) Fecha, 
case when sum(p.Horas) > t.HorasEstimadas then sum(p.Horas) else HorasEstimadas end Horas
from PTareas t
inner join PObjetivos o on o.id = t.Objetivo_Id
left join PParteHoras p on p.Tarea_Id = t.id 
where 
t.FechaFinalizado is not null
and isnull( t.Certificada,0) = 0
and o.Proyecto_Id = @idp
group by t.Id,t.Nombre,FechaFinalizado,t.HorasEstimadas
