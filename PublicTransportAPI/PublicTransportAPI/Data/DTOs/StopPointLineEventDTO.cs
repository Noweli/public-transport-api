namespace PublicTransportAPI.Data.DTOs;

public class StopPointLineEventDTO
{
    public string? Arrival { get; set; }
    public string? Departure { get; set; }
    public virtual int? LineId { get; set; }
    public virtual int? StopPointId { get; set; }
}