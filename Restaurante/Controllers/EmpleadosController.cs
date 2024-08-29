using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurante.Data;
using Restaurante.Dto;

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
            //return base.Ok(new {message = "Estos son todos los empleados"}); // Ajusta la lógica según sea necesario
                                                                             // Consulta de la base de datos para obtener todos los empleados
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
            return Ok(new { message = "Este es el empleado buscado por Id" });
        }

        [HttpPost("Create")]
        public async Task<ActionResult<EmpleadoResponseDto>> Create(EmpleadoRequestCreateDto empleado)
        {
            return Ok(new { Message = "Usted ha creado un empleado" });
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
