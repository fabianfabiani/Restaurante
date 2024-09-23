using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurante.Data;
using Restaurante.Dto;
using Restaurante.Entities;
using Restaurante.Interface;

namespace Restaurante.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : Controller
    {
        private readonly IPedidoService _pedidoService;
        private readonly DataBaseContext _context;
        public PedidosController(IPedidoService pedidoService, DataBaseContext context)
        {
            _pedidoService = pedidoService;
            _context = context;
        }

        [HttpGet("GetAllPedidos")]
        public async Task<ActionResult<List<PedidoResponseDto>>> GetAllPedidos()
        {
            var pedidosResponseDto = await _pedidoService.GetAllPedidos();
            return Ok(new {message ="Estos son todos los pedidos", Pedido = pedidosResponseDto});
        }
        
        [HttpPost("create")]
        public async Task<ActionResult<PedidoResponseDto>> CrearPedido(PedidoRequestDto pedido)
        {
            var pedidoResponseDto = await _pedidoService.CrearPedido(pedido);
            return Ok(new { message = "Ha creado un nuevo pedido", Pedido = pedidoResponseDto });
        }

        [HttpPost("UpdateEstadoPedido")]
        public async Task<IActionResult> ActualizarEstado(int id)
        {
            // Llama al servicio para actualizar el estado del pedido
            await _pedidoService.ActualizarEstadoPedido(id);

            // Consulta el estado actualizado del pedido desde la base de datos
            var pedidoActualizado = await _context.Pedidos
                .Include(p => p.EstadoPedido) // Incluye la tabla de EstadoPedido para obtener la descripción
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedidoActualizado == null)
            {
                return NotFound("Pedido no encontrado.");
            }

            // Obtiene la descripción del estado actualizado
            var estadoActual = pedidoActualizado.EstadoPedido.Descripcion;

            // Retorna la respuesta con el estado actualizado
            return Ok($"Se actualizó el estado de su pedido a '{estadoActual}'");
        }



        /*
        [HttpGet("getPorCodigoPedido/{codigoPedido}")]
        public async Task<ActionResult<PedidoResponseDto>> GetPorCodigoPedido(string codigoPedido)
        {
            return base.Ok(new { message = "Este es el pedido que busca por su codigo" });
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
        */
    }
}
