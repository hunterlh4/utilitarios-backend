namespace UtilitariosCore.Domain.Models;

public class ActressAdult
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Image { get; set; }
    public DateTime CreatedAt { get; set; }
}
