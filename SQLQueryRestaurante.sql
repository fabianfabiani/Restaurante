
use RestauranteC;

/****************************/
insert into EstadoMesa(Descripcion)
values
	('cliente esperando pedido'),
	('cliente comiendo'),
	('cliente pagando'),
	('cerrada'),
	('vacia');

select * from EstadoMesa

/************************/
insert into EstadoPedido(Descripcion)
  values
		('pendiente')
		('en preparacion'),
		('listo para servir');
		
		/*('finalizado')*/;

select * from EstadoPedido

/************************************/
  insert into Roles(Descripcion)
  values
		('bartender'),
		('cervecero'),
		('cocinero'),
		('mozo'),
		('administrador'),
		('socio');
select * from Roles
/*****************************************/
  insert into Sectores(descripcion)
  values
		('Tragos'),
		('Cerveceria'),
		('Cocina'),
		('Postres'),
		('Administracion'),
		('Socio');

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
		(4,4,'Pepe','pepe','1111'),
		(5,5,'Admin1','unAdmin','2222'),
		(6,6,'Socio1','unSocio','3333');
	
	Select * from Empleados


ALTER TABLE Mesas
ADD CONSTRAINT DF_Mesas_EstadoMesaId DEFAULT 5 FOR EstadoMesaId; /*cambie a 5 "vacia"*/

/*cargamos Mesas*/
 insert into Mesas(Nombre)
  values('mesa para 6'),
		('mesa para 6'),
		('mesa para 8'),
		('terraza para 4'),
		('terraza para 6')
select * from Mesas;

/*Cargamos Productos */
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
select * from Productos;

/* IMPORTANTE 3 
Nota:
No se puede Generar un pedido si no existe una comanda
Para crear una comanda necesitamos :
Id que es el numero de mesa, y su nombre significa donde queda y para cuantas personas es la mesa.
nombreCliente : El nombre del cliente
codigoComanda : Es un c�digo Alfanumerico de 5 caracteres

Ahora si podemos crear un pedido ya que tenemos el Id de la comanda (ComandaId)
para crear el pedido necesitamos:
ProductoId : Este ya esta precargado, seg�n su Id es el producto relacionado.
ComandaId : La comanda generada para esa mesa y cliente (Deberia de existir para poder crear el pedido).
EstadoId : Son 3 los estados posibles, "Pendiente", "En preparaci�n" y "Listo para servir"
Cantidad: La cantidad del producto solicitado.
FechaCreacion : Se auto completa con la fecha del dia generado el pedido.
*/


/* 7 de septiembre.
******IMPORTANTE 1 *******
Se agregaron mesas con su descripcion por ejemplo 'Mesa para 4'
Estas tienen una relacion de una a muchas con EstadoMesas, lo que significa que la mesa puede estar 
cliente esperando pedido
cliente comiendo
cliente pagando
cerrada
Para nuestra logica y para no generar otro campo m�s como podria ser: Mesa 'Libre', vamos a tomar que cuando creamos
una Mesa nos cree el campo
EstadoMesaId = 4

*****IMPORTANTE 2 *******
Modificamos la tabla para que por default sea 4 (cerrada)
La idea es que cuando se cree una nueva comanda , se supone que esa mesa esta libre (cerrada)
entonces pase al estado 1 (cliente esperando pedido)
Dentro de la creacion de comanda no tenemos el campo EstadoMesa, pero cuando se ejecute la una nueva comanda deberia ejecutarse una 
logica que nos modifique esa mesa o haga un Update en la tabla Mesas en el campo EstadoMesaId = 1 ("cliente esperando pedido") 
*/

/*
formato DateTime
2024-09-22T14:30:00
*/

/*Se agrego Filter para el endpoint GetAllPedidos
solo pueden entrar Socio o Admin a modo de ejemplo
