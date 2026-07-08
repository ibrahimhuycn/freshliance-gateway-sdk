using FreshlianceGateway.Sdk.Models.User;
using FreshlianceGateway.Sdk.Services;

namespace Freshliance.Dashboard.Services;

public sealed class UserPreferenceService
{
    private static readonly TimeZoneInfo FallbackTimeZone = TimeZoneInfo.Utc;

    public UserInfoResponse? UserInfo { get; private set; }
    public bool IsLoaded { get; private set; }

    public TimeZoneInfo TimeZone
    {
        get
        {
            if (UserInfo?.TimeZone is { Length: > 0 } tz)
            {
                try { return TimeZoneInfo.FindSystemTimeZoneById(tz); }
                catch { }
            }
            return FallbackTimeZone;
        }
    }

    public async Task LoadAsync(IUserService userService)
    {
        var response = await userService.GetAsync();
        response.EnsureSuccess();
        UserInfo = response.Data;
        IsLoaded = true;
    }

    public string FormatDateTime(long epochMs)
    {
        var dt = DateTimeOffset.FromUnixTimeMilliseconds(epochMs);
        var local = TimeZoneInfo.ConvertTime(dt, TimeZone);
        var format = UserInfo?.DateFormat switch
        {
            2 => "dd/MM/yyyy HH:mm",
            3 => "MM/dd/yyyy HH:mm",
            _ => "yyyy/MM/dd HH:mm"
        };
        return local.ToString(format);
    }

    public string FormatTemperature(double? celsius)
    {
        if (celsius is null)
            return "—";

        var unit = UserInfo?.TemperatureUnit;
        if (unit == 2)
        {
            var f = celsius.Value * 9.0 / 5.0 + 32.0;
            return $"{f:F1}°F";
        }
        return $"{celsius.Value:F1}°C";
    }
}
