
select * from publicacion where 
idPublicacion  not in (select distinct t.IdPublicacion from Comentario t)