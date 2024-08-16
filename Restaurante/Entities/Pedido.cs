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
        public Estado Estado { get; set; }
        public int Cantidad { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime FechaFinalizacion { get; set; }


    }
}
