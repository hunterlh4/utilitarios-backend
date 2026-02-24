namespace UtilitariosCore.Application.Features.ActressAdults.Requests;

public record CreateActressAdultRequest
{
    public string Name { get; set; } = string.Empty;
}
