namespace PublicTransportAPI.Data.Models;

public class Line
{
    public int Id { get; set; }
    public string? LineIdentifier { get; set; }
    public virtual ICollection<StopPoint>? StopPoints { get; set; }
}