namespace UtilitariosCore.Application.Features.Projects.Dtos;

public class ProjectMediaDto
{
    public int Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public string? Thumbnail { get; set; }
    public int OrderIndex { get; set; }
}

public class ProjectLinkDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string Url { get; set; } = string.Empty;
    public int? OrderIndex { get; set; }
}

// Lista: portada (primera imagen) + tags de tecnologías
public class ProjectDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Url { get; set; }
    public string? FirstImageUrl { get; set; }
    public List<string> Tags { get; set; } = [];
    public DateTime CreatedAt { get; set; }
}

// Detalle: todas las imágenes + links + tags
public class ProjectDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Url { get; set; }
    public List<ProjectMediaDto> Media { get; set; } = [];
    public List<ProjectLinkDto> Links { get; set; } = [];
    public List<string> Tags { get; set; } = [];
    public DateTime CreatedAt { get; set; }
}

