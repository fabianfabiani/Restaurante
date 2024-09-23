using Microsoft.AspNetCore.Mvc;
using Restaurante.Dto;

namespace Restaurante.Interface
{
    public interface IPedidoService
    {
        public Task<ActionResult<List<PedidoResponseDto>>> GetAllPedidos();
        public Task<ActionResult<PedidoResponseDto>> CrearPedido(PedidoRequestDto pedido);

        public Task ActualizarEstadoPedido(int pedidoId);

    }
}
