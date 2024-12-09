using AutoMapper;
using Azure.Messaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Restaurante.Data;
using Restaurante.Dto;
using Restaurante.Entities;
using Restaurante.Interface;

namespace Restaurante.Service
{
    public class PedidoService : IPedidoService
    {
        private readonly DataBaseContext _context;
        private readonly IMapper _mapper;
        public PedidoService(DataBaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ActionResult<List<PedidoListarDTO>>> GetAllPedidos()
        {
            
            var pedidos = await _context.Pedidos
                .Include(c => c.Producto)  
                .Include(c => c.EstadoPedido)
                .Include(c => c.Producto.Sector)
                .Include(c => c.Comanda)
                .ThenInclude(comanda => comanda.Mesa)
                .ThenInclude(mesa => mesa.EstadoMesa)
                .ToListAsync();

            
            var respuesta = _mapper.Map<List<PedidoListarDTO>>(pedidos);

            return respuesta;
        }

        public async Task<string> CrearPedido(PedidoRequestDto pedido)
        {
            Producto? p = _context.Productos.FirstOrDefault(p => p.Id == pedido.ProductoId);
            if (p == null)
            {
                throw new Exception("Producto inexistente, no puede cargar pedido");
            }
            p.ReducirStock(pedido.Cantidad);
            _context.Productos.Update(p);

            var nuevoPedido = _mapper.Map<Pedido>(pedido);

            // Genera el código único
            nuevoPedido.CodigoPedido = GenerarCodigoUnico();

            nuevoPedido.EmpleadoModificadorId = pedido.EmpleadoId;

            _context.Pedidos.Add(nuevoPedido);
            //Guarda todos los cambios en la base de datos
            await _context.SaveChangesAsync();

            return nuevoPedido.CodigoPedido; // Retorna el código generado
        }

        private string GenerarCodigoUnico()
        {
            // Obtén el último código en la base de datos para incrementar
            var ultimoPedido = _context.Pedidos
                .OrderByDescending(p => p.CodigoPedido)
                .FirstOrDefault();

            // Si no hay pedidos, comienza con P0001
            if (ultimoPedido == null)
            {
                return "P0001";
            }

            // Extrae el número del último código
            if (int.TryParse(ultimoPedido.CodigoPedido.Substring(1), out int ultimoNumero))
            {
                return $"P{ultimoNumero + 1:D4}"; // Incrementa el número
            }

            // Si el código no es válido, devuelve un código por defecto
            return "P0001";
        }

        public async Task ActualizarEstadoPedido(int pedidoId)
        {
            // Recuperamos el pedido desde la base de datos
            var pedido = await _context.Pedidos.FindAsync(pedidoId);
            if (pedido == null)
            {
                throw new Exception("Pedido no encontrado.");
            }

            // Recuperamos el estado actual del pedido desde la base de datos
            var estadoActual = await _context.EstadoPedido.FindAsync(pedido.EstadoId);
            if (estadoActual == null)
            {
                throw new Exception("Estado del pedido no encontrado.");
            }

            // Verificamos si el estado actual es 'Finalizado'
            if (estadoActual.Descripcion == "finalizado")
            {
                throw new Exception("No se puede actualizar el estado de un pedido finalizado.");
            }

            // Si el estado actual es 'En Preparación', actualizamos la fecha de finalización
            if (estadoActual.Descripcion == "en preparacion")
            {
                pedido.FechaFinalizacion = DateTime.Now;
            }

            // Obtener el siguiente estado (según la lógica de negocio)
            var siguienteEstado = await _context.EstadoPedido
                .OrderBy(e => e.Id)
                .FirstOrDefaultAsync(e => e.Id > pedido.EstadoId);

            if (siguienteEstado == null)
            {
                throw new Exception("No hay un siguiente estado disponible.");
            }

            // Actualizar el EstadoId del pedido con el nuevo estado
            pedido.EstadoId = siguienteEstado.Id;
            pedido.EstadoPedido = siguienteEstado;

            // Guardamos los cambios en la base de datos
            await _context.SaveChangesAsync();
        }
        public async Task<List<PedidoListarDTO>> ListarPedidosPendientesPorEmpleado(int idEmpleado)
        {
            var empleado = await _context.Usuarios.Include(e => e.Sector).FirstOrDefaultAsync(e => e.Id == idEmpleado);

            if (empleado == null)
            {
                throw new Exception("Empleado no encontrado.");
            }

            var pedidosPendientes = await _context.Pedidos
                .Include(p => p.Producto)
                .Include(p => p.EstadoPedido)
                .Where(p => p.Producto.SectorId == empleado.SectorId && p.EstadoPedido.Descripcion == "pendiente")
                .ToListAsync();

            return _mapper.Map<List<PedidoListarDTO>>(pedidosPendientes);

        }

        public async Task CambiarEstadoEnPreparacion(string codigoPedido, int empleadoId, DateTime tiempoPreparacion) // Cambiar estado a en preparación
        {
            // Buscar el pedido en la base de datos
            var pedido = await _context.Pedidos.FirstOrDefaultAsync(p => p.CodigoPedido == codigoPedido);
            if (pedido == null)
            {
                throw new Exception("Pedido no encontrado");
            }

            // Obtener el estado "pendiente" (asumiendo que el ID correspondiente es 2)
            var estadoPendiente = await _context.EstadoPedido
                .FirstOrDefaultAsync(e => e.Id == 2); // Cambiar "en preparación" por el ID del estado pendiente
            if (estadoPendiente == null)
            {
                throw new Exception("Estado 'pendiente' no encontrado");
            }

            // Actualizar el estado y el tiempo estimado
            pedido.EstadoId = estadoPendiente.Id; // Asignar el nuevo EstadoId
            pedido.FechaEstimadaDeFinalizacion = tiempoPreparacion; // Asignar el tiempo estimado
            pedido.EmpleadoModificadorId = empleadoId; // Asignar el empleado que modifica

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();
        }

        public async Task CambiarEstadoListoParaServir(string codigoPedido, int empleadoId)
        {
            // Buscar el pedido en la base de datos
            var pedido = await _context.Pedidos.FirstOrDefaultAsync(p => p.CodigoPedido == codigoPedido);
            if (pedido == null)
            {
                throw new Exception("Pedido no encontrado.");
            }

            // Obtener el estado "listo para servir"
            var estadoListoParaServir = await _context.EstadoPedido
                .FirstOrDefaultAsync(e => e.Descripcion == "listo para servir");
            if (estadoListoParaServir == null)
            {
                throw new Exception("Estado 'listo para servir' no encontrado.");
            }

            // Cambiar el estado a "listo para servir"
            pedido.EstadoId = estadoListoParaServir.Id; // Cambiar a "listo para servir"
            pedido.FechaFinalizacion = DateTime.Now; // Establecer fecha de finalización
            pedido.EmpleadoModificadorId = empleadoId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Error al actualizar el pedido.", ex);
            }
        }

        //public async Task<PedidoListarDTO> ObtenerTiempoDeDemora(int mesaId, string codigoPedido)
        public async Task<string> ObtenerTiempoDeDemora(int mesaId, string codigoPedido)
        {
            // Buscar el pedido en base a mesaId y codigoPedido
            var pedido = await _context.Pedidos
                .Include(p => p.Comanda) // Incluye la relación para obtener el nombre del cliente
                .Include(p => p.EstadoPedido) // Incluye el estado del pedido
                .Include(p => p.Producto.Sector) // Incluye el sector del producto
                .FirstOrDefaultAsync(p => p.Comanda.MesaId == mesaId && p.CodigoPedido == codigoPedido);

            if (pedido == null)
            {
                throw new Exception("Pedido no encontrado.");
            }
            if(pedido.EstadoPedido.Id== 3)
            {
                return $"El pedido {pedido.CodigoPedido} ya fue entregado";
            }
            else
            {
                // Mapeo de Pedido a PedidoListarDTO usando AutoMapper
                var pedidoDTO = _mapper.Map<PedidoListarDTO>(pedido);

                // Retorna el DTO con la información del pedido, nombre del cliente y tiempo transcurrido
                pedidoDTO.nombreCliente = pedido.Comanda.nombreCliente;

                // return pedidoDTO;
                return $"{pedidoDTO.nombreCliente}, el tiempo de demora de su pedido {pedidoDTO.CodigoPedido} es: {pedidoDTO.TiempoTranscurrido}";
            }
        }

        public async Task<List<PedidoListarDTO>> GetPedidosEnPreparacionConDemoras()
        {
            // Obtener los pedidos con EstadoId == 2 (En preparación)
            var pedidos = await _context.Pedidos
                .Include(p => p.EstadoPedido)
                .Where(p => p.EstadoId == 2) // Filtrar por el estado "En preparación"
                .ToListAsync();

            // Mapear la lista completa de pedidos a PedidoListarDTO
            var pedidosConDemoras = pedidos.Select(p => new PedidoListarDTO
            {
                CodigoPedido = p.CodigoPedido,
                FechaEstimadaDeFinalizacion = p.FechaEstimadaDeFinalizacion,
                FechaFinalizacion = p.FechaFinalizacion
            }).ToList();

            return pedidosConDemoras;
        }

        public async Task<ActionResult<string>> SiListoParaServirCambiarEstadoMesa(int mesaId)
        {
            var pedidosMesa = await _context.Pedidos
                .Include(p => p.Comanda)
                .Where(p => p.Comanda.MesaId == mesaId)
                .ToListAsync();

            // Verificar si no existen pedidos para la mesa solicitada
            if (!pedidosMesa.Any())
            {
                return "No existen pedidos en la mesa solicitada.";
            }

            // Verificar si hay algún pedido en estado "en preparación" (EstadoId == 2) o "pendiente" (Estado == 1)
            if (pedidosMesa.Any(p => p.EstadoId == 2 || p.EstadoId == 1))
            {
                return "Aún faltan entregar pedidos.";
            }

            // Verificar si todos los pedidos están listos para servir (EstadoId == 3)
            if (pedidosMesa.All(p => p.EstadoId == 3))
            {
                var mesa = await _context.Mesas.FindAsync(mesaId);
                if (mesa != null)
                {
                    mesa.EstadoMesaId = 2; // Cambiar estado a "cliente comiendo"
                    _context.Mesas.Update(mesa);
                    await _context.SaveChangesAsync();
                    return "Estado de la mesa actualizado a 'cliente comiendo'.";
                }
            }

            if (pedidosMesa.Any(p => p.EstadoId == 4))
            {
                return "Mesa cerrada, no hay pedidos!";
            }

            return "No hay pedidos listos para servir o la mesa no existe.";
        }

        public async Task<ActionResult<float>> CambiarEstadoMesaYCalcularTotal(string codigoComanda)
        {
            // Buscar la comanda por el código
            var comanda = await _context.Comandas
                .Include(c => c.Mesa)
                .Include(c => c.Mesa.EstadoMesa) // Incluir el estado de la mesa
                .FirstOrDefaultAsync(c => c.codigoComanda == codigoComanda);

            if (comanda == null)
            {
                throw new Exception("No se encontró la comanda.");
            }

            // Verificar si el estado de la mesa es "cliente comiendo"
            if (comanda.Mesa.EstadoMesaId != 2) // Id 2 = "cliente comiendo"
            {
                throw new Exception("El cliente aún no está en estado 'cliente comiendo'.");
            }

            // Obtener todos los pedidos asociados a la comanda
            var pedidos = await _context.Pedidos
                .Include(p => p.Producto)
                .Where(p => p.ComandaId == comanda.Id)
                .ToListAsync();

            if (!pedidos.Any())
            {
                throw new Exception("No se encontraron pedidos asociados a esta comanda.");
            }

            // Calcular el total de los pedidos
            float total = pedidos.Sum(p => p.Cantidad * p.Producto.Precio);

            // Cambiar el estado de la mesa a "cliente pagando"
            comanda.Mesa.EstadoMesaId = 3; // Id 3 = "cliente pagando"
            _context.Mesas.Update(comanda.Mesa);

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();

            // Devolver el total calculado
            return total;
        }

        public async Task<string> CambiarEstadoMesaCerrada(string codigoComanda)
        {
            var comanda = await _context.Comandas
                .Include(c => c.Mesa)
                .Include(c => c.Mesa.EstadoMesa)
                .FirstOrDefaultAsync(c => c.codigoComanda == codigoComanda);

            if (comanda == null)
            {
                throw new Exception("Comanda no existe.");
            }

            if (comanda.Mesa.EstadoMesaId == 4)
            {
                throw new Exception("La mesa ya está cerrada.");
            }

            if (comanda.Mesa.EstadoMesaId == 3)
            {
                // Cambiar el estado de la mesa a "cerrada"
                comanda.Mesa.EstadoMesaId = 4;
                _context.Mesas.Update(comanda.Mesa);
                await _context.SaveChangesAsync();
                return "La mesa ha sido cerrada correctamente.";
            }
            return "No se pudo cerrar la mesa ya que no esta en estado 'cliente pagando'.";

        }












    }
}
