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
    }
}
