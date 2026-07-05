using FreshlianceGateway.Sdk.Core;
using FreshlianceGateway.Sdk.Models;
using FreshlianceGateway.Sdk.Models.Template;

namespace FreshlianceGateway.Sdk.Services;

/// <summary>Implements <see cref="IConfigTemplateService"/>.</summary>
public class ConfigTemplateService : IConfigTemplateService
{
    private readonly FreshlianceClient _client;

    /// <summary>Initializes a new instance of <see cref="ConfigTemplateService"/> with the given Freshliance client.</summary>
    public ConfigTemplateService(FreshlianceClient client) => _client = client;

    /// <summary>See <see cref="IConfigTemplateService.GetPageAsync"/>.</summary>
    public Task<FreshlianceResponse<PageResult<TemplatePageItemResponse>>> GetPageAsync(GetTemplatePageRequest request, CancellationToken ct = default)
        => _client.PostAsync<PageResult<TemplatePageItemResponse>>("gw.configTemplate.page", request, ct);

    /// <summary>See <see cref="IConfigTemplateService.GetAsync"/>.</summary>
    public Task<FreshlianceResponse<TemplateDetailResponse>> GetAsync(GetTemplateDetailRequest request, CancellationToken ct = default)
        => _client.PostAsync<TemplateDetailResponse>("gw.configTemplate.get", request, ct);

    /// <summary>See <see cref="IConfigTemplateService.CreateAsync"/>.</summary>
    public Task<FreshlianceResponse<bool>> CreateAsync(CreateTemplateRequest request, CancellationToken ct = default)
        => _client.PostAsync<bool>("gw.configTemplate.create", request, ct);

    /// <summary>See <see cref="IConfigTemplateService.UpdateAsync"/>.</summary>
    public Task<FreshlianceResponse<bool>> UpdateAsync(UpdateTemplateRequest request, CancellationToken ct = default)
        => _client.PostAsync<bool>("gw.configTemplate.update", request, ct);

    /// <summary>See <see cref="IConfigTemplateService.DeleteAsync"/>.</summary>
    public Task<FreshlianceResponse<bool>> DeleteAsync(DeleteTemplateRequest request, CancellationToken ct = default)
        => _client.PostAsync<bool>("gw.configTemplate.delete", request, ct);
}
