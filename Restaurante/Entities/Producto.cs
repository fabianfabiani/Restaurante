using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurante.Entities
{
    public class Producto : ClaseBase
    {
        [ForeignKey(nameof(Sector))]
        public Sector Sector { get; set; }
        public string Descripcion { get; set; }
        public int Stock { get; set; } 
        public float Precio { get; set; }
    }
}
