using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurante.Data;
using Restaurante.Dto;
using Restaurante.Entities;

namespace Restaurante.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadosController : Controller
    {
        protected readonly DataBaseContext _context; //Se Agrego para la coneccion con la BD
        public EmpleadosController(DataBaseContext context) //Se Agrego para la coneccion con la BD
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<List<EmpleadoResponseDto>>> GetAll()
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
            return Ok(empleadosResponseDto);
        }
    

        [HttpGet("GetById/{idEmpleado}")]
        public async Task<ActionResult<EmpleadoResponseDto>> GetById(int idEmpleado)
        {
            var empleado = await _context.Empleados.Include(x=>x.Rol)
                .Where(e => e.Id == idEmpleado)
                .Select(e => new EmpleadoResponseDto
                {
                    Id = e.Id,
                    Nombre = e.Nombre,
                    Usuario = e.Usuario,
                    Rol = e.Rol.Descripcion
                })
                .FirstOrDefaultAsync();
            return Ok(empleado);
        }

        [HttpPost("Create")]
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
            return Ok(new { message = "Usted ha creado un nuevo empleado", Empleado = empleadoResponse });
        }

        [HttpPut("Update/{idEmpleado}")]
        public async Task<ActionResult<EmpleadoResponseDto>> Update(int idEmpleado, EmpleadoRequestUpdateDto empleado)
        {
            return Ok(new { message = "Usted actualizo un empleado" });
        }

        [HttpDelete("Delete/{idEmpleado}")]
        public async Task<ActionResult<bool>> DeleteByIdEmpleado(int idEmpleado)
        {
            return Ok(new { message = "Se elimino un empleado" });
        }

        [HttpPost("Entrada/{idEmpleado}")]
        public async Task<ActionResult> RegistrarEntrada(int idEmpleado)
        {
            return Ok(new { message = "Se registro la entrada" });
        }
    }
}
