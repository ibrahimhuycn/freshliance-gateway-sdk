using FreshlianceGateway.Sdk.Models.Data;

namespace Freshliance.Dashboard.Services;

public readonly record struct DashboardSnapshot(
    int TotalDevices,
    int OnlineCount,
    int OfflineCount,
    int AlarmCount,
    int AbnormalCount,
    int InactiveCount,
    DateTimeOffset LastUpdated,
    List<AlarmDataResponse> RecentAlarms
);
