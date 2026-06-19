using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace curtains_be.Controllers;

[ApiController]
[Route("api/upload")]
public class UploadController(IConfiguration config) : ControllerBase
{
    // Cấp chữ ký để FE upload trực tiếp lên Cloudinary (signed upload).
    // [Authorize] => chỉ admin đã đăng nhập mới xin được chữ ký => chống spam upload.
    [Authorize]
    [HttpGet("signature")]
    public IActionResult GetSignature()
    {
        var cloudName = config["Cloudinary:CloudName"];
        var apiKey = config["Cloudinary:ApiKey"];
        var apiSecret = config["Cloudinary:ApiSecret"];

        if (string.IsNullOrEmpty(cloudName) || string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiSecret))
            return StatusCode(500, new { message = "Cloudinary chưa được cấu hình." });

        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        const string folder = "products";

        // Cloudinary yêu cầu: ký các tham số (sắp xếp theo alphabet) nối với api_secret rồi SHA1.
        var toSign = $"folder={folder}&timestamp={timestamp}{apiSecret}";
        var signature = Convert.ToHexString(
            SHA1.HashData(Encoding.UTF8.GetBytes(toSign))).ToLowerInvariant();

        return Ok(new
        {
            cloudName,
            apiKey,
            timestamp,
            folder,
            signature
        });
    }
}
