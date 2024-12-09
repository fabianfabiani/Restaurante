using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurante.Data;
using Restaurante.Dto;
using Restaurante.Entities;
using Restaurante.Interface;

namespace Restaurante.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
 
    public class MesasController : Controller
    {
        private readonly IMesaService _mesaService;
        public MesasController(IMesaService mesaService)
        {
            _mesaService = mesaService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<MesaListarDTO>> GeTAll()
        {
            
            var mesaResponseDto = await _mesaService.GeTAll();
            if (mesaResponseDto == null)
            {
                return NotFound("Nose encontraron mesas.");
            }

            return Ok(new { Message = "Listado de mesas", Mesas = mesaResponseDto});
        }
        
        [HttpPost("Create")]
        public async Task<IActionResult> CrearMesa([FromBody] MesaRequestDto mesa)
        {
           await _mesaService.CrearMesa(mesa);
            return Ok(new {message="Se agrego una mesa"});
        }


        [HttpGet("ObtenerMesaMasUsada")]
        public async Task<IActionResult> ObtenerMesaMasUsada()
        {
            var mesa = await _mesaService.ObtenerMesaMasUsadaAsync();

            if (mesa == null)
                return NotFound(new { Mensaje = "No se encontró ninguna mesa que cumpla con los criterios." });

            return Ok(new { Mensaje = $"Mesa más usada es la Numero = {mesa.Id}" });
        }

        [HttpGet("mesaMenosUsada")]
        public async Task<IActionResult> ObtenerMesaMenosUsada()
        {
            var mesaMenosUsada = await _mesaService.ObtenerMesaMenosUsadaAsync();

            if (mesaMenosUsada == null)
                return NotFound(new { Mensaje = "No se encontraron mesas con encuestas." });

            return Ok(new { Mensaje = $"Mesa menos usada es la Numero = {mesaMenosUsada.Id}" });
        }

        [HttpGet("mejoresComentarios")]
        public async Task<ActionResult> ObtenerMejoresComentarios()
        {
            var mejorEncuesta = await _mesaService.ObtenerMejorComentarioAsync();

            if (mejorEncuesta == null)
                return NotFound(new { Mensaje = "No se encontraron encuestas." });

            // Usamos 'dynamic' para acceder a las propiedades del objeto anónimo
            return Ok(new
            {
                Mensaje = "Mesa con Mejor Valoracion",
                MesaId = ((dynamic)mejorEncuesta).MesaId,  // Acceder a la propiedad MesaId
                Comentario = ((dynamic)mejorEncuesta).Comentario  // Acceder a la propiedad Comentario
            });
        }

        [HttpGet("peoresComentarios")]
        public async Task<ActionResult> ObtenerPeoresComentarios()
        {
            var peorEncuesta = await _mesaService.ObtenerPeorComentarioAsync();

            if (peorEncuesta == null)
                return NotFound(new { Mensaje = "No se encontraron encuestas." });

            // Usar var para trabajar con el objeto anónimo
            return Ok(new
            {
                Mensaje = "Mesa con Peor Valoracion",
                MesaId = ((dynamic)peorEncuesta).MesaId,  // Acceder a la propiedad MesaId
                Comentario = ((dynamic)peorEncuesta).Comentario  // Acceder a la propiedad Comentario
            });
        }

        /*
        [HttpGet("GetById/{idMesa}")]
        public async Task<ActionResult<MesaResponseDto>> GetById(int idMesa)
        {
            return base.Ok(new { message = "Estos son los detalles de la mesa" });
        }

        [HttpPost("Update")]
        public async Task<ActionResult<MesaResponseDto>> Update(int idMesa, MesaRequestDto mesa)
        {
            return base.Ok(new { message = "Mesa actualizada" });
        }
        */
    }
}
