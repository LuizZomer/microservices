using Microsoft.EntityFrameworkCore;
using System;

namespace Exemplo
{
    public class DataContext : DbContext
    {
        public class OrderItem
        {
            public int Id { get; set; }
            public int StockId { get; set; }

            public DateTime OrderDate { get; set; } // Armazena a data do pedido.

            public string Status { get; set; } = "Pending"; // Armazena o status do pedido (verdadeiro ou falso).
        }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<OrderItem> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define o valor padrão para OrderDate como a data/hora atual do banco.
            modelBuilder.Entity<OrderItem>()
                .Property(o => o.OrderDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Define que Status é obrigatório (não permite nulo).
            modelBuilder.Entity<OrderItem>()
                .Property(o => o.Status)
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }
}
