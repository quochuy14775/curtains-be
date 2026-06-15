namespace curtains_be.Models;

public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Material { get; set; } = string.Empty;
    public decimal? Price { get; set; }
    public string? Tag { get; set; }
    public string? ColorHex { get; set; }
    public string? ColorGroup { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
}
