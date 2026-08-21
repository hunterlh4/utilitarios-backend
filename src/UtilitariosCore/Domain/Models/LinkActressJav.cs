namespace UtilitariosCore.Domain.Models;

public class LinkActressJav
{
    public int Id { get; set; }
    public int ActressJavId { get; set; }
    public required string Url { get; set; }
    public int? OrderIndex { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // Navigation property
    public ActressJav? ActressJav { get; set; }
}