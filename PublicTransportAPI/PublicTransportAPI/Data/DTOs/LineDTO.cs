namespace PublicTransportAPI.Data.DTOs;

public class LineDTO
{
    public string? LineIdentifier { get; set; }
    public virtual ICollection<int>? StopPointIds { get; set; }
}