using System.ComponentModel.DataAnnotations;

namespace LaboratorioWeb.Interfaces
{
    public interface IEncuesta
    {
        public int Id { get; set; }  // Llave primaria

        public int PuntuacionMesa { get; set; }

        public int PuntuacionRestaurante { get; set; }

        public int PuntuacionMozo { get; set; }

        public int PuntuacionCocinero { get; set; }
        
        public string Comentario { get; set; }

        public DateTime FechaEncuesta { get; set; }
        public int? ComandaId { get; set; } // Id de la Comanda, acepta null
        public int? MesaId { get; set; } // Id de la Mesa, acepta null
    }
}
