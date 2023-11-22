using Microsoft.EntityFrameworkCore;
using SolforbTest.Models;

namespace SolforbTest.DBContext
{
    public class ApplicationDBcontext : DbContext
    {
        public ApplicationDBcontext(DbContextOptions<ApplicationDBcontext> options) : base(options)
        {

        }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderItem> OrderItem { get; set; }
        public virtual DbSet<Provider> Provider { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
            .HasIndex(o => new { o.Number, o.ProviderId }).IsUnique();  
        }
    }
}
