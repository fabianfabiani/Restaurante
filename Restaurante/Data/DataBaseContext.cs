using Entidades;
using Microsoft.EntityFrameworkCore;
using Restaurante.Entities;

namespace Restaurante.Data
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext()
        {

        }

        public DataBaseContext(DbContextOptions<DataBaseContext> options)
        : base(options)

        {

        }

        public virtual DbSet<Comanda> Comandas { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<EstadoMesa> EstadoMesa { get; set; }
        public virtual DbSet<Mesa> Mesas { get; set; }
        public virtual DbSet<Pedido> Pedidos { get; set; }
        public virtual DbSet<Producto> Productos { get; set; }
        public virtual DbSet<Rol> Roles { get; set; }
        public virtual DbSet<Sector> Sectores { get; set; }
        public virtual DbSet<EstadoPedido> EstadoPedido { get; set; }

        public DbSet<Encuesta> Encuesta { get; set; }
       
    }


}
