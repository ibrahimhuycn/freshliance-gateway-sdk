using FreshlianceGateway.Sdk.Core;
using FreshlianceGateway.Sdk.Models.User;

namespace FreshlianceGateway.Sdk.Services;

/// <summary>Provides operations for managing the current user's profile and information in Freshliance Cloud.</summary>
public interface IUserService
{
    /// <summary>Obtains the current user's information from Freshliance Cloud.</summary>
    Task<FreshlianceResponse<UserInfoResponse>> GetAsync(CancellationToken ct = default);
    /// <summary>Updates the current user's information in Freshliance Cloud.</summary>
    Task<FreshlianceResponse<bool>> UpdateAsync(UpdateUserInfoRequest request, CancellationToken ct = default);
}
