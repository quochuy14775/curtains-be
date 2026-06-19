namespace curtains_be.Models;

// Thông tin liên hệ hiển thị trên trang chủ. Chỉ có duy nhất 1 bản ghi (Id = 1).
public class ContactInfo
{
    public int Id { get; set; }
    public string? CompanyName { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? ZaloUrl { get; set; }
    public string? WhatsappUrl { get; set; }
    public string? FacebookUrl { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
}
