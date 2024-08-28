using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurante.Entities
{
    public class Producto : ClaseBase
    {
        public int SectorId { get; set; }
        [ForeignKey(nameof(SectorId))]
        public Sector Sector { get; set; }
        public string Descripcion { get; set; }
        public int Stock { get; set; } 
        public float Precio { get; set; }
    }

}
