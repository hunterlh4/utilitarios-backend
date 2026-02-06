namespace UtilitariosCore.Domain.Models;

public class Proyect
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Url { get; set; }
    public DateTime CreatedAt { get; set; }
}
