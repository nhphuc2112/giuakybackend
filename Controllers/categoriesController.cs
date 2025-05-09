using Microsoft.AspNetCore.Mvc;
using Backend.Data;
using Backend.Models;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly DatabaseContext _db;

    public CategoriesController(DatabaseContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
    {
        var categories = await _db.QueryAsync<Category>("sp_GetAllCategories");
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetCategory(int id)
    {
        var category = await _db.QuerySingleOrDefaultAsync<Category>("sp_GetCategoryById", new { Id = id });
        if (category == null)
            return NotFound();
        return Ok(category);
    }

    [HttpPost]
    public async Task<ActionResult<Category>> CreateCategory(Category category)
    {
        category.CreatedAt = DateTime.UtcNow;
        var parameters = new
        {
            Name = category.Name,
            Description = category.Description,
            CreatedAt = category.CreatedAt,
            UpdatedAt = (DateTime?)null
        };
        var id = await _db.ExecuteAsync("sp_CreateCategory", parameters);
        category.Id = id;
        return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(int id, Category category)
    {
        if (id != category.Id)
            return BadRequest();

        category.UpdatedAt = DateTime.UtcNow;
        var parameters = new
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt
        };
        var result = await _db.ExecuteAsync("sp_UpdateCategory", parameters);
        if (result == 0)
            return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var result = await _db.ExecuteAsync("sp_DeleteCategory", new { Id = id });
        if (result == 0)
            return NotFound();
        return NoContent();
    }
} 