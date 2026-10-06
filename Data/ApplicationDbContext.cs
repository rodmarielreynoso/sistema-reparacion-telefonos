using Microsoft.EntityFrameworkCore;
using sistema_reparacion_telefonos.Models;

namespace sistema_reparacion_telefonos.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Dispositivo> Dispositivos { get; set; }

        public DbSet<Reparacion> Reparaciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Reparacion>()
                .Property(r => r.Precio)
                .HasPrecision(18, 2);
        }
    }
}