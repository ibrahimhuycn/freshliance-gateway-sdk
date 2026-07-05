using FreshlianceGateway.Sdk.Core;
using FreshlianceGateway.Sdk.Models.User;

namespace FreshlianceGateway.Sdk.Services;

/// <summary>Implements <see cref="IUserService"/>.</summary>
public class UserService : IUserService
{
    private readonly FreshlianceClient _client;

    /// <summary>Initializes a new instance of <see cref="UserService"/> with the given Freshliance client.</summary>
    public UserService(FreshlianceClient client) => _client = client;

    /// <summary>See <see cref="IUserService.GetAsync"/>.</summary>
    public Task<FreshlianceResponse<UserInfoResponse>> GetAsync(CancellationToken ct = default)
        => _client.PostAsync<UserInfoResponse>("gw.userInfo.get", null, ct);

    /// <summary>See <see cref="IUserService.UpdateAsync"/>.</summary>
    public Task<FreshlianceResponse<bool>> UpdateAsync(UpdateUserInfoRequest request, CancellationToken ct = default)
        => _client.PostAsync<bool>("gw.userInfo.update", request, ct);
}
