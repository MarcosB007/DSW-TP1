using System.ComponentModel.DataAnnotations;

namespace DSW_TP1.Dominio.Models
{
    public class Order
    {
        [Key]
        public Guid OrderId { get; set; }

        public Guid CustomerId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(50)]
        public string OrderStatus { get; set; } = "Pending";

        [Required]
        public decimal TotalAmount { get; set; }

        [Required]
        public string? ShippingAddress { get; set; }

        [Required]
        public string? BillingAddress { get; set; }

        public string? Notes { get; set; }
        
        //RELACION 1 A N
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
