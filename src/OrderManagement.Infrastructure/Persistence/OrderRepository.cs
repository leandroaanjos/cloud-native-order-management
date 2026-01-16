using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Abstractions;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Infrastructure.Persistence
{
    public sealed class OrderRepository : IOrderRepository
    {
        private readonly OrderManagementDbContext _db;

        public OrderRepository(OrderManagementDbContext db) => _db = db;

        public async Task AddAsync(Order order, CancellationToken ct)
        {
            await _db.Orders.AddAsync(order, ct);
            await _db.SaveChangesAsync(ct);
        }

        public Task<Order?> GetByIdAsync(Guid id, CancellationToken ct)
            => _db.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == id, ct);
    }
}
