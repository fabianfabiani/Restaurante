using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurante.Data;
using Restaurante.Dto;
using Restaurante.Entities;
using Restaurante.Interface;

namespace Restaurante.Service
{
    public class EmpleadoService : IEmpleadoService
    {
        private readonly DataBaseContext _context;
        public EmpleadoService(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<List<EmpleadoResponseDto>> GetAllEmpleados()
        {
            var empleados = await _context.Empleados.ToListAsync();

            // Mapea los empleados a EmpleadoResponseDto
            var empleadosResponseDto = empleados.Select(e => new EmpleadoResponseDto
            {
                Id = e.Id,
                Nombre = e.Nombre,
                Usuario = e.Usuario,
            }).ToList();

            // Retorna la lista de empleados mapeados a DTO
            return empleadosResponseDto;
        }

        public async Task<ActionResult<EmpleadoResponseDto>> GetById(int idEmpleado)
        {
            var empleado = await _context.Empleados.Include(x => x.Rol)
               .Where(e => e.Id == idEmpleado)
               .Select(e => new EmpleadoResponseDto
               {
                   Id = e.Id,
                   Nombre = e.Nombre,
                   Usuario = e.Usuario,
                   Rol = e.Rol.Descripcion
               })
               .FirstOrDefaultAsync();
            return empleado;
        }

        public async Task<ActionResult<EmpleadoResponseDto>> Create(EmpleadoRequestCreateDto empleado)
        {
            var nuevoEmpleado = new Empleado
            {
                Nombre = empleado.Nombre,
                Usuario = empleado.Usuario,
                Password = empleado.Password,
                RolId = empleado.IdRol,
                SectorId = empleado.IdSector
            };

            _context.Empleados.Add(nuevoEmpleado);
            _context.SaveChanges();

            var empleadoResponse = new EmpleadoResponseDto
            {
                Id = nuevoEmpleado.Id,
                Nombre = nuevoEmpleado.Nombre,
                Usuario = nuevoEmpleado.Usuario,
                Rol = (await _context.Roles.FindAsync(nuevoEmpleado.RolId)).Descripcion,
                Sector = (await _context.Sectores.FindAsync(nuevoEmpleado.SectorId)).descripcion
            };
            return empleadoResponse;
        }
    }
}
