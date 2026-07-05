using FreshlianceGateway.Sdk.Core;
using FreshlianceGateway.Sdk.Models;
using FreshlianceGateway.Sdk.Models.Device;

namespace FreshlianceGateway.Sdk.Services;

/// <summary>Provides operations for querying devices and their metadata from Freshliance Cloud.</summary>
public interface IDeviceService
{
    /// <summary>Retrieves all available device categories from Freshliance Cloud.</summary>
    Task<FreshlianceResponse<List<CategoryResponse>>> GetCategoriesAsync(CancellationToken ct = default);
    /// <summary>Retrieves a paginated list of devices from Freshliance Cloud.</summary>
    Task<FreshlianceResponse<PageResult<DevicePageItemResponse>>> GetPageAsync(GetDevicePageRequest request, CancellationToken ct = default);
    /// <summary>Retrieves a paginated list of device records from Freshliance Cloud.</summary>
    Task<FreshlianceResponse<PageResult<RecordPageItemResponse>>> GetRecordPageAsync(GetRecordPageRequest request, CancellationToken ct = default);
    /// <summary>Retrieves a paginated list of sub-devices from Freshliance Cloud.</summary>
    Task<FreshlianceResponse<PageResult<SubDevicePageItemResponse>>> GetSubDevicePageAsync(GetSubDevicePageRequest request, CancellationToken ct = default);
}
