using FreshlianceGateway.Sdk.Core;
using FreshlianceGateway.Sdk.Models;
using FreshlianceGateway.Sdk.Models.Data;

namespace FreshlianceGateway.Sdk.Services;

/// <summary>Provides operations for querying device telemetry and alarm data from Freshliance Cloud.</summary>
public interface IDeviceDataService
{
    /// <summary>Retrieves a paginated list of device data (telemetry) records from Freshliance Cloud.</summary>
    Task<FreshlianceResponse<PageResult<DeviceDataResponse>>> GetDataPageAsync(GetDeviceDataRequest request, CancellationToken ct = default);
    /// <summary>Retrieves a paginated list of device alarm records from Freshliance Cloud.</summary>
    Task<FreshlianceResponse<PageResult<AlarmDataResponse>>> GetAlarmPageAsync(GetAlarmDataRequest request, CancellationToken ct = default);
}
