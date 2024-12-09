using Entidades;
using LaboratorioApi.Services;
using LaboratorioWeb.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LaboratorioWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class EncuestaController : ControllerBase
    {
        private readonly IEncuestaService _encuestaService;

        public EncuestaController(IEncuestaService encuestaService)
        {
            _encuestaService = encuestaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Encuesta>>> GetEncuestas()
        {
            var encuestas = await _encuestaService.GetEncuestasAsync();
            return Ok(encuestas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Encuesta>> GetEncuesta(int id)
        {
            var encuesta = await _encuestaService.GetEncuestaByIdAsync(id);
            if (encuesta == null) return NotFound();

            return Ok(encuesta);
        }

        [HttpPost("CrearEncuesta")]
        public async Task<ActionResult<Encuesta>> CreateEncuesta(CrearEncuestaDTO nuevaEncuestaDto)
        {
            // Mapear DTO a la entidad
            var nuevaEncuesta = new Encuesta
            {
                PuntuacionMesa = nuevaEncuestaDto.PuntuacionMesa,
                PuntuacionRestaurante = nuevaEncuestaDto.PuntuacionRestaurante,
                PuntuacionMozo = nuevaEncuestaDto.PuntuacionMozo,
                PuntuacionCocinero = nuevaEncuestaDto.PuntuacionCocinero,
                Comentario = nuevaEncuestaDto.Comentario,
                FechaEncuesta = nuevaEncuestaDto.FechaEncuesta,
                ComandaId = nuevaEncuestaDto.ComandaId,
                MesaId = nuevaEncuestaDto.MesaId
            };

            // Validar campos
            List<string> listerrores = await _encuestaService.ValidarCampos(nuevaEncuesta);
            if (listerrores.Count > 0)
            {
                return BadRequest(string.Join(Environment.NewLine, listerrores));
            }

            // Crear encuesta
            var encuestaCreada = await _encuestaService.CreateEncuestaAsync(nuevaEncuesta);
            return CreatedAtAction(nameof(GetEncuesta), new { id = encuestaCreada.Id }, encuestaCreada);
        }





        [HttpDelete]
        public async Task<IActionResult> DeleteEncuesta(int id)
        {
            var eliminado = await _encuestaService.DeleteEncuestaAsync(id);
            if (!eliminado) return NotFound();

            return NoContent();
        }
    }
}
