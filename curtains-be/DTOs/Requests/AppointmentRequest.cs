namespace curtains_be.DTOs.Requests;

public class AppointmentRequest
{
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? Note { get; set; }
}
