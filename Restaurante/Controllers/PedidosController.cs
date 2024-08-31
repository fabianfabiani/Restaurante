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
        public async Task<ActionResult<PedidoResponseDto>> CreatePedido(PedidoRequestDto pedido)
        {
            return base.Ok(new {message="Usted ha creado un pedido"});
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
