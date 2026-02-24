namespace UtilitariosCore.Application.Features.ActressAdults.Requests;

public record LinkInput
{
    public string? Name { get; set; }
    public string Url { get; set; } = string.Empty;
    public int? OrderIndex { get; set; }
}
