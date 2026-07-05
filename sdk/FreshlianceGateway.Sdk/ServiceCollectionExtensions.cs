using FreshlianceGateway.Sdk.Core;
using FreshlianceGateway.Sdk.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace FreshlianceGateway.Sdk;

/// <summary>
/// Extension methods for registering Freshliance SDK services in the dependency injection container.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the Freshliance gateway client, signature provider, and all service dependencies.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="configure">A delegate to configure <see cref="FreshlianceOptions"/>.</param>
    /// <returns>An <see cref="IHttpClientBuilder"/> for further HTTP client configuration.</returns>
    public static IHttpClientBuilder AddFreshlianceGateway(
        this IServiceCollection services,
        Action<FreshlianceOptions> configure)
    {
        services.Configure(configure);

        services.TryAddSingleton<ISignatureProvider>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<FreshlianceOptions>>().Value;
            return new Rsa2SignatureProvider(options.PrivateKeyPem, options.PublicKeyPem);
        });

        services.TryAddScoped<FreshlianceClient>();

        services.TryAddScoped<IUserService, UserService>();
        services.TryAddScoped<IDeviceService, DeviceService>();
        services.TryAddScoped<IDeviceDataService, DeviceDataService>();
        services.TryAddScoped<IGroupService, GroupService>();
        services.TryAddScoped<IGroupDeviceService, GroupDeviceService>();
        services.TryAddScoped<IRemoteCommandService, RemoteCommandService>();
        services.TryAddScoped<IConfigTemplateService, ConfigTemplateService>();

        return services.AddHttpClient<FreshlianceClient>((sp, client) =>
        {
            var options = sp.GetRequiredService<IOptions<FreshlianceOptions>>().Value;
            client.BaseAddress = new Uri(options.BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);

            if (options.AcceptLanguage is not null)
                client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", options.AcceptLanguage);
        });
    }
}
