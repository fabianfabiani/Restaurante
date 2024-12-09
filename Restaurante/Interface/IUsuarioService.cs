using Microsoft.AspNetCore.Mvc;
using Restaurante.Dto;

namespace Restaurante.Interface
{
    public interface IUsuarioService
    {
        public Task<List<UsuarioResponseDto>> GetAllUsuarios();

        public Task<ActionResult<UsuarioResponseDto>> GetById(int idEmpleado);

        public Task<ActionResult<UsuarioResponseDto>> Create(UsuarioRequestCreateDto empleado);

        public string LoguearUsuario(CredencialesDto credencialesDto);

    }
}
