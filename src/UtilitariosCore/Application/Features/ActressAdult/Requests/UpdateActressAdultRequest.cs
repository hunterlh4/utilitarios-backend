namespace UtilitariosCore.Application.Features.ActressAdults.Requests;

public record UpdateActressAdultRequest
{
    public string Name { get; set; } = string.Empty;
    public List<int> Tags { get; set; } = [];
}
