namespace UtilitariosCore.Application.Features.Actresses.Requests;

public record UpdateActressJavRequest
{
    public string Name { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = [];
}
