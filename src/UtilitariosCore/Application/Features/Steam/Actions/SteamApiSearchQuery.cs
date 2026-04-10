using MediatR;
using System.Text.Json;
using UtilitariosCore.Shared.Responses;

namespace UtilitariosCore.Application.Features.SteamItems.Actions;

public record SteamApiSearchQuery(string Query, int Game = 1) : IRequest<Result<JsonElement>>;

internal sealed class SteamApiSearchQueryHandler(IHttpClientFactory httpClientFactory)
    : IRequestHandler<SteamApiSearchQuery, Result<JsonElement>>
{
    private const int DotaAppId = 570;
    private const int CS2AppId = 730;

    public async Task<Result<JsonElement>> Handle(SteamApiSearchQuery request, CancellationToken cancellationToken)
    {
        var appId = request.Game == 2 ? CS2AppId : DotaAppId;
        var client = httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");

        var url = $"https://steamcommunity.com/market/search/render/?query={Uri.EscapeDataString(request.Query)}&appid={appId}&search_descriptions=0&sort_column=popular&sort_dir=desc&norender=1&count=30&currency=1";

        var response = await client.GetAsync(url, cancellationToken);
        if (!response.IsSuccessStatusCode)
            return Errors.BadRequest("Error al consultar Steam Market.");

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var json = JsonDocument.Parse(content);
        return json.RootElement;
    }
}
