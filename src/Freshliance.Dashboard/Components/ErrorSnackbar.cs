using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;
using MudBlazor;

namespace Freshliance.Dashboard.Components;

public class ErrorSnackbar : ErrorBoundary
{
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private ILogger<ErrorSnackbar> Logger { get; set; } = null!;

    protected override Task OnErrorAsync(Exception exception)
    {
        Logger.LogError(exception, "Unhandled component exception");
        Snackbar.Add(exception.Message, Severity.Error, cfg => cfg.VisibleStateDuration = 10_000);
        return Task.CompletedTask;
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.AddContent(0, ChildContent);
    }
}
