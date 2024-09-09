using Microsoft.AspNetCore.Mvc;
using Restaurante.Dto;

namespace Restaurante.Interface
{
    public interface IMesaService
    {
        public Task<ActionResult<List<MesaResponseDto>>> GeTAll();
        public Task<ActionResult<MesaResponseDto>> CrearMesa([FromBody] MesaRequestDto mesa);
    }
}
