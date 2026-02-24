using UtilitariosCore.Domain.Enums;

namespace UtilitariosCore.Application.Features.ActressAdults.Requests;

public record UpdateVideoAdultStatusRequest
{
    public ContentStatus Status { get; set; }
}
