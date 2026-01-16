using Microsoft.EntityFrameworkCore;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Infrastructure.Persistence
{
    public sealed class OrderManagementDbContext : DbContext
    {
        public OrderManagementDbContext(DbContextOptions<OrderManagementDbContext> options) : base(options) { }

        public DbSet<Order> Orders => Set<Order>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(b =>
            {
                b.ToTable("orders");
                b.HasKey(x => x.Id);

                b.Property(x => x.Status).HasConversion<int>();
                b.Property(x => x.TotalAmount).HasColumnType("numeric(18,2)");
                b.Property(x => x.CreatedAt);

                b.OwnsMany(x => x.Items, ib =>
                {
                    ib.ToTable("order_items");
                    ib.WithOwner().HasForeignKey("order_id");
                    ib.Property<Guid>("id");
                    ib.HasKey("id");

                    ib.Property(x => x.ProductId).IsRequired();
                    ib.Property(x => x.Quantity).IsRequired();
                    ib.Property(x => x.UnitPrice).HasColumnType("numeric(18,2)");
                });
            });
        }
    }
}
