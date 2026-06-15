using curtains_be.Enum;

namespace curtains_be.DTOs.Requests;

public class AppointmentUpdateRequest
{
    public AppointmentStatus Status { get; set; }
    public string? StaffNote { get; set; }
    public DateTime? ScheduledAt { get; set; }
}
