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


// El enunciado dice que las mesas tienen un codigo de identificador unico, pero en el diagrama de base de datos no aparece ese atributo