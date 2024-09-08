using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurante.Data;
using Restaurante.Dto;
using Restaurante.Entities;

namespace Restaurante.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : Controller
    {
        protected readonly DataBaseContext _context; //Se Agrego para la coneccion con la BD
        public PedidosController(DataBaseContext context) //Se Agrego para la coneccion con la BD
        {
            _context = context;
        }

        [HttpGet("GetAllPedidos")]
        public async Task<ActionResult<List<PedidoResponseDto>>> GetPedidos()
        {
            //return base.Ok(new {message="Estos son todos los pedidos"});
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
            return Ok(respuesta);
        }

        [HttpGet("getPorCodigoPedido/{codigoPedido}")]
        public async Task<ActionResult<PedidoResponseDto>> GetPorCodigoPedido(string codigoPedido)
        {
            return base.Ok(new {message="Este es el pedido que busca por su codigo"});
        }

        [HttpPost("create")]
        public async Task<ActionResult> CreatePedido(PedidoRequestDto pedido)
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
            return Ok(new { message = "Ha creado un pedido nuevo", Pedido = pedidoResponse });
        }

        [HttpPut("actualizarEstado/{codigoPedido}/{nuevoEstado}")]
        public async Task<ActionResult<PedidoResponseDto>> ActualizarEstado(string codigoPedido, string nuevoEstado)
        {
            return base.Ok(new {message="Usted ha actualizado un pedido"});
        }

        [HttpPut("tiempoEstimado/{codigoPedido}/{tiempoEstimado}")]
        public async Task<ActionResult<PedidoResponseDto>> SetTiempoEstimado(string codigoPedido, TimeSpan tiempoEstimado)  // ¿Como se usa TimeSpan?
        {
            return base.Ok(new {message="este e el tiempo estimado de su pedido"});
        }
    }
}
