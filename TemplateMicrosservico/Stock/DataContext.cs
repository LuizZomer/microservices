using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Emit;
using System.Text.Json.Serialization;

namespace Exemplo
{
    public class DataContext : DbContext
    {
        public class StockItem
        {
            public int Id { get; set; }
            public int Quantity { get; set; }
            public int ProductId { get; set; }
            public string ProductName { get; set; }
        }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        
        public DbSet<StockItem> Stocks{ get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Exemplo>().HasKey(p => p.Id);
            

            base.OnModelCreating(modelBuilder);
        }
    }
}
