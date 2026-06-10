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
public class ProductController(IRepository<Product> productRepository)
    : BaseController<Product>(productRepository)
{
    [HttpGet]
    public async Task<IActionResult> GetAll(ODataQueryOptions<Product> queryOptions)
        => Ok(await ProductService.GetAllAsync(_baseRepository, queryOptions));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await ProductService.GetByIdAsync(_baseRepository, id);
        if (product is null)
            return NotFound(ResponseMessage.Fail("Product not found"));

        return Ok(product);
    }

    [Authorize]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllAdmin(ODataQueryOptions<Product> queryOptions)
        => Ok(await ProductService.GetAllAdminAsync(_baseRepository, queryOptions));

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductRequest req)
    {
        var id = await ProductService.CreateAsync(_baseRepository, req, User.Identity?.Name);
        return Ok(id);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ProductRequest req)
    {
        var updated = await ProductService.UpdateAsync(_baseRepository, id, req, User.Identity?.Name);
        if (!updated)
            return NotFound(ResponseMessage.Fail("Product not found"));

        return Ok(ResponseMessage.Ok("Updated"));
    }

    [Authorize]
    [HttpPatch("{id}/toggle-active")]
    public async Task<IActionResult> ToggleActive(int id)
    {
        var result = await ProductService.ToggleActiveAsync(_baseRepository, id, User.Identity?.Name);
        if (result is null)
            return NotFound(ResponseMessage.Fail("Product not found"));

        return Ok(ResponseMessage.Ok("Actived"));
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await ProductService.DeleteAsync(_baseRepository, id, User.Identity?.Name);
        if (!deleted)
            return NotFound(ResponseMessage.Fail("Product not found"));

        return Ok(ResponseMessage.Ok("Deleted"));
    }
}
