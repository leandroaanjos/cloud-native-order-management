namespace OrderManagement.Application.Orders.CreateOrder
{
    public sealed record CreateOrderRequest(IReadOnlyList<CreateOrderItemRequest> Items);

    public sealed record CreateOrderItemRequest(Guid ProductId, int Quantity, decimal UnitPrice);
}
