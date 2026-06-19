namespace curtains_be.DTOs.Requests;

public class ContactInfoRequest
{
    public string? CompanyName { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? ZaloUrl { get; set; }
    public string? WhatsappUrl { get; set; }
    public string? FacebookUrl { get; set; }
}
