using UtilitariosCore.Domain.Interfaces;
using UtilitariosCore.Infrastructure.Persistence;
using UtilitariosCore.Infrastructure.Persistence.Repositories;
using UtilitariosCore.Infrastructure.Services.Hostaway;
using UtilitariosCore.Shared.Extensions;
using UtilitariosCore.Shared.Requests;
using UtilitariosCore.Shared.Settings;
using UtilitariosCore.Shared.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System.Reflection;

namespace UtilitariosCore;

public static class DependencyInyection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddScoped<IAuthContext, AuthContextService>();
        services.AddScoped<IJwtUtil, JwtUtil>();

        services.AddHttpClient();

        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCache();

        services.AddSingleton<MssqlContext>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserDetailRepository, UserDetailRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IAnimeRepository, AnimeRepository>();
        services.AddScoped<IHentaiRepository, HentaiRepository>();
        services.AddScoped<IJavRepository, JavRepository>();
        services.AddScoped<IActressRepository, ActressRepository>();
        services.AddScoped<ILinkRepository, LinkRepository>();
        services.AddScoped<ISeriesRepository, SeriesRepository>();
        services.AddScoped<IMediaRepository, MediaRepository>();
        services.AddScoped<IGirlGaleryRepository, GirlGaleryRepository>();
        services.AddScoped<IAnimeGaleryRepository, AnimeGaleryRepository>();

        // Servicios de notificaciones
        services.AddSingleton<Infrastructure.Queue.AzureQueueClient>();
        services.AddSingleton<INotificationService, Infrastructure.Services.Notifications.NotificationService>();

        // Servicio de subida de imágenes
        services.AddHttpClient<Infrastructure.Services.ImageUpload.IImgBBService, Infrastructure.Services.ImageUpload.ImgBBService>();

        // Configuraciones
        services.Configure<Infrastructure.Queue.AzureQueueSettings>(configuration.GetSection("Settings:AzureQueue"));
        services.Configure<Infrastructure.Queue.AzureQueueCredentials>(configuration.GetSection("Credentials:AzureQueue"));
        services.Configure<Infrastructure.Settings.SystemSettings>(configuration.GetSection("Settings:System"));
        services.Configure<Infrastructure.Settings.NotificationSettings>(configuration.GetSection("Settings:Notifications"));

        services.AddRefit(configuration);
        services.AddTimeZone(configuration);

        return services;
    }

    private static IServiceCollection AddRefit(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection("Providers:Hostaway");
        var setting = section.Get<HostawaySetting>() ?? throw new Exception("Failed to get HostawaySetting");
        var baseUrl = new Uri(setting.BaseUrl);

        services.Configure<HostawaySetting>(section);

        services.AddTransient<HostawayHeaderHandler>();
        services.AddTransient<IHostawayProvider, HostawayProvider>();

        services.AddRefitClient<IHostawayAuthApi>().ConfigureHttpClient(c => c.BaseAddress = baseUrl);
        services.AddRefitClient<IHostawayApi>().ConfigureHttpClient(c => c.BaseAddress = baseUrl).AddHttpMessageHandler<HostawayHeaderHandler>();

        return services;
    }

    public static IServiceCollection AddTimeZone(this IServiceCollection services, IConfiguration configuration)
    {
        var timeZoneId = configuration.GetValue<string>("Host:TimeZoneId") ?? throw new Exception("Failed to get TimeZoneSetting");

        DateTimeExtensions.Initialize(timeZoneId);

        return services;
    }

    public static IServiceCollection AddCache(this IServiceCollection services)
    {
        services.AddMemoryCache();
        return services;
    }
}
