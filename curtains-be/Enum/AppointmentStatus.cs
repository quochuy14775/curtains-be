using System.Text.Json.Serialization;

namespace curtains_be.Enum;

// JSON trao đổi với FE dùng string ("Pending"...) thay vì số
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AppointmentStatus
{
    Pending = 0,
    Confirmed = 1,
    Completed = 2,
    Cancelled = 3
}