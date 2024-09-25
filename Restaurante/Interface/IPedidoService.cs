using Microsoft.AspNetCore.Mvc;
using Restaurante.Dto;
using Restaurante.Entities;

namespace Restaurante.Interface
{
    public interface IPedidoService
    {
        public Task<ActionResult<List<PedidoListarDTO>>> GetAllPedidos();
        public Task CrearPedido(PedidoRequestDto pedido);
        public Task<List<PedidoListarDTO>> ListarPedidosPendientesPorEmpleado(int idEmpleado);
        public Task ActualizarEstadoPedido(int pedidoId);
        public Task CambiarEstadoEnPreparacion(int pedidoId, DateTime tiempoPreparacion);

    }
}
