namespace UtilitariosCore.Application.Features.Proyects.Dtos;

public class ProyectMediaDto
{
    public int Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public string? Thumbnail { get; set; }
    public int OrderIndex { get; set; }
}

public class ProyectLinkDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string Url { get; set; } = string.Empty;
    public int? OrderIndex { get; set; }
}

// Lista: portada (primera imagen) + tags de tecnologías
public class ProyectDto
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
public class ProyectDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Url { get; set; }
    public List<ProyectMediaDto> Media { get; set; } = [];
    public List<ProyectLinkDto> Links { get; set; } = [];
    public List<string> Tags { get; set; } = [];
    public DateTime CreatedAt { get; set; }
}

