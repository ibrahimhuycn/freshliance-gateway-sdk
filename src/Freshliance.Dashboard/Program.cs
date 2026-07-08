using Freshliance.Dashboard.Components;
using Freshliance.Dashboard.Services;
using FreshlianceGateway.Sdk;
using MudBlazor.Services;
using Serilog;
using Serilog.Formatting.Compact;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.secrets.json", optional: true, reloadOnChange: true);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Async(a => a.File(
        new RenderedCompactJsonFormatter(),
        "logs/dashboard-.log",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30))
    .CreateLogger();

try
{
    builder.Host.UseSerilog();

    builder.Services.Configure<PollingOptions>(
        builder.Configuration.GetSection("Polling"));

    builder.Services.AddFreshlianceGateway(options =>
    {
        options.AppId = builder.Configuration["Freshliance:AppId"]!;
        options.PrivateKeyPem = ResolveKey(builder.Configuration["Freshliance:PrivateKey"], "private key")!;
        options.PublicKeyPem = ResolveKey(builder.Configuration["Freshliance:PublicKey"], "public key");
        options.AcceptLanguage = builder.Configuration["Freshliance:AcceptLanguage"];
    });

    builder.Services.AddMudServices();
    builder.Services.AddRazorComponents()
        .AddInteractiveServerComponents();
    builder.Services.AddSingleton<PollingService>();
    builder.Services.AddHostedService(sp => sp.GetRequiredService<PollingService>());
    builder.Services.AddScoped<UserPreferenceService>();
    builder.Services.AddScoped<DeviceStateService>();
    builder.Services.AddScoped<NotificationService>();

    var app = builder.Build();

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
    }

    app.UseStaticFiles();
    app.UseSerilogRequestLogging();
    app.UseAntiforgery();

    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();

    app.Lifetime.ApplicationStarted.Register(() =>
    {
        foreach (var url in app.Urls)
            Log.Information("Now listening on: {Url}", url);
    });

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    await Log.CloseAndFlushAsync();
}

static string? ResolveKey(string? value, string keyKind)
{
    if (string.IsNullOrWhiteSpace(value))
        return null;

    var trimmed = value.Trim();
    if (trimmed.StartsWith('-') || trimmed.StartsWith("MII"))
        return trimmed;

    var path = trimmed;
    if (!Path.IsPathRooted(path))
        path = Path.Combine(Directory.GetCurrentDirectory(), path);

    if (!File.Exists(path))
    {
        Log.Warning("Configured {KeyKind} file not found at {Path} — using value as inline key", keyKind, path);
        return trimmed;
    }

    Log.Information("Loading {KeyKind} from {Path}", keyKind, path);
    return File.ReadAllText(path);
}
