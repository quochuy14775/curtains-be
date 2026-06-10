namespace curtains_be.DTOs.Responses;

public class ProductResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Material { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? Tag { get; set; }
    public int CategoryId { get; set; }
    public string? CategoryTitle { get; set; }
    public bool IsActived { get; set; }
}
