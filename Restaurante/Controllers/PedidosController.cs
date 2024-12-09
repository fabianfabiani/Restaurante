using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurante.Data;
using Restaurante.Dto;
using Restaurante.Entities;
using Restaurante.Filters;
using Restaurante.Interface;
using Restaurante.Service;
using System.ComponentModel.DataAnnotations;

namespace Restaurante.Controllers
{
    //[ServiceFilter(typeof(AutenticationFilter))]
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

        //Filter aplicado a GetAllPedidos
        //Para acceder en Postman agrego la ruta ejemplo https://localhost:7215/api/Pedidos/GetAllPedidos
        // en Headers Key : Rol y Value : Socio o Admin 
        /*[AccessFilter(Enumerables.ERol.Admin, Enumerables.ERol.Socio)]*/ //(Enumerables.ERol.Socio, Enumerables.ERol.Admin)]
        [Authorize(policy: "Socio")]
        [HttpGet("GetAllPedidos")]
        public async Task<ActionResult<List<PedidoRequestDto>>> GetAllPedidos()
        {
            var pedidosResponseDto = await _pedidoService.GetAllPedidos();
            return Ok(new { message = "Estos son todos los pedidos", Pedido = pedidosResponseDto });
        }


        [HttpPost("create")]
        public async Task<IActionResult> CrearPedido(PedidoRequestDto pedido)
        {

            if (pedido.EmpleadoId <= 0) // Validar que el EmpleadoId sea válido
            {
                return BadRequest(new { message = "El Empleado es obligatorio." });
            }

            var codigoGenerado = await _pedidoService.CrearPedido(pedido);
            return Ok(new { message = "Ha creado un nuevo pedido", codigoPedido = codigoGenerado });
        }






        //  [HttpPost("UpdateEstadoPedido")]
        //    public async Task<IActionResult> ActualizarEstado(int id)
        /// <summary>
        ///   {
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        //      await _pedidoService.ActualizarEstadoPedido(id);
        //      return Ok("Estado del pedido actualizado.");
        //  }


        [Authorize(policy: "Empleado")]
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

        [Authorize(policy: "Empleado")]
        [HttpPost("cambiarEstadoEnPreparacion")]
        public async Task<ActionResult> CambiarEstadoEnPreparacion( string CodigoPedido, int empleadoId, DateTime tiempoPreparacion)
        {
            try
            {
                await _pedidoService.CambiarEstadoEnPreparacion(CodigoPedido, empleadoId, tiempoPreparacion);
                return Ok("Estado cambiado a 'en preparación' y tiempo estimado actualizado."); // Mensaje de éxito
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message); // Retorna el mensaje de error en caso de que falle
            }
        }

        [Authorize(policy: "Empleado")]
        [HttpPost("cambiarEstadoListoParaServir")]
        public async Task<ActionResult> CambiarEstadoListoParaServir(string CodigoPedido, int empleadoId)
        {
            try
            {
                await _pedidoService.CambiarEstadoListoParaServir(CodigoPedido, empleadoId);
                return Ok("Estado cambiado a 'listo para servir'.");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        
        [HttpGet("ObtenerTiempoDeDemora")]
        public async Task<ActionResult<string>> ObtenerTiempoDeDemora(int mesaId, string codigoPedido)
        {
            try
            {
                var resultado = await _pedidoService.ObtenerTiempoDeDemora(mesaId, codigoPedido);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(policy: "Socio")]
        [HttpGet("GetAllTiempoDemora")] //Va a listar todos los pedidos con sus demoras que esten en estado en preparacion 
        public async Task<ActionResult<List<object>>> GetAllTiempoDemora()
        {
            try
            {
                // Llamamos al servicio para obtener la lista filtrada y proyectada
                var pedidosConDemoras = await _pedidoService.GetPedidosEnPreparacionConDemoras();

                // Proyectar los resultados a solo los campos CodigoPedido y TiempoTranscurrido
                var resultado = pedidosConDemoras.Select(p => new
                {
                    CodigoPedido = p.CodigoPedido,
                    TiempoTranscurrido = p.TiempoTranscurrido
                }).ToList();

                // Devolvemos la respuesta con OK (200)
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                // Manejamos cualquier posible error
                return StatusCode(500, $"Ocurrió un error: {ex.Message}");
            }
        }

        [Authorize(policy: "Mozo")]
        [HttpPut("SiListoParaServirCambiarEstadoMesa/{mesaId}")]
        public async Task<IActionResult> SiListoParaServirCambiarEstadoMesa(int mesaId)
        {
            var result = await _pedidoService.SiListoParaServirCambiarEstadoMesa(mesaId);
            return Ok(result);
        }

        [Authorize(policy: "Mozo")]
        [HttpPost("CambiarEstadoMesaYCalcularTotal/{codigoComanda}")]
        public async Task<ActionResult<float>> CambiarEstadoMesaYCalcularTotal(string codigoComanda)
        {
            return await _pedidoService.CambiarEstadoMesaYCalcularTotal(codigoComanda);
        }

        [Authorize(policy: "Socio")]
        [HttpPost("cerrar-mesa")]
        public async Task<IActionResult> CerrarMesa(string codigoComanda)
        {
            var resultado = await _pedidoService.CambiarEstadoMesaCerrada(codigoComanda);

            if (resultado == "Comanda no existe.")
            {
                return NotFound(resultado);
            }

            if (resultado == "La mesa ya está cerrada." || resultado == "La mesa no se puede cerrar ya que no está en estado 'cliente pagando'.")
            {
                return BadRequest(resultado);
            }

            return Ok(resultado);
        }


    }
}
