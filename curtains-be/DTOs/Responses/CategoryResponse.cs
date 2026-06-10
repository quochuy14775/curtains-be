namespace curtains_be.DTOs.Responses;

public class CategoryResponse
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Subtitle { get; set; }
    public int ProductCount { get; set; }
}
