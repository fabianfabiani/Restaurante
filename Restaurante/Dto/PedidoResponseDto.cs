namespace Restaurante.Dto
{
    public class PedidoResponseDto
    {
        public string Producto { get; set; }
        public string CodigoComanda { get; set; } //referente a la entidad Comanda
        public string Estado { get; set; } //referente a la entidad EstadoMesa 
        public int Cantidad { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaFinalizacion { get; set; }
        
    }
}
