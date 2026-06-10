using curtains_be.Common;
using curtains_be.DTOs.Requests;
using curtains_be.DTOs.Responses;
using curtains_be.Models;
using curtains_be.Repository;
using curtains_be.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace curtains_be.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController(IRepository<Category> categoryRepository)
    : BaseController<Category>(categoryRepository)
{
    [HttpGet]
    public async Task<IActionResult> GetAll(ODataQueryOptions<Category> queryOptions)
        => Ok(await CategoryService.GetAllAsync(_baseRepository, queryOptions));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await CategoryService.GetByIdAsync(_baseRepository, id);
        if (category is null)
            return NotFound(ResponseMessage.Fail("Category not found"));

        return Ok(category);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CategoryRequest req)
    {
        var id = await CategoryService.CreateAsync(_baseRepository, req, User.Identity?.Name);
        return Ok(id);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CategoryRequest req)
    {
        var updated = await CategoryService.UpdateAsync(_baseRepository, id, req, User.Identity?.Name);
        if (!updated)
            return NotFound(ResponseMessage.Fail("Category not found"));

        return Ok(ResponseMessage.Ok("Updated"));
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await CategoryService.DeleteAsync(_baseRepository, id, User.Identity?.Name);
        if (!deleted)
            return NotFound(ResponseMessage.Fail("Category not found"));

        return Ok(ResponseMessage.Ok("Deleted"));
    }
}
