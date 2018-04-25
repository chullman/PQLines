namespace PQLines.Models
{
    // Marker interface - to indicate to our services which models are the "root" level models
    public interface IRootModel
    {
        string Description { get; set; }
    }
}