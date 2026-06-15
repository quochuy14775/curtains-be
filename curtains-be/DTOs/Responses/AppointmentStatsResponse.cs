namespace curtains_be.DTOs.Responses;

public class AppointmentStatsResponse
{
    public int Total { get; set; }
    public int Pending { get; set; }
    public int Confirmed { get; set; }
    public int Completed { get; set; }
    public int Cancelled { get; set; }
    public int CompletedThisMonth { get; set; }
    public int CompletionRate { get; set; }
    public int TotalCustomers { get; set; }
    public IEnumerable<AppointmentResponse> Upcoming { get; set; } = [];
    public IEnumerable<AppointmentResponse> Recent { get; set; } = [];
}
