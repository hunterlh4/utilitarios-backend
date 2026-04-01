🔐 1. Configuración inicial
Variables en backend (.NET)
{
  "Mal": {
    "ClientId": "TU_CLIENT_ID",
    "ClientSecret": "TU_CLIENT_SECRET",
    "RedirectUri": "http://localhost:3000/callback"
  }
}


🔑 2. Generar code_verifier

En .NET:

public string GenerateCodeVerifier()
{
    var bytes = new byte[32];
    using var rng = RandomNumberGenerator.Create();
    rng.GetBytes(bytes);
    return Convert.ToBase64String(bytes)
        .TrimEnd('=')
        .Replace('+', '-')
        .Replace('/', '_');
}


🌐 3. URL de autorización
public string GetAuthorizationUrl(string codeVerifier)
{
    return $"https://myanimelist.net/v1/oauth2/authorize" +
           $"?response_type=code" +
           $"&client_id={CLIENT_ID}" +
           $"&redirect_uri={REDIRECT_URI}" +
           $"&code_challenge={codeVerifier}" +
           $"&code_challenge_method=plain";
}



🚀 4. Autenticación (SOLO UNA VEZ)
Paso:
Abre la URL generada en el navegador
Inicia sesión en MAL
Te redirige a:
http://localhost:3000/callback?code=XXXX


🔄 5. Obtener tokens
public async Task<TokenResponse> GetToken(string code, string codeVerifier)
{
    var client = new HttpClient();

    var content = new FormUrlEncodedContent(new Dictionary<string, string>
    {
        { "client_id", CLIENT_ID },
        { "client_secret", CLIENT_SECRET },
        { "grant_type", "authorization_code" },
        { "code", code },
        { "redirect_uri", REDIRECT_URI },
        { "code_verifier", codeVerifier }
    });

    var response = await client.PostAsync(
        "https://myanimelist.net/v1/oauth2/token",
        content
    );

    var json = await response.Content.ReadAsStringAsync();
    return JsonSerializer.Deserialize<TokenResponse>(json);
}


💾 6. Guardar tokens en BD

Tabla:

MalTokens
- Id
- AccessToken
- RefreshToken
- ExpiresAt



🔄 7. Refresh token automático
public async Task<TokenResponse> RefreshToken(string refreshToken)
{
    var client = new HttpClient();

    var content = new FormUrlEncodedContent(new Dictionary<string, string>
    {
        { "client_id", CLIENT_ID },
        { "client_secret", CLIENT_SECRET },
        { "grant_type", "refresh_token" },
        { "refresh_token", refreshToken }
    });

    var response = await client.PostAsync(
        "https://myanimelist.net/v1/oauth2/token",
        content
    );

    var json = await response.Content.ReadAsStringAsync();
    return JsonSerializer.Deserialize<TokenResponse>(json);
}



🔍 8. Buscar animes (sin OAuth)
public async Task<string> SearchAnime(string query)
{
    var client = new HttpClient();

    client.DefaultRequestHeaders.Add("X-MAL-CLIENT-ID", CLIENT_ID);

    var response = await client.GetAsync(
        $"https://api.myanimelist.net/v2/anime?q={query}&limit=10"
    );

    return await response.Content.ReadAsStringAsync();
}



❤️ 9. Guardar anime en MAL
public async Task AddToMal(int animeId, string accessToken)
{
    var client = new HttpClient();

    client.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", accessToken);

    var content = new FormUrlEncodedContent(new Dictionary<string, string>
    {
        { "status", "completed" }
    });

    await client.PutAsync(
        $"https://api.myanimelist.net/v2/anime/{animeId}/my_list_status",
        content
    );
}


🔁 10. Lógica completa en backend
if (TokenExpired())
{
    var newToken = await RefreshToken(refreshToken);
    SaveToken(newToken);
}

await AddToMal(animeId, accessToken);