using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DSW_TP1.Datos;
using DSW_TP1.Dominio.Models;
using Microsoft.AspNetCore.Authorization;

namespace DSW_TP1.Presentacion.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                return BadRequest("Los parámetros de paginación deben ser mayores que 0.");

            var query = _context.Products.AsQueryable();

            var totalItems = await query.CountAsync();

            var products = await query
                .OrderBy(p => p.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new
            {
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Items = products
            };

            return Ok(result);
        }
        
        // Busqueda por ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound($"No se encontró un producto con el ID {id}");

            return Ok(product);
        }
    }
}
