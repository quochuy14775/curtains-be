namespace curtains_be.DTOs.Requests;

public class CategoryRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Subtitle { get; set; }
}
