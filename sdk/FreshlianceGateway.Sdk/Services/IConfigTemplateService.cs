using FreshlianceGateway.Sdk.Core;
using FreshlianceGateway.Sdk.Models;
using FreshlianceGateway.Sdk.Models.Template;

namespace FreshlianceGateway.Sdk.Services;

/// <summary>Provides operations for managing device configuration templates in Freshliance Cloud.</summary>
public interface IConfigTemplateService
{
    /// <summary>Retrieves a paginated list of configuration templates from Freshliance Cloud.</summary>
    Task<FreshlianceResponse<PageResult<TemplatePageItemResponse>>> GetPageAsync(GetTemplatePageRequest request, CancellationToken ct = default);
    /// <summary>Retrieves a specific configuration template by its identifier.</summary>
    Task<FreshlianceResponse<TemplateDetailResponse>> GetAsync(GetTemplateDetailRequest request, CancellationToken ct = default);
    /// <summary>Creates a new configuration template in Freshliance Cloud.</summary>
    Task<FreshlianceResponse<bool>> CreateAsync(CreateTemplateRequest request, CancellationToken ct = default);
    /// <summary>Updates an existing configuration template in Freshliance Cloud.</summary>
    Task<FreshlianceResponse<bool>> UpdateAsync(UpdateTemplateRequest request, CancellationToken ct = default);
    /// <summary>Deletes a configuration template from Freshliance Cloud.</summary>
    Task<FreshlianceResponse<bool>> DeleteAsync(DeleteTemplateRequest request, CancellationToken ct = default);
}
