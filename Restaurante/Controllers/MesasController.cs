using Microsoft.AspNetCore.Mvc;
using Restaurante.Dto;

namespace Restaurante.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class MesasController : Controller
    {
        [HttpGet("GetAll")]
        public async Task<ActionResult<List<MesaResponseDto>>> GeTAll()
        {
            return base.Ok(new { message = "Estas son todas las mesas" });
        }

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

    }
}
