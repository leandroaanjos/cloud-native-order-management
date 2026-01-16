using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Abstractions;
using OrderManagement.Application.Orders.CreateOrder;
using OrderManagement.Application.Orders.GetOrder;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Api.Controllers
{
    [ApiController]
    [Route("api/v1/orders")]
    public sealed class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _repo;

        public OrdersController(IOrderRepository repo) => _repo = repo;

        [HttpPost]
        public async Task<ActionResult<CreateOrderResponse>> Create([FromBody] CreateOrderRequest request, CancellationToken ct)
        {
            if (request.Items is null || request.Items.Count == 0)
                return BadRequest("An order must have at least one item.");

            var items = request.Items.Select(i => new OrderItem(i.ProductId, i.Quantity, i.UnitPrice)).ToList();
            var order = new Order(Guid.NewGuid(), items);

            await _repo.AddAsync(order, ct);

            return CreatedAtAction(nameof(GetById), new { id = order.Id }, new CreateOrderResponse(order.Id));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<GetOrderResponse>> GetById([FromRoute] Guid id, CancellationToken ct)
        {
            var order = await _repo.GetByIdAsync(id, ct);
            if (order is null) return NotFound();

            var response = new GetOrderResponse(
                order.Id,
                order.Status.ToString(),
                order.TotalAmount,
                order.CreatedAt,
                order.Items.Select(i => new GetOrderItemResponse(i.ProductId, i.Quantity, i.UnitPrice)).ToList()
            );

            return Ok(response);
        }
    }
}