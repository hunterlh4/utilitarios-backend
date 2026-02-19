namespace UtilitariosCore.Application.Features.Actresses.Dtos;

public class ActressJavDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? FirstImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateActressJavDto
{
    public int Id { get; set; }
}

public class UpdateActressJavDto
{
    public string Name { get; set; } = string.Empty;
}
