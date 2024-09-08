using System.ComponentModel.DataAnnotations;

namespace Restaurante.Dto
{
    public class PedidoRequestDto
    {
        public int ProductoId { get; set; } //Codigo de producto que se esta pidiendo
        public int ComandaId { get; set; } //Representa el identificador unico de la comanda
        public int EstadoId { get; set; }
        public int Cantidad { get; set; }
        [DataType(DataType.Date)] // Para que solo se use la fecha en Swagger
        public DateTime FechaCreacion { get; set; }
        [DataType(DataType.Date)] // Para que solo se use la fecha en Swagger
        public DateTime? FechaFinalizacion { get; set; }
        
    }
}
