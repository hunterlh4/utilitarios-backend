namespace UtilitariosCore.Application.Features.Hentais.Requests;

public record UpdateHentaiTagsRequest(List<int> TagIds, string Name = "");
