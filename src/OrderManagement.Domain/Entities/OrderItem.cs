namespace OrderManagement.Domain.Entities
{
    public sealed class OrderItem
    {
        public Guid ProductId { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }

        private OrderItem() { } // For EF Core

        public OrderItem(Guid productId, int quantity, decimal unitPrice)
        {
            if (productId == Guid.Empty) throw new ArgumentException("ProductId is required.", nameof(productId));
            if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");
            if (unitPrice <= 0) throw new ArgumentOutOfRangeException(nameof(unitPrice), "UnitPrice must be greater than zero.");

            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        public decimal Total() => Quantity * UnitPrice;
    }
}
