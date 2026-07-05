using FreshlianceGateway.Sdk.Core;
using FreshlianceGateway.Sdk.Models.Command;

namespace FreshlianceGateway.Sdk.Services;

/// <summary>Provides operations for sending remote commands to devices through Freshliance Cloud.</summary>
public interface IRemoteCommandService
{
    /// <summary>Sends a parameter update command to a device.</summary>
    Task<FreshlianceResponse<bool>> UpdateParameterAsync(UpdateParameterRequest request, CancellationToken ct = default);
    /// <summary>Sends a save-data-then-shutdown command to a device.</summary>
    Task<FreshlianceResponse<bool>> SaveDataShutdownAsync(ShutdownRequest request, CancellationToken ct = default);
    /// <summary>Sends a direct (immediate) shutdown command to a device.</summary>
    Task<FreshlianceResponse<bool>> DirectShutdownAsync(ShutdownRequest request, CancellationToken ct = default);
    /// <summary>Sends a save-data configuration command to a device. Returns the command ID.</summary>
    Task<FreshlianceResponse<int>> SaveDataConfigAsync(SaveDataConfigRequest request, CancellationToken ct = default);
    /// <summary>Sends a direct configuration command to a device. Returns the command ID.</summary>
    Task<FreshlianceResponse<int>> DirectConfigAsync(DirectConfigRequest request, CancellationToken ct = default);
    /// <summary>Deletes a pending remote command for a device.</summary>
    Task<FreshlianceResponse<bool>> DeleteCommandAsync(DeleteCommandRequest request, CancellationToken ct = default);
}
