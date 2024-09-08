using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurante.Data;
using Restaurante.Dto;
using Restaurante.Entities;

namespace Restaurante.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
 
    public class MesasController : Controller
    {
        private readonly DataBaseContext _context;
        public MesasController(DataBaseContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<MesaResponseDto>> GeTAll()
        {
            var mesas = await _context.Mesas
                .Include(m => m.EstadoMesa)
                .ToListAsync();
            var mesaResponse = mesas.Select(x => new MesaResponseDto
            {
                Id = x.Id,
                Nombre = x.Nombre,
                EstadoMesa = x.EstadoMesa.Descripcion
            }).ToList();

            return Ok(new { Message = "Listado de mesas", Mesas = mesaResponse});
        }

        [HttpGet("GetById/{idMesa}")]
        public async Task<ActionResult<MesaResponseDto>> GetById(int idMesa)
        {
            return base.Ok(new { message = "Estos son los detalles de la mesa" });
        }

        [HttpPost("Create")]
        public async Task<ActionResult<MesaResponseDto>> CrearMesa([FromBody] MesaRequestDto mesa)
        {
            var nuevaMesa = new Mesa
            {
                EstadoMesaId = mesa.EstadoMesa,
                Nombre = mesa.Nombre,
            };
            _context.Mesas.Add(nuevaMesa);
            await _context.SaveChangesAsync();

            var estadoMesa = await _context.EstadoMesa.FindAsync(nuevaMesa.EstadoMesaId);
            var mesaResponse = new MesaResponseDto
            {
                Id = nuevaMesa.Id,
                EstadoMesa = estadoMesa.Descripcion,
                Nombre = nuevaMesa.Nombre
            };
            return Ok(new {message="Se agrego una mesa", Mesa=mesaResponse});
        }
        

        [HttpPost("Update")]
        public async Task<ActionResult<MesaResponseDto>> Update(int idMesa, MesaRequestDto mesa)
        {
            return base.Ok(new { message = "Mesa actualizada" });
        }

    }
}
