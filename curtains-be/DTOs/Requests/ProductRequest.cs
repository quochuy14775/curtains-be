namespace curtains_be.DTOs.Requests;

public class ProductRequest
{
    public string Name { get; set; } = string.Empty;
    public string Material { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? Tag { get; set; }
    public int CategoryId { get; set; }
}
