SELECT TOP (1000) [Id]
      ,[SectorId]
      ,[Descripcion]
      ,[Stock]
      ,[Precio]
  FROM [Restaurante].[dbo].[Productos]

insert into Productos(SectorId, Descripcion, Stock, Precio)
values
		(1,'Malbec Vino tinto',10,15500),
		(1,'Bonarda Vino tinto',12,22000),
		(1,'Michel Torino tinto',20,7600),
		(1,'Pantera Rosa',10,8000),
		(1,'Daikiri Frutilla',10,8000),
		(1,'Satanas',10,8000),
		(2,'Quilmes',50,4500),
		(2,'Estela',50,7000),
		(2,'Cerveza Artesanal Mendoza',15,6000),
		(2,'Cerveza Artesanal San Juan',13,6500),
		(3,'Espaguetis con estofado',25,9000),
		(3,'Vacio',30,12000),
		(3,'Milanesa',25,10000),
		(3,'Empanada',60,1500),
		(4,'Flan',15,2500),
		(4,'Tiramisu',15,2500),
		(4,'Helado',15,2500);
select * from Productos
go;
/****************************/
insert into EstadoMesa(Descripcion)
values
	('cliente esperando pedido'),
	('cliente comiendo'),
	('cliente pagando'),
	('cerrada');

select * from EstadoMesa
go
/************************/
insert into EstadoPedido(Descripcion)
  values
		('pendiente'),
		('en preparacion'),
		('listo para servir');

select * from EstadoPedido
go
/************************************/
  insert into Roles(Descripcion)
  values
		('bartender'),
		('cervecero'),
		('cocinero'),
		('mozo'),
		('socio');
select * from Roles
/*****************************************/
  insert into Sectores(descripcion)
  values
		('Tragos y vinos'),
		('Cervezas'),
		('Cocina'),
		('Candy bar');

select * from Sectores
/*********************************************/
 insert into Empleados(SectorId, RolId, Nombre, Usuario, Password)
  values
		(1,1,'Lucas','lucas','1111'),
		(1,1,'Ariel','ariel','1111'),
		(2,2,'Adriana','adriana','1111'),
		(2,2,'Lili','lili','1111'),
		(3,3,'Fabian','fabian','1111'),
		(3,3,'Hector','hector','1111'),
		(4,4,'Carlos','carlos','1111'),
		(4,4,'Pepe','pepe','1111');
	go;
	Select * from Empleados
