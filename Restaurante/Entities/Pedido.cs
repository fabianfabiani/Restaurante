using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurante.Entities
{
    public class Pedido:ClaseBase
    {
        [ForeignKey(nameof(Producto))]
        public Producto Producto { get; set; }

        [ForeignKey(nameof(Comanda))]
        public Comanda Comanda { get; set; }

        [ForeignKey(nameof(Estado))]
        public EstadoMesa Estado { get; set; } //Se corrijio
        public int Cantidad { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime FechaFinalizacion { get; set; }


    }
}
