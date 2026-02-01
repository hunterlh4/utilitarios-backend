using System.Net.Http.Headers;

namespace UtilitariosCore.Infrastructure.Services.Hostaway;

public class HostawayHeaderHandler(IHostawayProvider hostawayProvider) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var auth = await hostawayProvider.GetAuthentication();

        if (!string.IsNullOrWhiteSpace(auth?.TokenType) && !string.IsNullOrWhiteSpace(auth?.AccessToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue(auth.TokenType, auth.AccessToken);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}