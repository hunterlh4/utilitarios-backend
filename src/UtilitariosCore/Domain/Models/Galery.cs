namespace UtilitariosCore.Domain.Models;

public class AnimeGalery
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Image { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class GirlGalery
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Image { get; set; }
    public DateTime CreatedAt { get; set; }
}
