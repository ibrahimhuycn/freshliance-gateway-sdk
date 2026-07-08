using FreshlianceGateway.Sdk.Models.Device;

namespace Freshliance.Dashboard.Services;

public sealed class DeviceStateService
{
    private List<DevicePageItemResponse>? _cache;

    public void SetCache(List<DevicePageItemResponse> devices)
    {
        _cache = devices;
    }

    public DevicePageItemResponse? GetByRecordId(int recordId)
    {
        return _cache?.Find(d => d.DeviceInfo.RecordId == recordId);
    }

    public void Invalidate()
    {
        _cache = null;
    }
}
