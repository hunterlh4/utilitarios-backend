namespace UtilitariosCore.Application.Features.DotaHeroes.Dtos;

public class DotaHeroDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Image { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateDotaHeroDto
{
    public string Name { get; set; } = string.Empty;
    public string? Image { get; set; }
}

public class UpdateDotaHeroDto
{
    public string? Name { get; set; }
    public string? Image { get; set; }
}
