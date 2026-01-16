namespace OrderManagement.Domain.Entities
{
    public sealed class Order
    {
        public Guid Id { get; private set; }
        public OrderStatus Status { get; private set; }
        public decimal TotalAmount { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }

        private readonly List<OrderItem> _items = new();
        public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

        private Order() { } // For EF Core

        public Order(Guid id, IEnumerable<OrderItem> items)
        {
            if (id == Guid.Empty) throw new ArgumentException("Order id is required.", nameof(id));

            var list = items?.ToList() ?? throw new ArgumentNullException(nameof(items));
            if (list.Count == 0) throw new ArgumentException("An order must have at least one item.", nameof(items));

            Id = id;
            Status = OrderStatus.Created;
            CreatedAt = DateTimeOffset.UtcNow;

            _items.AddRange(list);
            RecalculateTotal();
        }

        private void RecalculateTotal() => TotalAmount = _items.Sum(i => i.Total());
    }
}
