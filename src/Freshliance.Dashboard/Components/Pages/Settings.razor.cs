using Freshliance.Dashboard.Services;
using FreshlianceGateway.Sdk.Models.User;
using FreshlianceGateway.Sdk.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Freshliance.Dashboard.Components.Pages;

public partial class Settings
{
    [Inject] private IUserService UserService { get; set; } = null!;
    [Inject] private UserPreferenceService UserPreferences { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    private UserInfoResponse? _userInfo;
    private bool _isLoading = true;
    private bool _isSaving;
    private string? _error;

    private string _email = "";
    private string _nickname = "";
    private string _timezone = "";
    private int _language;
    private int _dateFormat;
    private int _temperatureUnit;

    private string _originalEmail = "";
    private string _originalNickname = "";
    private string _originalTimezone = "";
    private int _originalLanguage;
    private int _originalDateFormat;
    private int _originalTemperatureUnit;

    private List<string> _timezones = [];
    private Dictionary<string, string> _timezoneDisplayMap = [];

    protected override async Task OnInitializedAsync()
    {
        try
        {
            if (!UserPreferences.IsLoaded)
            {
                await UserPreferences.LoadAsync(UserService);
            }

            _userInfo = UserPreferences.UserInfo;
            if (_userInfo is not null)
            {
                _email = _originalEmail = _userInfo.Email;
                _nickname = _originalNickname = _userInfo.Nickname ?? "";
                _timezone = _originalTimezone = _userInfo.TimeZone;
                _language = _originalLanguage = _userInfo.Language;
                _dateFormat = _originalDateFormat = _userInfo.DateFormat;
                _temperatureUnit = _originalTemperatureUnit = _userInfo.TemperatureUnit;

                _timezones = TimeZoneInfo.GetSystemTimeZones()
                    .Select(tz => tz.Id)
                    .OrderBy(id => id)
                    .ToList();

                _timezoneDisplayMap = TimeZoneInfo.GetSystemTimeZones()
                    .ToDictionary(tz => tz.Id, tz => tz.DisplayName);

                if (!_timezones.Contains(_timezone) && !string.IsNullOrEmpty(_timezone))
                {
                    _timezones.Add(_timezone);
                    _timezoneDisplayMap[_timezone] = _timezone;
                    _timezones = _timezones.OrderBy(id => id).ToList();
                }
            }
        }
        catch (Exception ex)
        {
            _error = $"Failed to load user settings: {ex.Message}";
        }
        finally
        {
            _isLoading = false;
        }
    }

    private async Task SaveAsync()
    {
        if (_isSaving)
            return;

        _isSaving = true;

        try
        {
            var request = new UpdateUserInfoRequest();

            if (_nickname != _originalNickname)
                request.Nickname = _nickname;

            if (_timezone != _originalTimezone)
                request.TimeZone = _timezone;

            if (_language != _originalLanguage)
                request.Language = _language;

            if (_dateFormat != _originalDateFormat)
                request.DateFormat = _dateFormat;

            if (_temperatureUnit != _originalTemperatureUnit)
                request.TemperatureUnit = _temperatureUnit;

            var response = await UserService.UpdateAsync(request);
            response.EnsureSuccess();

            _originalNickname = _nickname;
            _originalTimezone = _timezone;
            _originalLanguage = _language;
            _originalDateFormat = _dateFormat;
            _originalTemperatureUnit = _temperatureUnit;

            Snackbar.Add("Settings saved successfully.", Severity.Success, config =>
            {
                config.VisibleStateDuration = 4000;
            });

            if (_userInfo is not null)
            {
                _userInfo.Nickname = _nickname;
                _userInfo.TimeZone = _timezone;
                _userInfo.Language = _language;
                _userInfo.DateFormat = _dateFormat;
                _userInfo.TemperatureUnit = _temperatureUnit;
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to save settings: {ex.Message}", Severity.Error, config =>
            {
                config.VisibleStateDuration = 6000;
            });
        }
        finally
        {
            _isSaving = false;
        }
    }

    private async Task<IEnumerable<string>> SearchTimezones(string searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
            return _timezones;

        return await Task.FromResult(_timezones
            .Where(tz =>
                tz.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                (_timezoneDisplayMap.TryGetValue(tz, out var display) &&
                 display.Contains(searchText, StringComparison.OrdinalIgnoreCase)))
            .Take(50));
    }
}

