using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DSW_TP1.Presentacion.DTO;
using DSW_TP1.Dominio.Models;
using DSW_TP1.Datos;
using Microsoft.AspNetCore.Authorization;

namespace DSW_TP1.Presentacion.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/orders
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
        {
            if (dto.OrderItems == null || !dto.OrderItems.Any())
                return BadRequest("Debe incluir al menos un producto en la orden.");

            // Verificamos stock
            var productIds = dto.OrderItems.Select(i => i.ProductId).ToList();
            var productos = await _context.Products
                .Where(p => productIds.Contains(p.ProductId))
                .ToListAsync();

            foreach (var item in dto.OrderItems)
            {
                var producto = productos.FirstOrDefault(p => p.ProductId == item.ProductId);

                if (producto == null)
                    return BadRequest($"Producto con ID {item.ProductId} no encontrado.");

                if (producto.StockQuantity < item.Quantity)
                    return BadRequest($"Stock insuficiente para el producto {producto.Name} (ID: {producto.ProductId}).");
            }

            // Crear entidad Order
            var order = new Order
            {
                OrderId = Guid.NewGuid(),
                CustomerId = dto.CustomerId,
                OrderDate = DateTime.UtcNow,
                OrderStatus = "Pending",
                ShippingAddress = dto.ShippingAddress,
                BillingAddress = dto.BillingAddress,
                Notes = dto.Notes,
                TotalAmount = 0m, 
                OrderItems = new List<OrderItem>()
            };

            decimal total = 0;

            foreach (var item in dto.OrderItems)
            {
                var producto = productos.First(p => p.ProductId == item.ProductId);

                var subtotal = item.UnitPrice * item.Quantity;

                order.OrderItems.Add(new OrderItem
                {
                    OrderItemId = Guid.NewGuid(),
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Subtotal = subtotal
                });

                producto.StockQuantity -= item.Quantity;
                total += subtotal;
            }

            order.TotalAmount = total;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrderById), new { id = order.OrderId }, order);
        }

        // GET: api/orders/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders(
        [FromQuery] string? status,
        [FromQuery] Guid? customerId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                return BadRequest("Los parámetros de paginación deben ser mayores que 0.");

            var query = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(i => i.Product)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status))
                query = query.Where(o => o.OrderStatus == status);

            if (customerId.HasValue)
                query = query.Where(o => o.CustomerId == customerId.Value);

            var totalItems = await query.CountAsync();

            var orders = await query
                .OrderByDescending(o => o.OrderDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new
            {
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Items = orders
            };

            return Ok(result);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(Guid id, [FromBody] UpdateOrderStatusDTO dto)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
                return NotFound($"No se encontró una orden con el ID {id}");

            var validStatuses = new[] { "Pending", "Processing", "Shipped", "Delivered", "Cancelled" };
            if (!validStatuses.Contains(dto.NewStatus))
                return BadRequest("Estado inválido. Estados válidos: Pending, Processing, Shipped, Delivered, Cancelled.");

            order.OrderStatus = dto.NewStatus;
            await _context.SaveChangesAsync();

            return Ok(order);
        }

    }
}
