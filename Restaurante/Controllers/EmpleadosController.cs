using Microsoft.AspNetCore.Mvc;
using Restaurante.Dto;

namespace Restaurante.Controllers
{
    [Route("api/controller")]
    [ApiController]
    public class EmpleadosController : Controller
    {
        [HttpGet("GetAll")]
        public async Task<ActionResult<List<EmpleadoResponseDto>>> GetAll()
        {
            return base.Ok();
        }

        [HttpGet("GetById")]
        public async Task<ActionResult<EmpleadoResponseDto>> GetById(int idEmpleado)
        {
            return base.Ok(new { message = "Este es el empleado buscado por Id" });
        }

        [HttpPost("Create")]
        public async Task<ActionResult<EmpleadoResponseDto>> Create(EmpleadoRequestCreateDto empleado)
        {
            return base.Ok(new { Message = "Usted ha creado un empleado" });
        }

        [HttpPut("Update")]
        public async Task<ActionResult<EmpleadoResponseDto>> Update(int idEmpleado, EmpleadoRequestUpdateDto empleado)
        {
            return base.Ok(new { message = "Usted actualizo un empleado" });
        }

        [HttpDelete("Delete")]
        public async Task<ActionResult<bool>> DeleteByIdEmpleado(int idEmpleado)
        {
            return base.Ok(new { message = "Se elimino un empleado" });
        }

        [HttpPost("Entrada/{empleadoId}")]
        public async Task<ActionResult> RegistrarEntrada(int idEmpleado)
        {
            return base.Ok(new { message = "Se registro la entrada" });
        }

    }
}
