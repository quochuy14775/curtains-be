namespace curtains_be.Models;

public class Customer : BaseEntity
{
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }

    public List<Appointment>? Appointments { get; set; }
}
