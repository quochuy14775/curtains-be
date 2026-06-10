namespace curtains_be.Models;

public interface IAuditable
{
    string? CreatedBy { get; set; }
    string? UpdatedBy { get; set; }
    DateTime CreatedAt { get; set; }
    DateTime? UpdatedAt { get; set; }
    bool IsDeleted { get; set; }
    bool IsActive { get; set; }
}