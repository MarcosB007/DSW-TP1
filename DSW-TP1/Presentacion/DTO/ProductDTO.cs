using DSW_TP1.Dominio.Models;

namespace DSW_TP1.Presentacion.DTO
{
    public class ProductDTO
    {
        public int TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public List<Product> Items { get; set; } = new();
    }
}
