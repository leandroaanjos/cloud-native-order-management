namespace OrderManagement.Application.Orders.GetOrder
{
    public sealed record GetOrderResponse(
        Guid Id,
        string Status,
        decimal TotalAmount,
        DateTimeOffset CreatedAt,
        IReadOnlyList<GetOrderItemResponse> Items);

    public sealed record GetOrderItemResponse(Guid ProductId, int Quantity, decimal UnitPrice);
}
