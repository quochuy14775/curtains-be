using curtains_be.Enum;

namespace curtains_be.Models;



public class Appointment : BaseEntity
{
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    public string? Note { get; set; }
    public string? StaffNote { get; set; }
    public AppointmentStatus Status { get; set; }
    public DateTime? ScheduledAt { get; set; }
}
