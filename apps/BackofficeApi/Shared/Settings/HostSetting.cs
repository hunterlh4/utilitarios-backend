namespace BackofficeApi.Shared.Settings;

public class HostSetting
{
    public required string WebHostUrl { get; set; }
    public required string AllowedHosts { get; set; }
    public required string PolicyName { get; set; }
}
