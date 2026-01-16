using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Abstractions
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order, CancellationToken ct);
        Task<Order?> GetByIdAsync(Guid id, CancellationToken ct);
    }
}
