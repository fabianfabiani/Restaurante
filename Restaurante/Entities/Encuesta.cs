using Restaurante.Entities;
using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class Encuesta
    {
        [Key]
        public int Id { get; set; }  // Llave primaria

        public int PuntuacionMesa { get; set; }

        public int PuntuacionRestaurante { get; set; }

        public int PuntuacionMozo { get; set; }

        public int PuntuacionCocinero { get; set; }

        public string Comentario { get; set; }

        public DateTime FechaEncuesta { get; set; } = DateTime.Now;  // Fecha de creación de la encuesta
        public int? ComandaId { get; set; } // Id de la Comanda, acepta null
        public int? MesaId { get; set; } // Id de la Mesa, acepta null

        public Mesa Mesa { get; set; } // Relación con la entidad Mesa
    }
}