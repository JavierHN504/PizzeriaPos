using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PizzeriaPos.Core.Entities;

namespace PizzeriaPos.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Producto> Productos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Direccion> Direcciones { get; set; }
        public DbSet<PedidoCabecera> PedidosCabecera { get; set; }
        public DbSet<PedidoDetalle> PedidosDetalle { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Filtros globales de soft delete
            modelBuilder.Entity<Producto>().HasQueryFilter(p => !p.Deleted);
            modelBuilder.Entity<Cliente>().HasQueryFilter(c => !c.Deleted);
            modelBuilder.Entity<Direccion>().HasQueryFilter(d => !d.Deleted);
            modelBuilder.Entity<PedidoCabecera>().HasQueryFilter(p => !p.Deleted);
            modelBuilder.Entity<PedidoDetalle>().HasQueryFilter(p => !p.Deleted);

            // Subtotal es calculado, no se guarda en Base de Datos
            modelBuilder.Entity<PedidoDetalle>()
                .Ignore(pd => pd.Subtotal);
        }
    }
}