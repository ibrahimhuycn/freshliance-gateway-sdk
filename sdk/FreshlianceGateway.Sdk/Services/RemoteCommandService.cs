using FreshlianceGateway.Sdk.Core;
using FreshlianceGateway.Sdk.Models.Command;

namespace FreshlianceGateway.Sdk.Services;

/// <summary>Implements <see cref="IRemoteCommandService"/>.</summary>
public class RemoteCommandService : IRemoteCommandService
{
    private readonly FreshlianceClient _client;

    /// <summary>Initializes a new instance of <see cref="RemoteCommandService"/> with the given Freshliance client.</summary>
    public RemoteCommandService(FreshlianceClient client) => _client = client;

    /// <summary>See <see cref="IRemoteCommandService.UpdateParameterAsync"/>.</summary>
    public Task<FreshlianceResponse<bool>> UpdateParameterAsync(UpdateParameterRequest request, CancellationToken ct = default)
        => _client.PostAsync<bool>("gw.deviceCmd.updateParameter", request, ct);

    /// <summary>See <see cref="IRemoteCommandService.SaveDataShutdownAsync"/>.</summary>
    public Task<FreshlianceResponse<bool>> SaveDataShutdownAsync(ShutdownRequest request, CancellationToken ct = default)
        => _client.PostAsync<bool>("gw.deviceCmd.saveDataShutdown", request, ct);

    /// <summary>See <see cref="IRemoteCommandService.DirectShutdownAsync"/>.</summary>
    public Task<FreshlianceResponse<bool>> DirectShutdownAsync(ShutdownRequest request, CancellationToken ct = default)
        => _client.PostAsync<bool>("gw.deviceCmd.directShutdown", request, ct);

    /// <summary>See <see cref="IRemoteCommandService.SaveDataConfigAsync"/>.</summary>
    public Task<FreshlianceResponse<int>> SaveDataConfigAsync(SaveDataConfigRequest request, CancellationToken ct = default)
        => _client.PostAsync<int>("gw.deviceCmd.saveDataConfig", request, ct);

    /// <summary>See <see cref="IRemoteCommandService.DirectConfigAsync"/>.</summary>
    public Task<FreshlianceResponse<int>> DirectConfigAsync(DirectConfigRequest request, CancellationToken ct = default)
        => _client.PostAsync<int>("gw.deviceCmd.directConfig", request, ct);

    /// <summary>See <see cref="IRemoteCommandService.DeleteCommandAsync"/>.</summary>
    public Task<FreshlianceResponse<bool>> DeleteCommandAsync(DeleteCommandRequest request, CancellationToken ct = default)
        => _client.PostAsync<bool>("gw.deviceCmd.delete", request, ct);
}
