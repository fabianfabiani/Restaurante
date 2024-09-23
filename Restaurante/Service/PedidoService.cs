using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurante.Data;
using Restaurante.Dto;
using Restaurante.Entities;
using Restaurante.Interface;

namespace Restaurante.Service
{
    public class PedidoService : IPedidoService
    {
        private readonly DataBaseContext _context;
        public PedidoService(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<List<PedidoResponseDto>>> GetAllPedidos()
        {
            var pedidos = await _context.Pedidos.
               Include(c => c.Producto).
               Include(c => c.Comanda).
               ToListAsync();

            var respuesta = pedidos.Select(e => new PedidoResponseDto
            {
                Producto = e.Producto.Descripcion,
                CodigoComanda = e.ComandaId,
                Estado = e.Producto.Descripcion,
                Cantidad = e.Cantidad,
                FechaCreacion = e.FechaCreacion,
                FechaFinalizacion = e.FechaFinalizacion,
            }).ToList();
            return respuesta;
        }

        public async Task<ActionResult<PedidoResponseDto>> CrearPedido(PedidoRequestDto pedido)
        {
            Producto? p = _context.Productos.Where(p => p.Id == pedido.ProductoId).FirstOrDefault();
            if (p == null)
            {
                throw new Exception("Producto inexistente, no puede cargar pedido");
            }
            p.ReducirStock(pedido.Cantidad);
            this._context.Productos.Update(p);

            var nuevoPedido = new Pedido
            {
                ProductoId = pedido.ProductoId,
                ComandaId = pedido.ComandaId,
                EstadoId = pedido.EstadoId,
                Cantidad = pedido.Cantidad,
                FechaCreacion = DateTime.Now.Date,
                FechaFinalizacion = pedido.FechaFinalizacion?.Date //.Date formatea para que solo se muestre la fecha
            };
            _context.Pedidos.Add(nuevoPedido);
            await _context.SaveChangesAsync();

            var pedidoResponse = new PedidoResponseDto
            {
                Producto = (await _context.Productos.FindAsync(nuevoPedido.ProductoId)).Descripcion,
                CodigoComanda = nuevoPedido.ComandaId,
                Estado = (await _context.EstadoMesa.FindAsync(nuevoPedido.EstadoId)).Descripcion,
                Cantidad = nuevoPedido.Cantidad,
                FechaCreacion = nuevoPedido.FechaCreacion,
                FechaFinalizacion = nuevoPedido.FechaFinalizacion

            };
            return pedidoResponse;
        }

        public async Task ActualizarEstadoPedido(int pedidoId)
        {
            // Recuperamos el pedido desde la base de datos
            var pedido = await _context.Pedidos.FindAsync(pedidoId);
            if (pedido == null)
            {
                throw new Exception("Pedido no encontrado.");
            }

            // Recuperamos el estado actual del pedido desde la base de datos
            var estadoActual = await _context.EstadoPedido.FindAsync(pedido.EstadoId);
            if (estadoActual == null)
            {
                throw new Exception("Estado del pedido no encontrado.");
            }

            // Verificamos si el estado actual es 'Finalizado'
            if (estadoActual.Descripcion == "Finalizado")
            {
                throw new Exception("No se puede actualizar el estado de un pedido finalizado.");
            }

            // Si el estado actual es 'En Preparación', actualizamos la fecha de finalización
            if (estadoActual.Descripcion == "En Preparación")
            {
                pedido.FechaFinalizacion = DateTime.Now;
            }

            // Obtener el siguiente estado (según la lógica de negocio)
            var siguienteEstado = await _context.EstadoPedido
                .OrderBy(e => e.Id)
                .FirstOrDefaultAsync(e => e.Id > pedido.EstadoId);

            if (siguienteEstado == null)
            {
                throw new Exception("No hay un siguiente estado disponible.");
            }

            // Actualizar el EstadoId del pedido con el nuevo estado
            pedido.EstadoId = siguienteEstado.Id;
            pedido.EstadoPedido = siguienteEstado;

            // Guardamos los cambios en la base de datos
            await _context.SaveChangesAsync();
        }
    }
}
