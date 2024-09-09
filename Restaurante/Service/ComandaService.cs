using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurante.Data;
using Restaurante.Dto;
using Restaurante.Entities;
using Restaurante.Interface;

namespace Restaurante.Service
{
    public class ComandaService : IComandaService
    {
        private readonly DataBaseContext _context;
        public ComandaService(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<ActionResult<ComandaResponseDto>> CrearComanda(ComandaRequestDto comanda)
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
            return comandaResponse;
        }
    }
}
