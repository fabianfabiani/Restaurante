using Microsoft.AspNetCore.Mvc;
using Restaurante.Dto;

namespace Restaurante.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : Controller
    {
        [HttpGet("GetAllPedidos")]
        public async Task<ActionResult<List<PedidoResponseDto>>> GetPedidos()
        {
            return base.Ok(new {message="Estos son todos los pedidos"});
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
