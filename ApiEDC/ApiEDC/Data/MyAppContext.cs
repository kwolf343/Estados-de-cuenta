using ApiEDC.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiEDC.Data
{
    public class MyAppContext : DbContext
    {
        public MyAppContext(DbContextOptions<MyAppContext> options)
        : base(options)
        {
        }
        public DbSet<EstadoCuenta> EstadoCuenta { get; set; }

        public DbSet<DetalleEstadoCuenta> DetalleEstadoCuenta { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EstadoCuenta>()
                .HasMany(e => e.DetalleEstadoCuenta)
                .WithOne()
                .HasForeignKey(d => d.IdEstadoCuenta);

            base.OnModelCreating(modelBuilder);
        }

    }
}
