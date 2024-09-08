using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurante.Data;
using Restaurante.Dto;
using Restaurante.Entities;

namespace Restaurante.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComandaController : Controller
    {
        private readonly DataBaseContext _context;
        public ComandaController(DataBaseContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<ActionResult> CrearComanda(ComandaRequestDto comanda)
        {
            var nuevaComanda = new Comanda()
            {
                MesaId = comanda.MesaId,
                nombreCliente = comanda.nombreCliente,
                codigoComanda = comanda.codigoComanda
            };
            _context.Comandas.Add(nuevaComanda);
            await _context.SaveChangesAsync();

            var mesaConEstado = await _context.Mesas
                .Include(x => x.EstadoMesa)
                .FirstOrDefaultAsync(m => m.Id == nuevaComanda.MesaId);

            var comandaResponse = new ComandaResponseDto()
            {
                Id = nuevaComanda.Id,
                NombreMesa = mesaConEstado?.Nombre,
                EstadoMesaDescripcion = mesaConEstado?.EstadoMesa.Descripcion,
                NombreCliente = nuevaComanda.nombreCliente,
                CodigoComanda = nuevaComanda.codigoComanda
            };
            return Ok(new { message = "Se creo una nueva comanda", Comanda = comandaResponse });

        }
    }
}
