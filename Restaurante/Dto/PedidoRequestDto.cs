namespace Restaurante.Dto
{
    public class PedidoRequestDto
    {
        public int ProductoId { get; set; } //Codigo de producto que se esta pidiendo
        public int ComandaId { get; set; } //Representa el identificador unico de la comanda
        public string Estado { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaFinalizacion { get; set; }
        
    }
}
