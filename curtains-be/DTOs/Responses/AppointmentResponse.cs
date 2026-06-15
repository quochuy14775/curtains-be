using curtains_be.Enum;

namespace curtains_be.DTOs.Responses;

public class AppointmentResponse
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? Note { get; set; }
    public string? StaffNote { get; set; }
    public AppointmentStatus Status { get; set; }
    public DateTime? ScheduledAt { get; set; }
    public DateTime CreatedAt { get; set; }
}
