using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurante.Data;
using Restaurante.Dto;
using Restaurante.Entities;
using Restaurante.Interface;

namespace Restaurante.Service
{
    public class MesaService : IMesaService
    {
        private readonly DataBaseContext _context;
        public MesaService(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<ActionResult<List<MesaResponseDto>>> GeTAll()
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

            return mesaResponse;
        }
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
            return mesaResponse;
        }
    }
}
