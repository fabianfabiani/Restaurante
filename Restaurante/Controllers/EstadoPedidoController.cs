using Microsoft.AspNetCore.Mvc;

namespace Restaurante.Controllers
{
    [ApiController]
    [Route("api/controller")]
    public class EstadoPedidoController : Controller
    {
        [HttpGet("{codigoMesa/{codigoPedido}")]
        public async Task<ActionResult> GetEstadoPedido(string codigoMesa, string codigoPedido) //busca el pedido que coincida con ambos codigos
        {
            return base.Ok(new { message = "Este es el estado de su pedido" });
        }
    }
}
