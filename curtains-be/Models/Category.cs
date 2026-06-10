namespace curtains_be.Models;

public class Category : BaseEntity
{
    public string? Title { get; set; }
    public string? Subtitle { get; set; }

    public List<Product>? Products { get; set; }
}
