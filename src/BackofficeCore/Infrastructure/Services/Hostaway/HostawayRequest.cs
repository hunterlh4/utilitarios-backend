using Refit;

namespace BackofficeCore.Infrastructure.Services.Hostaway;

public class HostawayAuthRequest
{
    [AliasAs("grant_type")]
    public string? GrantType { get; set; }

    [AliasAs("client_id")]
    public int ClientId { get; set; }

    [AliasAs("client_secret")]
    public string? ClientSecret { get; set; }

    [AliasAs("scope")]
    public string? Scope { get; set; }
}

public class HostawayCalendarQuery
{
    [AliasAs("startDate")]
    public string? StartDate { get; set; }

    [AliasAs("endDate")]
    public string? EndDate { get; set; }

    [AliasAs("includeResources")]
    public int IncludeResources { get; set; }
}