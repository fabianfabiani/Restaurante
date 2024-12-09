using AutoMapper;
using Entidades;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurante.Data;
using Restaurante.Dto;
using Restaurante.Entities;
using Restaurante.Interface;

using Microsoft.EntityFrameworkCore;

namespace LaboratorioApi.Services
{
    public class EncuestaService : IEncuestaService
    {
        private readonly DataBaseContext _context;
        private readonly IMapper _mapper;

        public EncuestaService(DataBaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Encuesta>> GetEncuestasAsync()
        {
            return await _context.Set<Encuesta>().ToListAsync();
        }

        public async Task<Encuesta> GetEncuestaByIdAsync(int id)
        {
            return await _context.Set<Encuesta>().FindAsync(id);
        }

        public async Task<Encuesta> CreateEncuestaAsync(Encuesta nuevaEncuesta)
        {
            _context.Set<Encuesta>().Add(nuevaEncuesta);
            await _context.SaveChangesAsync();
            return nuevaEncuesta;
        }

        public async Task<bool> DeleteEncuestaAsync(int id)
        {
            var encuesta = await _context.Set<Encuesta>().FindAsync(id);
            if (encuesta == null) return false;

            _context.Set<Encuesta>().Remove(encuesta);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<string>> ValidarCampos(Encuesta Encuesta)
        {
            List<string> ListErrores = new List<string>();

            if (Encuesta == null)
            {
                ListErrores.Add("Los datos de la encuesta son nulos");
                return ListErrores;
            }

            if (Encuesta.PuntuacionMesa < 1 || Encuesta.PuntuacionMesa > 10) ListErrores.Add("Puntuación de mesa fuera de rango");

            if (Encuesta.PuntuacionRestaurante < 1 || Encuesta.PuntuacionRestaurante > 10) ListErrores.Add("Puntuación de restaurante fuera de rango");

            if (Encuesta.PuntuacionMozo < 1 || Encuesta.PuntuacionMozo > 10) ListErrores.Add("Puntuación de Mozo fuera de rango");

            if (Encuesta.PuntuacionCocinero < 1 || Encuesta.PuntuacionCocinero > 10) ListErrores.Add("Puntuación de Cocinero fuera de rango");

            if (Encuesta.Comentario.Length > 66) ListErrores.Add("El comentario de la encuesta posee demasiados caracteres");

            if (Encuesta.FechaEncuesta.Date != DateTime.Now.Date) ListErrores.Add("La fecha de encuesta debe de ser del dia de hoy");

            return ListErrores;
        }
    }
}
