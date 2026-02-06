namespace UtilitariosCore.Application.Features.Proyects.Dtos;

public class ProyectDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Url { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateProyectDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Url { get; set; }
}

public class UpdateProyectDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Url { get; set; }
}
