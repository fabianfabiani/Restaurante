using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurante.Entities
{
    public class Comanda:ClaseBase
    {
        [ForeignKey(nameof(Mesa))]
        public Mesa Mesa { get; set; }
        public string nombreCliente { get; set; }
        public string codigoComanda { get; set; } //Se agrego codigo unico alfanumerico de 5 caracteres
    }
}
