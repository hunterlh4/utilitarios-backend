namespace UtilitariosCore.Domain.Models;

public class LinkJav
{
    public int Id { get; set; }
    public int JavId { get; set; }
    public required string Url { get; set; }
    public int? OrderIndex { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // Navigation property
    public Jav? Jav { get; set; }
}