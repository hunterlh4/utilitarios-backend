using BackofficeCore.Shared.Settings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace BackofficeCore.Infrastructure.Services.Hostaway;

public interface IHostawayProvider
{
    Task<HostawayAuthResponse?> GetAuthentication();
}

public class HostawayProvider(IHostawayAuthApi authApi, IMemoryCache cache, IOptions<HostawaySetting> options) : IHostawayProvider
{
    private const string CacheKey = "HostawayAuthentication";

    public async Task<HostawayAuthResponse?> GetAuthentication()
    {
        if (cache.TryGetValue(CacheKey, out HostawayAuthResponse? auth))
        {
            return auth;
        }

        var setting = options.Value;

        var payload = new HostawayAuthRequest
        {
            GrantType = "client_credentials",
            ClientId = setting.ClientId,
            ClientSecret = setting.ClientSecret,
            Scope = "general"
        };

        var authResponse = await authApi.AccessTokens(payload);

        if (string.IsNullOrWhiteSpace(authResponse.AccessToken)) throw new InvalidOperationException("Failed to retrieve access token.");

        var cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(authResponse.ExpiresIn - 60)
        };

        cache.Set(CacheKey, authResponse, cacheOptions);

        return authResponse;
    }
}