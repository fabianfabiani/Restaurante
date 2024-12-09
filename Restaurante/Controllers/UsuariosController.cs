using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurante.Data;
using Restaurante.Dto;
using Restaurante.Entities;
using Restaurante.Interface;
                               //En lugar de crear una instancia de UsuarioService directamente dentro del controlador ejemplo:
                               // var usuarioService = new EmpleadoService();
                               //La inyeccion de dependencia permite que el controlador reciba una implementacion
                               //ya construida de IEmpleadoService desde un contenedor de dependencias(en este caso
                               //configurado en Program)
namespace Restaurante.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : Controller
    {

        private readonly IUsuarioService _usuarioService; //campo privado de solo lectura de tipo IUsuarioService,
                                                            //sirve para guardar la instancia del servicio que sera usado
                                                            //dentro del controlador

        public UsuariosController(IUsuarioService usuarioService) //El servicio se inyecta mediante el constructor
        {
            _usuarioService = usuarioService; //El servicio es almacenado en el campo privado
        }

        [HttpGet("GetAll")] //Ahora podemos usar _usuarioService en los metodos del controlador
        public async Task<ActionResult<List<UsuarioResponseDto>>> GetAll()
        {
            var usuarioResponseDto = await _usuarioService.GetAllUsuarios();
            return Ok(new { message = "Estos son todos los usuarios: ", Usuario = usuarioResponseDto });
        }
    
        
        [HttpGet("GetById/{idUsuario}")]
        public async Task<ActionResult<UsuarioResponseDto>> GetById(int idUsuario)
        {
           var usuarioResponseDto = await _usuarioService.GetById(idUsuario);
            return Ok(new { message = "El usuario buscado es: ", Usuario = usuarioResponseDto });
        }
        
        [HttpPost("Create")]
        public async Task<ActionResult<UsuarioResponseDto>> Create(UsuarioRequestCreateDto usuario)
        {
            var usuarioResponseDto = await _usuarioService.Create(usuario);
            return Ok(new { message = "Usted ha creado un nuevo usuario", Usuario = usuarioResponseDto });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] CredencialesDto credenciales)
        {
            return this.Ok(new { token = this._usuarioService.LoguearUsuario(credenciales) });
        }



        /*
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
        */
    }
}

