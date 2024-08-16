using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurante.Entities
{
    public class Mesa : ClaseBase
    {
        [ForeignKey(nameof(EstadoMesa))]
        public EstadoMesa EstadoMesa { get; set; }
        public string Nombre { get; set; }
    }
}
