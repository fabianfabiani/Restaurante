using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurante.Data;
using Restaurante.Dto;
using Restaurante.Entities;
using Restaurante.Interface;
using System.ComponentModel.DataAnnotations;

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
        public async Task<ActionResult<List<PedidoRequestDto>>> GetAllPedidos()
        {
            var pedidosResponseDto = await _pedidoService.GetAllPedidos();
            return Ok(new {message ="Estos son todos los pedidos", Pedido = pedidosResponseDto});
        }
        
        [HttpPost("create")]
        public async Task<IActionResult> CrearPedido(PedidoRequestDto pedido)
        {
            await _pedidoService.CrearPedido(pedido);
            return Ok(new { message = "Ha creado un nuevo pedido" });
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

        [HttpGet("pendientes/empleado/{idEmpleado}")]
        public async Task<ActionResult<List<PedidoListarDTO>>> ListarPedidosPendientesPorEmpleado(int idEmpleado)
        {
            try
            {
                var pedidosPendientes = await _pedidoService.ListarPedidosPendientesPorEmpleado(idEmpleado);
                return Ok(pedidosPendientes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("cambiarEstadoEnPreparacion/{pedidoId}")]
        public async Task<ActionResult> CambiarEstadoEnPreparacion(int pedidoId, [FromQuery] DateTime tiempoPreparacion)
        {
            try
            {
                await _pedidoService.CambiarEstadoEnPreparacion(pedidoId, tiempoPreparacion);
                return Ok("Estado cambiado a 'en preparación' y tiempo estimado actualizado."); // Mensaje de éxito
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message); // Retorna el mensaje de error en caso de que falle
            }
        }
        
    }
}
