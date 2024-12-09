using AutoMapper;
using Entidades;
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
        private readonly IMapper _mapper;
        public MesaService(DataBaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ActionResult<List<MesaListarDTO>>> GeTAll()
        {
            var mesas = await _context.Mesas
                .Include(c => c.EstadoMesa)
                .ToListAsync();

            var mesaResponse = _mapper.Map<List<MesaListarDTO>>(mesas);

            return mesaResponse;
        }
        public async Task CrearMesa([FromBody] MesaRequestDto mesa)
        {
           

            var nuevaMesa = _mapper.Map<Mesa>(mesa);
            nuevaMesa.EstadoMesaId = 5;
            _context.Mesas.Add(nuevaMesa);
            await _context.SaveChangesAsync();

        }

        // Obtener la mesa más usada (más encuestas asociadas)
       public async Task<Mesa?> ObtenerMesaMasUsadaAsync()
{
    var mesaMasUsada = await _context.Encuesta
        .Where(e => e.MesaId.HasValue) // Ignorar registros con MesaId nulo
        .GroupBy(e => e.MesaId)
        .OrderByDescending(g => g.Count())
        .Select(g => new
        {
            MesaId = g.Key,
            CantidadEncuestas = g.Count()
        })
        .FirstOrDefaultAsync();

    if (mesaMasUsada == null || mesaMasUsada.MesaId == null)
        return null; // No hay encuestas o mesas asociadas.

    // Buscar la mesa correspondiente.
    var mesa = await _context.Mesas.FirstOrDefaultAsync(m => m.Id == mesaMasUsada.MesaId);

    return mesa; // Puede ser null si no se encontró.
}

        // Obtener la mesa menos usada (menos encuestas asociadas)
        public async Task<Mesa> ObtenerMesaMenosUsadaAsync()
        {
            // Obtener la mesa con menos encuestas asociadas
            var mesaMenosUsada = await _context.Encuesta
                .GroupBy(e => e.MesaId)
                .OrderBy(g => g.Count())  // Ordenar de menor a mayor por cantidad de encuestas
                .Select(g => new
                {
                    MesaId = g.Key,
                    CantidadEncuestas = g.Count()
                })
                .FirstOrDefaultAsync();

            if (mesaMenosUsada == null)
            {
                return null;  // No hay mesas con encuestas
            }

            // Buscar la mesa en la base de datos
            var mesa = await _context.Mesas.FindAsync(mesaMenosUsada.MesaId);
            return mesa;
        }

        public async Task<object> ObtenerMejorComentarioAsync()
        {
            var mejorEncuesta = await _context.Encuesta
                .OrderByDescending(e => e.PuntuacionMesa)  // Ordenar solo por PuntuacionMesa
                .FirstOrDefaultAsync();

            if (mejorEncuesta == null)
            {
                return null;  // Si no se encuentra la encuesta, devolver null
            }

            var mesa = await _context.Mesas.FindAsync(mejorEncuesta.MesaId); // Obtener la mesa asociada

            return new
            {
                MesaId = mesa?.Id,  // Si la mesa es encontrada, devolver el ID
                Comentario = mejorEncuesta.Comentario
            };
        }

        public async Task<object> ObtenerPeorComentarioAsync()
        {
            var peorEncuesta = await _context.Encuesta
                .OrderBy(e => e.PuntuacionMesa)  // Ordenar por PuntuacionMesa en orden ascendente
                .FirstOrDefaultAsync();

            if (peorEncuesta == null)
            {
                return null;  // Si no hay encuestas, devolver null
            }

            var mesa = await _context.Mesas.FindAsync(peorEncuesta.MesaId); // Obtener la mesa correspondiente a la encuesta

            return new
            {
                MesaId = mesa?.Id,  // Si la mesa es encontrada, se devuelve el ID
                Comentario = peorEncuesta.Comentario
            };
        }
    }
}

// Verificar si el EstadoMesaId existe en la base de datos
//Mesa? m = _context.Mesas.Where(m => m.EstadoMesaId == mesa.EstadoMesaId).FirstOrDefault();
/*Mesa? m = _context.Mesas.Where(m => m.Id == mesa.Id).FirstOrDefault();
if (m == null)
{
    throw new Exception("EstadoMesaId inexistente");
}*/