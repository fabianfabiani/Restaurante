using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurante.Entities
{
    public class Empleado : ClaseBase
    {
        [ForeignKey(nameof(Sector))]
        public Sector Sector { get; set; }

        [ForeignKey(nameof(Rol))]
        public Rol Rol { get; set; }

        public string Nombre { get; set; }
        public string Usuario { get; set; }
        public string Password { get; set; }
        public int IdSector { get; set; }
        public int IdRol { get; set; }

    }
}
