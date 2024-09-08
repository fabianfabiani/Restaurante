using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurante.Entities
{
    public class Pedido:ClaseBase
    {
        public int ProductoId { get; set; }
        [ForeignKey(nameof(ProductoId))]
        public Producto Producto { get; set; }

        public int ComandaId { get; set; }
        [ForeignKey(nameof(ComandaId))]
        public Comanda Comanda { get; set; }

        public int EstadoId { get; set; }
        [ForeignKey(nameof(EstadoId))]
        public EstadoPedido EstadoPedido { get; set; } 
        public int Cantidad { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaFinalizacion { get; set; }


    }
}
