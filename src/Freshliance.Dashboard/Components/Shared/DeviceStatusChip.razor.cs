using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Freshliance.Dashboard.Components.Shared;

public partial class DeviceStatusChip
{
    [Parameter]
    public int Status { get; set; }

    private Color _color = Color.Default;
    private string _label = "Unknown";

    protected override void OnParametersSet()
    {
        (_color, _label) = Status switch
        {
            1 => (Color.Success, "Online"),
            2 => (Color.Default, "Offline"),
            3 => (Color.Warning, "Abnormal"),
            _ => (Color.Surface, "Inactive")
        };
    }
}
