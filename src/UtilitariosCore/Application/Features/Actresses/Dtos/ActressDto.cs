namespace UtilitariosCore.Application.Features.Actresses.Dtos;

public class ActressDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Image { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateActressDto
{
    public string Name { get; set; } = string.Empty;
    public string? Image { get; set; }
}

public class UpdateActressDto
{
    public string? Name { get; set; }
    public string? Image { get; set; }
}
