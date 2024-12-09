using LaboratorioApi.Services;
using LaboratorioWeb.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace LaboratorioWeb.DTO
{
    public class CrearEncuestaDTO
    {
        public int PuntuacionMesa { get; set; }
        public int PuntuacionRestaurante { get; set; }
        public int PuntuacionMozo { get; set; }
        public int PuntuacionCocinero { get; set; }
        public string Comentario { get; set; }
        public DateTime FechaEncuesta { get; set; }
        public int? ComandaId { get; set; }
        public int? MesaId { get; set; }
    }
}

