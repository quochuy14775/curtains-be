using curtains_be.Common;
using curtains_be.DTOs.Requests;
using curtains_be.Models;
using curtains_be.Repository;
using curtains_be.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace curtains_be.Controllers;

[ApiController]
[Route("api/contact")]
public class ContactInfoController(IRepository<ContactInfo> repo) : ControllerBase
{
    // Công khai — trang chủ đọc thông tin liên hệ.
    [HttpGet]
    public async Task<IActionResult> Get()
        => Ok(await ContactInfoService.GetAsync(repo));

    // Chỉ admin được cập nhật.
    [Authorize]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ContactInfoRequest req)
    {
        await ContactInfoService.UpdateAsync(repo, req, User.Identity?.Name);
        return Ok(ResponseMessage.Ok("Updated"));
    }
}
