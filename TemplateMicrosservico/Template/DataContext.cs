using Microsoft.EntityFrameworkCore;

namespace MyProject
{
    public class DataContext : DbContext 
    {
        

        public class Product
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public string Description { get; set; }

        }
        public DbSet<Product> Products { get; set; }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Exemplo>().HasKey(p => p.Id);
            

            base.OnModelCreating(modelBuilder);
        }
    }
}
