using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using sistema_reparacion_telefonos.Models;

namespace sistema_reparacion_telefonos.Data
{
    public class ApplicationDbContext
        : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Tablas del sistema de reparación
        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Dispositivo> Dispositivos { get; set; }

        public DbSet<Reparacion> Reparaciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de las tablas de Identity
            base.OnModelCreating(modelBuilder);

            // Relación opcional entre clientes e Identity.
            modelBuilder.Entity<Cliente>()
                .Property(c => c.IdentityUserId)
                .HasMaxLength(450);

            modelBuilder.Entity<Cliente>()
                .HasIndex(c => c.IdentityUserId)
                .IsUnique()
                .HasFilter("[IdentityUserId] IS NOT NULL");

            modelBuilder.Entity<Cliente>()
                .HasOne<IdentityUser>()
                .WithOne()
                .HasForeignKey<Cliente>(c => c.IdentityUserId)
                .OnDelete(DeleteBehavior.SetNull);

            // Configuración del precio de las reparaciones
            modelBuilder.Entity<Reparacion>()
                .Property(r => r.Precio)
                .HasPrecision(18, 2);

            // Configuración del código de seguimiento
            modelBuilder.Entity<Reparacion>()
                .Property(r => r.CodigoSeguimiento)
                .HasMaxLength(50)
                .IsRequired();

            // Cada reparación debe tener un código único
            modelBuilder.Entity<Reparacion>()
                .HasIndex(r => r.CodigoSeguimiento)
                .IsUnique();
        }
    }
}