using curtains_be.Common;
using curtains_be.DTOs.Requests;
using curtains_be.Models;
using curtains_be.Repository;
using curtains_be.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.RateLimiting;

namespace curtains_be.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentController(
    IRepository<Appointment> appointmentRepository,
    IRepository<Customer> customerRepository)
    : BaseController<Appointment>(appointmentRepository)
{
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll(ODataQueryOptions<Appointment> queryOptions)
        => Ok(await AppointmentService.GetAllAsync(_baseRepository, queryOptions));

    [Authorize]
    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
        => Ok(await AppointmentService.GetStatsAsync(_baseRepository, customerRepository));

    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var appointment = await AppointmentService.GetByIdAsync(_baseRepository, id);
        if (appointment is null)
            return NotFound(ResponseMessage.Fail("Appointment not found"));

        return Ok(appointment);
    }

    // Public — form đặt lịch tư vấn trên trang chủ
    [HttpPost]
    [EnableRateLimiting("PublicForm")]
    public async Task<IActionResult> Create([FromBody] AppointmentRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Name) || string.IsNullOrWhiteSpace(req.Phone))
            return BadRequest(ResponseMessage.Fail("Name and phone are required"));

        var phoneDigits = req.Phone.Count(char.IsDigit);
        if (phoneDigits < 9 || phoneDigits > 11)
            return BadRequest(ResponseMessage.Fail("Invalid phone number"));

        var id = await AppointmentService.CreateAsync(_baseRepository, customerRepository, req, User.Identity?.Name);
        return Ok(id);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] AppointmentUpdateRequest req)
    {
        var updated = await AppointmentService.UpdateAsync(_baseRepository, id, req, User.Identity?.Name);
        if (!updated)
            return NotFound(ResponseMessage.Fail("Appointment not found"));

        return Ok(ResponseMessage.Ok("Updated"));
    }

    [Authorize]
    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] AppointmentStatusRequest req)
    {
        var updated = await AppointmentService.UpdateStatusAsync(_baseRepository, id, req.Status, User.Identity?.Name);
        if (!updated)
            return NotFound(ResponseMessage.Fail("Appointment not found"));

        return Ok(ResponseMessage.Ok("Status updated"));
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await AppointmentService.DeleteAsync(_baseRepository, id, User.Identity?.Name);
        if (!deleted)
            return NotFound(ResponseMessage.Fail("Appointment not found"));

        return Ok(ResponseMessage.Ok("Deleted"));
    }
}
