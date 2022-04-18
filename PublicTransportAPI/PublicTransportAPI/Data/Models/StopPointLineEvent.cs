namespace PublicTransportAPI.Data.Models;

public class StopPointLineEvent
{
    public int Id { get; set; }
    public DateTime? Arrival { get; set; }
    public DateTime? Departure { get; set; }
    public virtual Line? Line { get; set; }
    public virtual StopPoint? StopPoint { get; set; }
}