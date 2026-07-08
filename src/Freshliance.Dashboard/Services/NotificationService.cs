using MudBlazor;

namespace Freshliance.Dashboard.Services;

public sealed class NotificationService
{
    private readonly ISnackbar _snackbar;

    public NotificationService(ISnackbar snackbar)
    {
        _snackbar = snackbar;
    }

    public void Error(string message)
        => _snackbar.Add(message, Severity.Error, cfg => cfg.VisibleStateDuration = 8000);

    public void Warning(string message)
        => _snackbar.Add(message, Severity.Warning, cfg => cfg.VisibleStateDuration = 5000);

    public void Info(string message)
        => _snackbar.Add(message, Severity.Info, cfg => cfg.VisibleStateDuration = 3000);

    public void Success(string message)
        => _snackbar.Add(message, Severity.Success, cfg => cfg.VisibleStateDuration = 3000);
}
