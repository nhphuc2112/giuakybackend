using Microsoft.AspNetCore.Mvc;
using Backend.Data;
using Backend.Models;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly DatabaseContext _db;

    public ProductsController(DatabaseContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        var products = await _db.QueryAsync<Product>("sp_GetAllProducts");
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _db.QuerySingleOrDefaultAsync<Product>("sp_GetProductById", new { Id = id });
        if (product == null)
            return NotFound();
        return Ok(product);
    }

    [HttpGet("category/{categoryId}")]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory(int categoryId)
    {
        var products = await _db.QueryAsync<Product>("sp_GetProductsByCategory", new { CategoryId = categoryId });
        return Ok(products);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        product.CreatedAt = DateTime.UtcNow;
        var parameters = new
        {
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            CategoryId = product.CategoryId,
            CreatedAt = product.CreatedAt,
            UpdatedAt = (DateTime?)null
        };
        var id = await _db.ExecuteAsync("sp_CreateProduct", parameters);
        product.Id = id;
        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, Product product)
    {
        if (id != product.Id)
            return BadRequest();

        product.UpdatedAt = DateTime.UtcNow;
        var parameters = new
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            CategoryId = product.CategoryId,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt
        };
        var result = await _db.ExecuteAsync("sp_UpdateProduct", parameters);
        if (result == 0)
            return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var result = await _db.ExecuteAsync("sp_DeleteProduct", new { Id = id });
        if (result == 0)
            return NotFound();
        return NoContent();
    }
} 