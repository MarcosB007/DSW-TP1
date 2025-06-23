namespace DSW_TP1.DTO
{
    public class CreateOrderDto
    {
        public Guid CustomerId { get; set; }

        public string ShippingAddress { get; set; } = string.Empty;

        public string BillingAddress { get; set; } = string.Empty;

        public string? Notes { get; set; }

        public List<CreateOrderItemDto> OrderItems { get; set; } = new();
    }

    public class CreateOrderItemDto
    {
        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }
    }
}
