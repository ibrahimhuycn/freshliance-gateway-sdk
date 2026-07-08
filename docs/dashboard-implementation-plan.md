# Freshliance Dashboard — Implementation Plan v2

**Branch:** `feat/dashboard-blazor-ui`
**Stack:** Blazor Server (.NET 10), MudBlazor, Blazor-ApexCharts, SignalR, Serilog
**SDK:** Project reference to `sdk/FreshlianceGateway.Sdk/`
**Plan status:** Approved — Ready for implementation

---

## 0. Environment & References

| Resource | Location |
|----------|----------|
| MudBlazor source (docs, examples, icons) | `$env:TEMP\mudblazor` (cloned `https://github.com/MudBlazor/MudBlazor.git`) |
| MudBlazor icons | `Icons.Material.Filled.*` — see Section 10 for full list |
| SDK project | `sdk\FreshlianceGateway.Sdk\FreshlianceGateway.Sdk.csproj` |
| API docs | `gateway-api\*.md` |
| HTML mockup | `docs\dashboard-ui-mockup.html` |

---

## 1. NuGet Packages (latest stable, net10.0 compatible)

| Package | Version | Verified | Purpose |
|---------|---------|----------|---------|
| `MudBlazor` | **9.6.0** | NuGet registration upper bound | UI component library |
| `Blazor-ApexCharts` | **6.1.0** | Has `net10.0` TFM | Time-series line charts |
| `Serilog.AspNetCore` | **10.0.0** | Has `net10.0` TFM | Structured logging for ASP.NET |
| `Serilog.Enrichers.Environment` | **3.0.1** | Standard enricher | Machine name, env vars in logs |
| `Serilog.Enrichers.Thread` | **4.0.0** | Confirmed via search | Thread ID in logs |
| `Serilog.Sinks.Async` | **2.1.0** | Standard sink | Non-blocking log writes |
| `Serilog.Sinks.File` | **6.0.0** | Referenced by Serilog.AspNetCore 9.0.0 | Rolling file sink |
| `Serilog.Sinks.Console` | **6.1.1** | Referenced by Serilog.AspNetCore 10.0.0 | Console output |

All above verified via NuGet API. Use `dotnet add package <id> --version <x>` for exact versions.

---

## 2. Project Configuration

### 2.1 csproj — Strict build requirements

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
    <RootNamespace>Freshliance.Dashboard</RootNamespace>
  </PropertyGroup>

  <ItemGroup>
    <ProjectReference Include="..\..\sdk\FreshlianceGateway.Sdk\FreshlianceGateway.Sdk.csproj" />
    <PackageReference Include="MudBlazor" Version="9.6.0" />
    <PackageReference Include="Blazor-ApexCharts" Version="6.1.0" />
    <PackageReference Include="Serilog.AspNetCore" Version="10.0.0" />
    <PackageReference Include="Serilog.Enrichers.Environment" Version="3.0.1" />
    <PackageReference Include="Serilog.Enrichers.Thread" Version="4.0.0" />
    <PackageReference Include="Serilog.Sinks.Async" Version="2.1.0" />
  </ItemGroup>
</Project>
```

### 2.2 appsettings.json

```json
{
  "Freshliance": {
    "AppId": "",
    "PrivateKeyPem": "",
    "PublicKeyPem": "",
    "AcceptLanguage": "en-US"
  },
  "Polling": {
    "IntervalSeconds": 30,
    "Enabled": true
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "Microsoft.AspNetCore": "Warning",
        "System.Net.Http": "Warning"
      }
    },
    "WriteTo": [
      { "Name": "Console" },
      {
        "Name": "File",
        "Args": {
          "path": "logs/dashboard-.log",
          "rollingInterval": "Day",
          "retainedFileCountLimit": 30,
          "formatter": "Serilog.Formatting.Compact.RenderedCompactJsonFormatter"
        }
      }
    ],
    "Enrich": [ "FromLogContext", "WithMachineName", "WithThreadId" ]
  },
  "AllowedHosts": "*"
}
```

### 2.3 Program.cs — Serilog bootstrap

```csharp
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddFreshlianceGateway(options =>
{
    options.AppId = builder.Configuration["Freshliance:AppId"]!;
    options.PrivateKeyPem = builder.Configuration["Freshliance:PrivateKey"]!;
    options.PublicKeyPem = builder.Configuration["Freshliance:PublicKeyPem"];
    options.AcceptLanguage = builder.Configuration["Freshliance:AcceptLanguage"];
});
builder.Services.AddMudServices();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddSignalR();
builder.Services.AddScoped<UserPreferenceService>();
builder.Services.AddScoped<DeviceStateService>();
builder.Services.Configure<PollingOptions>(
    builder.Configuration.GetSection("Polling"));
builder.Services.AddHostedService<PollingService>();

var app = builder.Build();
app.UseSerilogRequestLogging();
app.MapHub<DashboardHub>("/hubs/dashboard");
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
```

---

## 3. File Listing (60 files total)

### 3.1 Dashboard App (32 files)

```
src/Freshliance.Dashboard/
├── Freshliance.Dashboard.csproj
├── Program.cs
├── appsettings.json
├── appsettings.Development.json
├── Components/
│   ├── App.razor
│   ├── Routes.razor
│   ├── _Imports.razor
│   ├── Layout/
│   │   ├── MainLayout.razor
│   │   ├── MainLayout.razor.css
│   │   └── NavMenu.razor
│   │       NavMenu.razor.css
│   ├── Pages/
│   │   ├── Dashboard.razor
│   │   │   Dashboard.razor.cs
│   │   │   Dashboard.razor.css
│   │   ├── Devices.razor
│   │   │   Devices.razor.cs
│   │   │   Devices.razor.css
│   │   ├── DeviceDetail.razor
│   │   │   DeviceDetail.razor.cs
│   │   │   DeviceDetail.razor.css
│   │   ├── Alarms.razor
│   │   │   Alarms.razor.cs
│   │   ├── Groups.razor
│   │   │   Groups.razor.cs
│   │   │   Groups.razor.css
│   │   ├── Templates.razor
│   │   │   Templates.razor.cs
│   │   └── Settings.razor
│   │       Settings.razor.cs
│   └── Shared/
│       ├── DeviceStatusChip.razor
│       ├── DeviceStatusChip.razor.cs
│       ├── LastReadingCard.razor
│       ├── LastReadingCard.razor.cs
│       ├── ProbeTabs.razor
│       ├── ProbeTabs.razor.cs
│       ├── TimeRangePicker.razor
│       ├── TimeRangePicker.razor.cs
│       ├── ConfirmDialog.razor
│       ├── ConfirmDialog.razor.cs
│       ├── LoadingSkeleton.razor
│       └── EmptyState.razor
├── Hubs/
│   └── DashboardHub.cs
├── Services/
│   ├── PollingOptions.cs
│   ├── PollingService.cs
│   ├── DashboardSnapshot.cs
│   ├── DeviceStateService.cs
│   └── UserPreferenceService.cs
└── wwwroot/
    └── css/
        └── app.css
```

**Rule:** One class per file. Code-behind via `Page.razor.cs`. Scoped CSS via `Page.razor.css`.

### 3.2 Unit Test Project (15 files)

```
src/Freshliance.Dashboard.Tests/
├── Freshliance.Dashboard.Tests.csproj
├── Pages/
│   ├── DashboardTests.cs
│   ├── DevicesTests.cs
│   ├── DeviceDetailTests.cs
│   ├── AlarmsTests.cs
│   ├── GroupsTests.cs
│   ├── TemplatesTests.cs
│   └── SettingsTests.cs
├── Shared/
│   ├── DeviceStatusChipTests.cs
│   ├── LastReadingCardTests.cs
│   ├── ProbeTabsTests.cs
│   └── TimeRangePickerTests.cs
└── Services/
    ├── PollingServiceTests.cs
    ├── DeviceStateServiceTests.cs
    └── UserPreferenceServiceTests.cs
```

### 3.3 Integration Test Project (6 files)

```
src/Freshliance.Dashboard.IntegrationTests/
├── Freshliance.Dashboard.IntegrationTests.csproj
├── DashboardIntegrationTests.cs
├── DeviceDataPaginationTests.cs
├── UserPreferenceIntegrationTests.cs
├── ErrorRecoveryTests.cs
└── SignalRIntegrationTests.cs
```

### 3.4 E2E Test Project (7 files)

```
src/Freshliance.Dashboard.E2ETests/
├── Freshliance.Dashboard.E2ETests.csproj
├── DashboardE2ETests.cs
├── NavigationE2ETests.cs
├── DeviceDetailChartE2ETests.cs
├── GroupManagementE2ETests.cs
├── SettingsFormE2ETests.cs
└── ResponsiveE2ETests.cs
```

### 3.5 File Count Summary

| Project | Files | Est. Lines |
|---------|-------|------------|
| `Freshliance.Dashboard` | 32 | ~4000 |
| `Freshliance.Dashboard.Tests` | 15 | ~2500 |
| `Freshliance.Dashboard.IntegrationTests` | 6 | ~1200 |
| `Freshliance.Dashboard.E2ETests` | 7 | ~1200 |
| **Total** | **60** | **~8900** |

---

## 4. Parallel Agent Implementation Strategy

Phases that can run in **parallel** via agents (no cross-dependencies):

### Wave 1 — Foundation (can run in 1 agent)
| Agent | Files | Description |
|-------|-------|-------------|
| A1 | `csproj`, `Program.cs`, `appsettings*.json`, `_Imports.razor`, `App.razor`, `Routes.razor`, `PollingOptions.cs`, `UserPreferenceService.cs` | Project scaffold + config + DI + Serilog + core services |

### Wave 2 — Layout + Shared (2 agents parallel after Wave 1)
| Agent | Files | Description |
|-------|-------|-------------|
| A2 | `MainLayout.razor`, `MainLayout.razor.css`, `NavMenu.razor`, `NavMenu.razor.css`, `app.css` | Shell layout — theme, nav, branding |
| A3 | `DeviceStatusChip.razor/+.cs`, `LastReadingCard.razor/+.cs`, `LoadingSkeleton.razor`, `EmptyState.razor`, `ConfirmDialog.razor/+.cs`, `ProbeTabs.razor/+.cs`, `TimeRangePicker.razor/+.cs` | All shared components |

### Wave 3 — Pages (4 agents parallel after Wave 2)
| Agent | Files | Description |
|-------|-------|-------------|
| A4 | `Devices.razor/+.cs/+.css` | Device list grid with filters + pagination |
| A5 | `DeviceDetail.razor/+.cs/+.css` | Probe tabs, ApexCharts, time range, alarm history |
| A6 | `Dashboard.razor/+.cs/+.css` + `Alarms.razor/+.cs` | Dashboard cards + alarms grid |
| A7 | `Groups.razor/+.cs/+.css` + `Templates.razor/+.cs` + `Settings.razor/+.cs` | Group tree, template CRUD, user settings |

### Wave 4 — SignalR + State (1 agent after Wave 3)
| Agent | Files | Description |
|-------|-------|-------------|
| A8 | `DashboardHub.cs`, `PollingService.cs`, `DashboardSnapshot.cs`, `DeviceStateService.cs` | Real-time infrastructure |

**Total: 8 agents across 4 waves. Wave 1 → Wave 2→3 → Wave 4.**

---

## 5. Gap Analysis (API vs UI needs)

| # | Gap | Severity | Mitigation |
|---|-----|----------|------------|
| **G1** | No "device by recordId" endpoint | Medium | `DeviceStateService` scoped cache. Navigate with recordId, look up from cache. Fallback: fetch device page filtered by deviceSn. |
| **G2** | No alarm acknowledge/handle endpoint | Medium | Show `handleStatus` badge. Disable "Mark handled" button with tooltip. Log warning on button press. |
| **G3** | No read-back for notification config | Low | Show empty defaults when creating config. No read-back display. |
| **G4** | No global device count aggregation | Medium | Fetch page 1, aggregate `deviceDeviceStateCount` across rows. For >50 devices, paginate through all pages and sum. Log warning if total > expected. |
| **G5** | Multi-probe data = 3 API calls | Medium | `Task.WhenAll` for 3 probe types. Show `LoadingSkeleton` per tab. Log each call duration. |
| **G6** | Epoch timestamps, Celsius temps | Low | `UserPreferenceService.FormatDateTime()` and `FormatTemperature()` convert client-side based on `gw.userInfo.get`. |
| **G7** | Max 50/page — chart data aggregation | Medium | Sequential pagination, merge into single `List<DeviceDataResponse>`. Cap at 2000 points. Log info if capped. |
| **G8** | No server push from Freshliance API | Low | `PollingService` (IHostedService) polls → `DashboardHub` pushes via SignalR. Acceptable. |

---

## 6. Page-by-Page Specifications

### 6.1 Dashboard (`Dashboard.razor`)

**API calls:**
```
IDeviceService.GetPageAsync(new GetDevicePageRequest { PageNum = 1, PageSize = 50 })
IGroupService.GetTreeAsync()
IDeviceDataService.GetAlarmPageAsync(...) — top 10 most recent
```

**UI:**
- 4× `MudCard` (Total, Online, Offline, In Alarm) with `@Icons.Material.Filled.Devices`, `@Icons.Material.Filled.Cloud`, `@Icons.Material.Filled.CloudOff`, `@Icons.Material.Filled.NotificationsActive`
- `MudTable` — recent 10 alarms
- `MudTreeView` — collapsed group hierarchy with counts
- `MudSwitch` — auto-refresh toggle, reading from `PollingOptions`
- `MudText` — "Last updated: Xs ago" using `System.Timers.Timer` for count-up

**SignalR:** Subscribe to `DashboardHub.On<DashboardSnapshot>("DashboardUpdate", ...)`. Unsubscribe in `DisposeAsync`.

**Logging:** `Log.Information("Dashboard loaded — {DeviceCount} devices, {OnlineCount} online, {AlarmCount} in alarm", ...)`

### 6.2 Devices (`Devices.razor`)

**API calls:**
```
IDeviceService.GetPageAsync(request) — ServerData callback
IDeviceService.GetCategoriesAsync() — cached once for filter labels
```

**UI:**
- `MudToolBar` with `MudTextField` (keyword), `MudSelect` (deviceStatus, productType, alarmStatus), `MudIconButton` for refresh (`@Icons.Material.Filled.Refresh`)
- `MudDataGrid` with `ServerData` callback
  - Columns: deviceName (linked), deviceSn, `DeviceStatusChip`, alarm status badge, `MudProgressLinear` for battery, productModel, `LastReadingCard` inline
  - `RowTemplate` expand: `subDeviceLastDataList` per probe
- `MudPagination` from `PageResult.Total`

**Navigation:** `NavigationManager.NavigateTo($"/device/{recordId}")` — store `DevicePageItemResponse` into `DeviceStateService` before navigating.

**Logging:** `Log.Debug("Device page {PageNum} loaded — {Count} rows of {Total}", ...)`

### 6.3 Device Detail (`DeviceDetail.razor`)

**Route:** `@page "/device/{RecordId:int}"`

**API calls (parallel):**
```csharp
var deviceInfo = DeviceStateService.GetByRecordId(RecordId);
// fallback if cache miss:
var devices = await DeviceService.GetPageAsync(new() { PageNum = 1, PageSize = 50 });

var (data0, data1, data2, alarms) = await Task.WhenAll(
    DeviceDataService.GetDataPageAsync(new() { RecordId, ProbeType = 0, PageSize = 50 }),
    DeviceDataService.GetDataPageAsync(new() { RecordId, ProbeType = 1, PageSize = 50 }),
    DeviceDataService.GetDataPageAsync(new() { RecordId, ProbeType = 2, PageSize = 50 }),
    DeviceDataService.GetAlarmPageAsync(new() { RecordId, PageSize = 50 })
);
```

**UI:**
- `MudBreadcrumbs`: Dashboard → Devices → {deviceName}
- Header `MudCard`: device name, SN, model, `DeviceStatusChip`, battery `MudProgressLinear`
- `MudTabs` per probe type (icons: `@Icons.Material.Filled.Thermostat`, `@Icons.Material.Filled.Sensors`)
  - Tab: `LastReadingCard` + `ApexChart` (Blazor-ApexCharts)
  - Tab Panel 4: "Alarm History" `MudTable`
- `MudToolBar`: `MudDateRangePicker`, preset buttons (1h/6h/24h/7d), CSV export
- `TimeRangePicker` — shared component for presets

**Chart config (Blazor-ApexCharts):**
```csharp
<ApexChart TItem="DeviceDataResponse"
    Options="chartOptions"
    Series="chartSeries">
    <ApexPointSeries TItem="DeviceDataResponse"
        Items="temperatureData"
        Name="Temperature"
        XValue="d => DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(d.DataTime)).DateTime"
        YValue="d => d.Temperature ?? 0"
        Color="#F44336" />
    <ApexPointSeries TItem="DeviceDataResponse"
        Items="humidityData"
        Name="Humidity"
        XValue="d => DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(d.DataTime)).DateTime"
        YValue="d => d.Humidity ?? 0"
        Color="#2196F3"
        YAggregate="AggregateType.Avg" />
</ApexChart>
```

**CSV export:** `var csv = "Time,Temperature,Humidity\n" + string.Join("\n", data.Select(...))`. Write to `MemoryStream`, trigger download via `IJSRuntime.InvokeVoidAsync("downloadFile", fileName, Convert.ToBase64String(bytes))`.

**Logging:** `Log.Information("DeviceDetail loaded — RecordId {RecordId}, {DataCount} data points", ...)`

### 6.4 Alarms (`Alarms.razor`)

**API calls:**
```
IDeviceDataService.GetAlarmPageAsync(request)
```

**UI:**
- `MudToolBar`: `MudSelect` (alarmProperty, alarmType, handleStatus, probeType), `MudDateRangePicker`
- `MudDataGrid` with `ServerData`
  - Row color: red (`#FFEBEE`) for `handleStatus == Processing`
  - Columns: device (linked → DeviceDetail), probeType, alarmProperty icon, alarmZone (H/L chip), threshold, actual value, alarmTime, handleStatus badge
- `handleStatus` displayed via badge (Processing=warning, Processed=success, Ignored=default)

**Logging:** `Log.Warning("Alarm list — {Total} alarms, {Unhandled} unhandled", ...)`

### 6.5 Groups (`Groups.razor`)

**API calls:**
```
IGroupService.GetTreeAsync()
IGroupDeviceService.GetAllocatedPageAsync(request)
IGroupDeviceService.GetUnallocatedPageAsync(request) — in "Add Devices" dialog
IGroupService.CreateAsync() / UpdateAsync() / DeleteAsync()
IGroupDeviceService.BindAsync() / UnbindAsync()
```

**UI:**
- Split panel: left 280px `MudTreeView` (icons: `@Icons.Material.Filled.Folder`, `@Icons.Material.Filled.FolderOpen`), right `MudDataGrid`
- Tree context menu: "Rename" / "Delete" (`@Icons.Material.Filled.Edit`, `@Icons.Material.Filled.Delete`)
- "Add Group" button → `MudDialog` with `MudForm`
- "Add Devices" button → `MudDialog` with unallocated device grid + `MudCheckBox` multi-select
- Remove device → `ConfirmDialog`

**Validation:** Cannot delete group with devices (warning). Only delete empty.

**Logging:** `Log.Information("Group '{GroupName}' created/updated/deleted", ...)`

### 6.6 Templates (`Templates.razor`)

**API calls:**
```
IConfigTemplateService.GetPageAsync(request)
IConfigTemplateService.GetAsync(configId)
IConfigTemplateService.CreateAsync() / UpdateAsync() / DeleteAsync()
IDeviceService.GetCategoriesAsync()
```

**UI:**
- `MudDataGrid` with `ServerData`
- Expand row: nested `MudTable` with `sensorConfigProbeList` alarm thresholds
- "Create Template" button → `MudDialog` with full form:
  - `templateName`, `categoryId` dropdown, `collectInterval`, `startDelay`
  - Dynamic alarm config rows: probeType, alarmZone, alarmProperty, alarmType, alarmThreshold, alarmDelay, alarmWay
  - `MudButton` "Add Alarm" (`@Icons.Material.Filled.Add`) per row
- Edit/Delete via row buttons

**Logging:** `Log.Information("Template '{Name}' created/updated/deleted — {AlarmCount} alarm points", ...)`

### 6.7 Settings (`Settings.razor`)

**API calls:**
```
IUserService.GetAsync()
IUserService.UpdateAsync(request)
```

**UI:**
- `MudCard` with `MudForm`
- Fields: email (readonly), nickname, timezone (`MudAutocomplete`), language (`MudSelect`), dateFormat (`MudSelect`), temperatureUnit (`MudSelect`)
- `MudAlert` — SMS/Voice balance info boxes
- Save → `MudButton` with `@Icons.Material.Filled.Save`

**Validation:** Nickname 1-30 chars. Only send changed fields to `UpdateAsync`.

**Logging:** `Log.Information("User settings updated — timezone={Tz}, language={Lang}, tempUnit={Unit}", ...)`

### 6.8 data-testid Requirement

Every UI element described in Sections 6.1–6.7 must carry `data-testid` attributes. The complete catalog is in Section 16.5. During implementation, each page MUST reference the catalog and add every listed attribute. bUnit and Playwright tests depend on these.

---

## 7. Serilog Configuration

### 7.1 Log Levels by Layer

| Layer | Level | Rationale |
|-------|-------|-----------|
| SDK calls | `Information` | Track each API call: method, timing, result |
| SDK errors | `Warning` | `FreshlianceException` caught and handled |
| SDK unexpected | `Error` | Unhandled exceptions from SDK |
| Page lifecycle | `Debug` | `OnInitializedAsync`, `OnParametersSetAsync` |
| PollingService | `Information` | Each poll cycle + timing |
| DashboardHub | `Debug` | Connection/disconnection events |
| MudBlazor internals | `Warning` (filtered) | Override in config |
| System.Net.Http | `Warning` (filtered) | Override in config |

### 7.2 Enrichers

- `FromLogContext()` — push properties via `LogContext.PushProperty()`
- `WithMachineName()` — `Environment.MachineName`
- `WithThreadId()` — `Environment.CurrentManagedThreadId`

### 7.3 Sinks

- **Console** — development, always on
- **Async File wrapper** — wraps the file sink for non-blocking writes
- **Rolling File** — `logs/dashboard-20260707.log`, retained 30 days, `RenderedCompactJsonFormatter`

### 7.4 Request logging

`app.UseSerilogRequestLogging()` — logs every HTTP request with `{Method} {Path} {StatusCode} {Elapsed}`.

---

## 8. MudBlazor Icons Reference

All using `@Icons.Material.Filled.*`. Full list verified against cloned repo at `$env:TEMP\mudblazor`.

### Page Icons
| Page | Icon |
|------|------|
| Dashboard | `Dashboard` |
| Devices | `Devices` |
| Device Detail | `Sensors` |
| Alarms | `NotificationsActive` |
| Groups | `Folder` |
| Templates | `Description` |
| Settings | `Settings` |

### Status Icons
| Status | Icon |
|--------|------|
| Online | `Cloud` |
| Offline | `CloudOff` |
| Alarm | `WarningAmber` |
| Abnormal | `ErrorOutline` |
| Normal | `CheckCircle` |
| Low Battery | `BatteryAlert` |
| Full Battery | `BatteryFull` |

### Sensor/Reading Icons
| Metric | Icon |
|--------|------|
| Temperature | `Thermostat` |
| Humidity | `WaterDrop` |
| Light/Illuminance | `LightMode` |
| CO2 | `Co2` |

### Action Icons
| Action | Icon |
|--------|------|
| Refresh | `Refresh` |
| Search | `Search` |
| Download/Export | `FileDownload` |
| Add/Create | `Add` |
| Edit | `Edit` |
| Delete | `Delete` |
| Save | `Save` |
| Close | `Close` |
| More (overflow) | `MoreVert` |
| Arrow Back | `ArrowBack` |

### Chart/Analytics Icons
| Use | Icon |
|-----|------|
| Line chart | `ShowChart` |
| Bar chart | `BarChart` |
| Timeline | `Timeline` |
| Analytics | `Analytics` |
| Insights | `Insights` |

### Navigation Icons
| Use | Icon |
|-----|------|
| Menu toggle | `Menu` |
| Home | `Home` |
| Expand/Accordion | `ExpandMore` / `ChevronRight` |

---

## 9. Polling Architecture (Event-Based, No Custom Hub)

Blazor Server's built-in SignalR circuit already handles all client↔server communication. No custom `DashboardHub` or `HubConnection` is needed. Instead, `PollingService` fires a C# event — the Dashboard page subscribes and updates its bound properties. Blazor's circuit pushes UI diffs automatically.

```
┌──────────────────────────────────────┐
│          Freshliance API              │
└──────────┬───────────────────────────┘
           │ HTTP POST (poll)
           ▼
┌──────────────────────────────────────┐
│   PollingService (Singleton,         │
│   IHostedService)                     │
│   Timer(PollingOptions.Interval)     │
│   ├─ IDeviceService.GetPageAsync()    │
│   ├─ IDeviceDataService.GetAlarm*()   │
│   ├─ Build DashboardSnapshot          │
│   └─ Fire SnapshotUpdated event ─────┼────┐
└──────────────────────────────────────┘    │
                                            │ C# event (in-process)
                                            ▼
┌──────────────────────────────────────┐    │
│   Dashboard.razor (Scoped)            │◄───┘
│   Inject<PollingService>              │
│   Subscribe: SnapshotUpdated          │
│   Handler: update _snapshot           │
│           → StateHasChanged()         │
│           → Blazor circuit pushes UI   │────► Browser
└──────────────────────────────────────┘

┌──────────────────────────────────────┐
│   DeviceDetail.razor (Scoped)         │
│   Own PeriodicTimer at 30s            │
│   Tick: refetch latest page           │
│         → merge into chart points     │
│         → StateHasChanged()           │
│         → Blazor circuit pushes UI    │────► Browser
└──────────────────────────────────────┘
```

**No custom SignalR hub. No `HubConnection` client. No extra NuGet. Just C# events + Blazor Server's built-in circuit.**

### PollingService (updated)
```csharp
public sealed class PollingService : BackgroundService
{
    public event Action<DashboardSnapshot>? SnapshotUpdated;
    public DashboardSnapshot? CurrentSnapshot { get; private set; }

    // Timer → fetch → build snapshot → CurrentSnapshot = snapshot
    // → SnapshotUpdated?.Invoke(snapshot)
}
```

### Dashboard subscriber
```csharp
[Inject] private PollingService PollingService { get; set; } = null!;

protected override void OnInitializedAsync()
{
    PollingService.SnapshotUpdated += OnSnapshotUpdated;
    // Get initial snapshot immediately if available
    if (PollingService.CurrentSnapshot.HasValue)
        ApplySnapshot(PollingService.CurrentSnapshot.Value);
}

private void OnSnapshotUpdated(DashboardSnapshot snapshot)
    => InvokeAsync(() => { ApplySnapshot(snapshot); StateHasChanged(); });

void IDisposable.Dispose()
    => PollingService.SnapshotUpdated -= OnSnapshotUpdated;
```

### DeviceDetail auto-refresh
```csharp
private PeriodicTimer? _liveTimer;

// Start when viewing a "live" time preset
async Task StartLiveRefresh()
{
    _liveTimer = new PeriodicTimer(TimeSpan.FromSeconds(30));
    while (await _liveTimer.WaitForNextTickAsync() && !_disposed)
    {
        if (_isOnAlarmTab || _isCustomRange) continue;
        await FetchLatestDataAsync();
        await InvokeAsync(StateHasChanged);
    }
}
```

**Polling behavior:**
- Timer fires every `PollingOptions.IntervalSeconds` (default 30, from appsettings)
- `Enabled=false` → timer stops, no events fired
- On failure: `Log.Warning(ex, "Poll cycle failed")`, continue running
- On consecutive failures > 5: `Log.Error("Poll cycle failed 5 consecutive times")`, fire snapshot with zero counts

---

## 10. State Management

### DeviceStateService (Scoped)
```csharp
public class DeviceStateService
{
    private List<DevicePageItemResponse>? _cache;
    public void SetCache(List<DevicePageItemResponse> devices);
    public DevicePageItemResponse? GetByRecordId(int recordId);
    public void Invalidate();
}
```

### UserPreferenceService (Scoped)
```csharp
public class UserPreferenceService
{
    public UserInfoResponse? UserInfo { get; private set; }
    public async Task LoadAsync(IUserService userService);
    public string FormatDateTime(long epochMs);      // localized, per user timezone + dateFormat
    public string FormatTemperature(double? celsius); // converts to Fahrenheit if preference set
}
```

---

## 11. Error Handling Strategy

| Layer | Pattern |
|-------|---------|
| SDK call | `try { response.EnsureSuccess(); } catch (FreshlianceException ex) { Log.Warning(ex, "..."); errorMessage = ex.Message; }` |
| Page | `@if (!string.IsNullOrEmpty(errorMessage)) { <MudAlert Severity="Error" Close="true">@errorMessage</MudAlert> }` |
| Global | `ErrorBoundary` in `MainLayout` wrapping `@Body` |
| Network | `MudSnackbar` for transient failures, `MudAlert` for persistent |
| Loading | `@if (_isLoading) { <LoadingSkeleton /> }` |
| Empty | `@if (!_isLoading && !data.Any()) { <EmptyState Icon="..." Message="..." /> }` |

---

## 12. Responsive Breakpoints (MudBlazor)

| Breakpoint | Layout |
|------------|--------|
| `>= 960px` | Full: nav drawer open (240px), 4-col dashboard cards, groups split panel |
| `600–959px` | Mini nav drawer (icons only), 2-col dashboard cards, single-col panels |
| `< 600px` | Nav drawer hidden (hamburger `MudIconButton`), single column, cards stack |

Use `MudGrid` + `MudItem` with `xs/md/lg` attributes. `MudHidden` for conditional visibility.

---

## 13. Implementation Phases (File → Phase mapping)

### Phase 1 — Project Scaffold (6 files)
- `Freshliance.Dashboard.csproj`
- `Program.cs`
- `appsettings.json`
- `appsettings.Development.json`
- `PollingOptions.cs`
- `UserPreferenceService.cs`

### Phase 2 — Layout Shell (5 files)
- `App.razor`
- `Routes.razor`
- `_Imports.razor`
- `MainLayout.razor` + `.razor.css`
- `NavMenu.razor` + `.razor.css`

### Phase 3 — Shared Components (13 files)
- `DeviceStatusChip.razor` + `.cs`
- `LastReadingCard.razor` + `.cs`
- `ProbeTabs.razor` + `.cs`
- `TimeRangePicker.razor` + `.cs`
- `ConfirmDialog.razor` + `.cs`
- `LoadingSkeleton.razor`
- `EmptyState.razor`
- `wwwroot/css/app.css`

### Phase 4 — Pages (14 files)
- `Dashboard.razor` + `.cs` + `.css`
- `Devices.razor` + `.cs` + `.css`
- `DeviceDetail.razor` + `.cs` + `.css`
- `Alarms.razor` + `.cs`
- `Groups.razor` + `.cs` + `.css`
- `Templates.razor` + `.cs`
- `Settings.razor` + `.cs`

### Phase 5 — SignalR + Polling (4 files)
- `DashboardHub.cs`
- `PollingService.cs`
- `DashboardSnapshot.cs`
- `DeviceStateService.cs`

### Phase 6 — Unit Tests (15 files)
- `Freshliance.Dashboard.Tests.csproj`
- `Pages/DashboardTests.cs`
- `Pages/DevicesTests.cs`
- `Pages/DeviceDetailTests.cs`
- `Pages/AlarmsTests.cs`
- `Pages/GroupsTests.cs`
- `Pages/TemplatesTests.cs`
- `Pages/SettingsTests.cs`
- `Shared/DeviceStatusChipTests.cs`
- `Shared/LastReadingCardTests.cs`
- `Shared/ProbeTabsTests.cs`
- `Shared/TimeRangePickerTests.cs`
- `Services/PollingServiceTests.cs`
- `Services/DeviceStateServiceTests.cs`
- `Services/UserPreferenceServiceTests.cs`

### Phase 7 — Integration + E2E Tests (13 files)
- `Freshliance.Dashboard.IntegrationTests.csproj`
- `DashboardIntegrationTests.cs`
- `DeviceDataPaginationTests.cs`
- `UserPreferenceIntegrationTests.cs`
- `ErrorRecoveryTests.cs`
- `SignalRIntegrationTests.cs`
- `Freshliance.Dashboard.E2ETests.csproj`
- `DashboardE2ETests.cs`
- `NavigationE2ETests.cs`
- `DeviceDetailChartE2ETests.cs`
- `GroupManagementE2ETests.cs`
- `SettingsFormE2ETests.cs`
- `ResponsiveE2ETests.cs`

### Phase 8 — Verify & Polish
- `dotnet build` with `TreatWarningsAsErrors` → zero warnings
- Run all ~128 tests: `dotnet test` → all green
- Run app: `dotnet run` → MudBlazor theme loads, no startup errors
- Open `docs/dashboard-ui-mockup.html` side-by-side for visual reference
- Create PR to `master`, squash-merge after review
- Delete `feat/dashboard-blazor-ui` branch after merge

---

## 14. Risk Register

| Risk | Mitigation |
|------|------------|
| MudBlazor 9.6.0 breaking changes from docs | Clone at `$env:TEMP\mudblazor` — check `src/MudBlazor.Docs` for component examples |
| Blazor-ApexCharts 6.1.0 net10.0 support | Verified via NuGet — targets `net10.0` explicitly |
| `TreatWarningsAsErrors` blocks build | Only suppress via `#pragma warning disable` in extreme cases (e.g., `CS1591` for XML docs) — document each suppression |
| Serilog.Sinks.File 6.0.0 vs 7.0.0 | Serilog.AspNetCore 10.0.0 pulls 7.0.0. Use 7.0.0 explicitly. |
| SDK csproj path changes | Use relative path `..\..\sdk\FreshlianceGateway.Sdk\FreshlianceGateway.Sdk.csproj` |
| Large datasets crash SignalR | `DashboardSnapshot` is a small record. Recent alarms capped at 10. No large payloads. |

---

## 15. Branching Strategy

**Convention:** `master` is protected trunk. Feature branches use `feat/<slug>`. This repo already has:
- `master` — trunk
- `feat/bizcontent-marker-interface` (merged into master)
- `feat/dashboard-blazor-ui` — **current branch** for all dashboard work

**Rules:**
| Rule | Detail |
|------|--------|
| Base branch | `master` |
| Feature branch | `feat/dashboard-blazor-ui` (no sub-branches — single branch for entire feature) |
| Commit style | Imperative mood, lowercase summary: `add device list page with filters` |
| Merge strategy | Squash-merge into `master` via PR after review |
| PR target | `master` |
| Branch lifetime | Delete after squash-merge |

**Rationale for single branch:** The dashboard is a self-contained Blazor Server project (~32 files). No parallel team members. No risk of merge conflicts. Sub-branches would add overhead with zero benefit.

**Git workflow:**
```
master ────────────────────────────────────────── ○ (squash merge)
           \
feat/dashboard-blazor-ui ── A ── B ── C ── D ── E ── F (PR → squash → master)
```

---

## 16. Test Strategy

### 16.1 Conventions (inherited from SDK)

| Aspect | Convention |
|--------|-----------|
| Framework | xUnit v3 |
| Assertions | FluentAssertions 8.2.0 |
| Mocking (unit) | NSubstitute 5.3.0 |
| HTTP stubbing (unit) | `FakeHttpMessageHandler` (existing in SDK tests) |
| HTTP stubbing (integration) | WireMock.Net 1.7.4 |
| Test project naming | `{Project}.Tests` / `{Project}.IntegrationTests` |
| TreatWarningsAsErrors | `true` with `CS1591` and framework-specific suppressions |

### 16.2 Unit Tests — `src/Freshliance.Dashboard.Tests/`

**Framework:** bUnit (Blazor component testing) + NSubstitute + FluentAssertions

**Target:** Isolated component logic. All SDK services substituted via DI.

**NuGet packages to add:**
| Package | Version | Purpose |
|---------|---------|---------|
| `bunit` | Latest stable | Blazor component rendering + interaction |
| `Microsoft.NET.Test.Sdk` | 17.13.0 | Test runner |
| `xunit.v3` | 2.0.0 | Test framework |
| `xunit.runner.visualstudio` | 3.1.0 | VS integration |
| `NSubstitute` | 5.3.0 | Mocking |
| `FluentAssertions` | 8.2.0 | Assertions |
| `NSubstitute.Analyzers.CSharp` | Latest | Compile-time NSubstitute checks |

**Files (~14 test files):**

| Test File | What It Tests | Key Assertions |
|-----------|--------------|----------------|
| `Pages/DashboardTests.cs` | Dashboard page renders summary cards, alarm table; SignalR subscription; auto-refresh toggle | Cards show correct counts; table has rows; toggle calls hub |
| `Pages/DevicesTests.cs` | Device grid with filters; pagination callback; row click navigation | ServerData triggers correct service call; filters reset page; click navigates |
| `Pages/DeviceDetailTests.cs` | Probe tabs render; chart data loaded; time range buttons; CSV export | 3 probe calls in parallel; chart series populated; export button triggers JS |
| `Pages/AlarmsTests.cs` | Alarm grid with property/type/status filters; row coloring for unhandled | Filters appended to request; processing rows have red background |
| `Pages/GroupsTests.cs` | Tree renders; group selection loads devices; bind/unbind dialogs | Tree items from service; allocated grid loads on click; bind sends correct IDs |
| `Pages/TemplatesTests.cs` | Template CRUD grid; expand alarm config; create dialog validation | Empty name prevented; alarm rows addable; delete requires confirm |
| `Pages/SettingsTests.cs` | Form pre-populated from user info; save sends update; validation | All fields mapped; Save button triggers UpdateAsync; nickname length check |
| `Shared/DeviceStatusChipTests.cs` | Chip renders correct color and label per status | Online→green, Offline→gray, Alarm→red, Inactive→outlined |
| `Shared/LastReadingCardTests.cs` | Displays temp/humidity/light/CO2/metrics/dataTime | Missing values show "—"; values formatted per temperature unit |
| `Shared/ProbeTabsTests.cs` | Tabs render per probe type; only renders tabs with data | 0 probes→empty; 3 probes→3 tabs; active tab has correct content |
| `Shared/TimeRangePickerTests.cs` | Presets emit correct epoch ranges; custom picker works | "1h" button→[now-1h, now]; date range→correct longs |
| `Services/PollingServiceTests.cs` | Timer fires; fetches data; broadcasts via hub | Timer interval respected; hub.SendAsync called; stops when disabled |
| `Services/DeviceStateServiceTests.cs` | Cache stores devices; lookup by recordId; invalidation | GetByRecordId returns correct device; Invalidate clears cache |
| `Services/UserPreferenceServiceTests.cs` | FormatDateTime converts epoch→local; FormatTemperature converts °C→°F | Timezone offset applied; 0°C→32°F; null→"—" |

**bUnit test pattern (example):**
```csharp
using var ctx = new Bunit.TestContext();
var deviceService = Substitute.For<IDeviceService>();
deviceService.GetPageAsync(Arg.Any<GetDevicePageRequest>(), Arg.Any<CancellationToken>())
    .Returns(new FreshlianceResponse<PageResult<DevicePageItemResponse>>
    {
        Code = "0",
        Data = new() { Total = 2, Rows = [ /* mock rows */ ] }
    });
ctx.Services.AddSingleton(deviceService);
ctx.Services.AddSingleton(Substitute.For<IDeviceDataService>());
ctx.Services.AddSingleton(Substitute.For<IGroupService>());
ctx.Services.AddSingleton(Substitute.For<IGroupDeviceService>());
ctx.Services.AddSingleton(Substitute.For<IConfigTemplateService>());
ctx.Services.AddSingleton(Substitute.For<IUserService>());
ctx.Services.AddSingleton(Substitute.For<NavigationManager>());
ctx.Services.AddSingleton(Substitute.For<IJSRuntime>());
ctx.Services.AddSingleton(Substitute.For<UserPreferenceService>());
ctx.Services.AddSingleton(Substitute.For<DeviceStateService>());

var cut = ctx.RenderComponent<Devices>();

cut.FindAll("table tbody tr").Count.Should().Be(2);
cut.Find(".mud-table-pagination").TextContent.Should().Contain("2");
```

**bUnit test counts:**
| Category | Files | Test methods (estimate) |
|----------|-------|------------------------|
| Page tests | 7 | ~35 |
| Shared component tests | 4 | ~20 |
| Service tests | 3 | ~15 |
| **Total** | **14** | **~70** |

### 16.3 Integration Tests — `src/Freshliance.Dashboard.IntegrationTests/`

**Framework:** WireMock.Net + real service instances

**Target:** Verify the Dashboard's service layer integrates correctly with the SDK and Freshliance API. No browser — these are headless C# tests.

**Files (~5 test files):**

| Test File | What It Tests | Setup |
|-----------|--------------|-------|
| `DashboardIntegrationTests.cs` | PollingService fetches devices+alarms, aggregates counts correctly | WireMock stubs device page + alarm page; assert `DashboardSnapshot` has correct counts |
| `DeviceDataPaginationTests.cs` | Multi-page data aggregation for charts works correctly | WireMock returns 3 pages of 50 each (total 150); assert all 150 rows merged |
| `UserPreferenceIntegrationTests.cs` | UserPreferenceService loads from API and formats correctly | WireMock stubs `gw.userInfo.get` with timezone="+08:00"; assert epoch→local conversion |
| `ErrorRecoveryTests.cs` | Pages handle API errors gracefully without crashing | WireMock returns `code: "40000"`; assert error message displayed, no exception |
| `SignalRIntegrationTests.cs` | DashboardHub receives and forwards snapshot to clients | Direct hub context mock; assert `SendAsync` called with correct group |

**Integration test uses WireMock exactly like SDK integration tests:**
```csharp
_fixture.ResetAndStubSuccess("""
    {
        "code": "0",
        "data": { "total": 47, "rows": [...] },
        "msg": "success"
    }
    """);
var client = _fixture.CreateClient();
var service = new DeviceService(client);
```

### 16.4 E2E Tests — `src/Freshliance.Dashboard.E2ETests/`

**Framework:** Playwright for .NET + xUnit v3

**Selector strategy:** All interactive and assertable elements carry `data-testid` attributes. Playwright selects via `Page.Locator("[data-testid='...']")`. This decouples tests from CSS class changes and MudBlazor internals.

**NuGet packages:**
| Package | Version | Purpose |
|---------|---------|---------|
| `Microsoft.Playwright` | Latest stable | Browser automation |
| `Microsoft.NET.Test.Sdk` | 17.13.0 | Test runner |
| `xunit.v3` | 2.0.0 | Test framework |
| `FluentAssertions` | 8.2.0 | Assertions |

**Files (~6 test files):**

| Test File | What It Tests | Browser Actions |
|-----------|--------------|----------------|
| `DashboardE2ETests.cs` | Dashboard loads, cards, alarm table, auto-refresh | `[data-testid="card-total"]`, `[data-testid="card-online"]`, `[data-testid="card-offline"]`, `[data-testid="card-alarm"]`, `[data-testid="alarm-table"]`, `[data-testid="auto-refresh-toggle"]` |
| `NavigationE2ETests.cs` | Sidebar navigation for all 7 pages | `[data-testid="nav-dashboard"]`, `[data-testid="nav-devices"]`, `[data-testid="nav-alarms"]`, `[data-testid="nav-groups"]`, `[data-testid="nav-templates"]`, `[data-testid="nav-settings"]`, `[data-testid="page-title"]` |
| `DeviceDetailChartE2ETests.cs` | Device list → detail, chart renders with data | `[data-testid="device-row"]` click → `[data-testid="probe-tab-builtin"]` → `[data-testid="chart-container"]` has SVG, `[data-testid="time-preset-1h"]`, `[data-testid="time-preset-6h"]`, `[data-testid="export-csv"]` |
| `GroupManagementE2ETests.cs` | Create group, add device, remove | `[data-testid="btn-add-group"]` → form `[data-testid="input-group-name"]` → `[data-testid="btn-submit"]` → `[data-testid="tree-group"]` visible → `[data-testid="btn-add-devices"]` → select checkboxes → `[data-testid="btn-bind"]` → `[data-testid="btn-remove-device"]` |
| `SettingsFormE2ETests.cs` | Load settings, edit, save | `[data-testid="input-nickname"]`, `[data-testid="select-timezone"]`, `[data-testid="select-language"]`, `[data-testid="btn-save-settings"]` → `[data-testid="snackbar-success"]` |
| `ResponsiveE2ETests.cs` | Layout adapts at breakpoints | `[data-testid="nav-drawer"]` visible/hidden per viewport; `[data-testid="hamburger-btn"]` visible on mobile only |

**Playwright test pattern with data-testid:**
```csharp
[Fact]
public async Task Dashboard_ShowsSummaryCards()
{
    await Page.GotoAsync(AppUrl);
    await Expect(Page.Locator("[data-testid='card-total']")).ToBeVisibleAsync();
    await Expect(Page.Locator("[data-testid='card-online']")).ToContainTextAsync("Online");
    await Expect(Page.Locator("[data-testid='card-offline']")).ToContainTextAsync("Offline");
    await Expect(Page.Locator("[data-testid='card-alarm']")).ToContainTextAsync("In Alarm");
}

[Fact]
public async Task DeviceDetail_ChartLoadsData()
{
    await Page.GotoAsync($"{AppUrl}/device/12922");
    await Page.Locator("[data-testid='probe-tab-builtin']").ClickAsync();
    await Expect(Page.Locator("[data-testid='chart-container'] svg")).ToBeVisibleAsync();
    await Expect(Page.Locator("[data-testid='reading-temp']")).NotToBeEmptyAsync();
    await Expect(Page.Locator("[data-testid='reading-humidity']")).NotToBeEmptyAsync();
}

[Fact]
public async Task Groups_CreateAndDelete()
{
    await Page.GotoAsync($"{AppUrl}/groups");
    await Page.Locator("[data-testid='btn-add-group']").ClickAsync();
    await Page.Locator("[data-testid='input-group-name']").FillAsync("E2E Test Group");
    await Page.Locator("[data-testid='btn-submit']").ClickAsync();
    await Expect(Page.Locator("[data-testid='tree-group']:has-text('E2E Test Group')")).ToBeVisibleAsync();
}
```

**E2E test counts:**
| File | Test methods (estimate) |
|------|------------------------|
| DashboardE2ETests | ~6 |
| NavigationE2ETests | ~8 (one per page + logo) |
| DeviceDetailChartE2ETests | ~5 |
| GroupManagementE2ETests | ~6 |
| SettingsFormE2ETests | ~4 |
| ResponsiveE2ETests | ~4 |
| **Total** | **~33** |

### 16.5 `data-testid` Attribute Catalog

Every component/page MUST add `data-testid` attributes. This is a **build requirement** — each `data-testid` below must be present in the rendered HTML.

#### MainLayout / NavMenu
| Element | `data-testid` |
|---------|--------------|
| `MudDrawer` | `nav-drawer` |
| Mobile hamburger button | `hamburger-btn` |
| Dashboard nav link | `nav-dashboard` |
| Devices nav link | `nav-devices` |
| Alarms nav link | `nav-alarms` |
| Groups nav link | `nav-groups` |
| Templates nav link | `nav-templates` |
| Settings nav link | `nav-settings` |
| Page title | `page-title` |
| User badge | `user-badge` |
| Last updated text | `last-updated` |

#### Dashboard.razor
| Element | `data-testid` |
|---------|--------------|
| Auto-refresh toggle | `auto-refresh-toggle` |
| Total devices card | `card-total` |
| Online count card | `card-online` |
| Offline count card | `card-offline` |
| In alarm card | `card-alarm` |
| Recent alarms table | `alarm-table` |
| Group tree container | `group-tree` |

#### Devices.razor
| Element | `data-testid` |
|---------|--------------|
| Search input | `search-devices` |
| Status filter select | `filter-status` |
| Product type filter | `filter-product-type` |
| Alarm status filter | `filter-alarm-status` |
| Refresh button | `btn-refresh-devices` |
| Device row (per row, with index) | `device-row-{index}` |
| Device row link | `device-link-{sn}` |
| Pagination row count | `pagination-info` |

#### DeviceDetail.razor
| Element | `data-testid` |
|---------|--------------|
| Breadcrumb | `breadcrumb` |
| Device header card | `device-header` |
| Status chip | `device-status-chip` |
| Battery bar | `battery-bar` |
| Probe tab — built-in | `probe-tab-builtin` |
| Probe tab — external 1 | `probe-tab-external1` |
| Probe tab — external 2 | `probe-tab-external2` |
| Probe tab — alarms | `probe-tab-alarms` |
| Temperature reading value | `reading-temp` |
| Humidity reading value | `reading-humidity` |
| Light reading value | `reading-light` |
| Co2 reading value | `reading-co2` |
| Chart container | `chart-container` |
| Time preset 1h | `time-preset-1h` |
| Time preset 6h | `time-preset-6h` |
| Time preset 24h | `time-preset-24h` |
| Time preset 7d | `time-preset-7d` |
| Date range picker | `date-range-picker` |
| CSV export button | `export-csv` |
| Alarm history table | `alarm-history-table` |

#### Alarms.razor
| Element | `data-testid` |
|---------|--------------|
| Alarm property filter | `filter-alarm-property` |
| Alarm type filter | `filter-alarm-type` |
| Handle status filter | `filter-handle-status` |
| Probe type filter | `filter-probe-type` |
| Alarm row (per row) | `alarm-row-{index}` |
| Alarm table | `alarms-grid` |

#### Groups.razor
| Element | `data-testid` |
|---------|--------------|
| Add group button | `btn-add-group` |
| Group tree container | `tree-group` |
| Rename button | `btn-rename-group` |
| Delete group button | `btn-delete-group` |
| Add devices button | `btn-add-devices` |
| Remove device button | `btn-remove-device` |
| Group name input (dialog) | `input-group-name` |
| Submit button (dialog) | `btn-submit` |
| Cancel button (dialog) | `btn-cancel` |
| Allocated devices grid | `allocated-devices-grid` |

#### Templates.razor
| Element | `data-testid` |
|---------|--------------|
| Create template button | `btn-create-template` |
| Template grid | `template-grid` |
| Edit template button (per row) | `btn-edit-template-{index}` |
| Delete template button (per row) | `btn-delete-template-{index}` |
| Template name input | `input-template-name` |
| Category select | `select-category` |
| Collect interval input | `input-collect-interval` |
| Add alarm row button | `btn-add-alarm-row` |
| Alarm config table | `alarm-config-table` |

#### Settings.razor
| Element | `data-testid` |
|---------|--------------|
| Email input (readonly) | `input-email` |
| Nickname input | `input-nickname` |
| Timezone select | `select-timezone` |
| Language select | `select-language` |
| Date format select | `select-date-format` |
| Temperature unit select | `select-temp-unit` |
| Save button | `btn-save-settings` |
| SMS balance alert | `alert-sms-balance` |
| Success snackbar | `snackbar-success` |
| Error snackbar | `snackbar-error` |

#### Shared Components
| Component | Element | `data-testid` |
|-----------|---------|--------------|
| `DeviceStatusChip` | Chip container | `status-chip` |
| `LastReadingCard` | Card container | `last-reading-card` |
| `ProbeTabs` | Tab container | `probe-tabs` |
| `TimeRangePicker` | Preset button group | `time-presets` |
| `ConfirmDialog` | Confirm button | `btn-confirm` |
| `ConfirmDialog` | Cancel button | `btn-cancel-confirm` |
| `LoadingSkeleton` | Skeleton container | `loading-skeleton` |
| `EmptyState` | Empty container | `empty-state` |

### 16.6 bUnit Tests with `data-testid`

bUnit tests also use `data-testid` for component selection (in addition to CSS selectors):

```csharp
// Prefer data-testid for stability
cut.Find("[data-testid='card-online']").TextContent.Should().Contain("32");
cut.Find("[data-testid='device-row-0']").Click();

// Or via FindAll + LINQ
var cards = cut.FindAll("[data-testid^='card-']");
cards.Count().Should().Be(4);

// Trigger events via testid
cut.Find("[data-testid='auto-refresh-toggle']").Change(false);
cut.Find("[data-testid='btn-refresh-devices']").Click();
```

### 16.7 Test Summary

| Level | Project | Framework | Count (files) | Count (tests) |
|-------|---------|-----------|---------------|---------------|
| Unit | `Freshliance.Dashboard.Tests` | bUnit + NSubstitute | 14 | ~70 |
| Integration | `Freshliance.Dashboard.IntegrationTests` | WireMock.Net | 5 | ~25 |
| E2E | `Freshliance.Dashboard.E2ETests` | Playwright | 6 | ~33 |
| **Total** | | | **25** | **~128** |

### 16.8 Test Execution Order (CI Pipeline)

```
1. dotnet build --no-restore -warnaserror          (entire solution)
2. dotnet test FreshlianceGateway.Sdk.Tests        (existing SDK unit tests)
3. dotnet test FreshlianceGateway.Sdk.IntegrationTests (existing SDK integration tests)
4. dotnet test Freshliance.Dashboard.Tests         (NEW: Dashboard bUnit unit tests)
5. dotnet test Freshliance.Dashboard.IntegrationTests (NEW: Dashboard integration tests)
6. playwright install --with-deps chromium         (Install Playwright browser)
7. dotnet test Freshliance.Dashboard.E2ETests      (NEW: E2E tests, requires running server)
```

---

## 17. CI/CD Updates

The existing `.github/workflows/ci.yml` must be updated to include the Dashboard project.

**Changes needed:**

```yaml
# Add to matrix or as separate job:
- name: Build Dashboard
  working-directory: src/Freshliance.Dashboard
  run: dotnet build --no-restore -warnaserror

- name: Run Dashboard unit tests
  working-directory: src/Freshliance.Dashboard.Tests
  run: dotnet test --no-build --verbosity normal

- name: Run Dashboard integration tests
  working-directory: src/Freshliance.Dashboard.IntegrationTests
  run: dotnet test --no-build --verbosity normal

- name: Install Playwright
  run: pwsh src/Freshliance.Dashboard.E2ETests/bin/Debug/net10.0/playwright.ps1 install --with-deps chromium

- name: Run Dashboard E2E tests
  working-directory: src/Freshliance.Dashboard.E2ETests
  run: dotnet test --no-build --verbosity normal
  env:
    DASHBOARD_URL: http://localhost:5000
```

**Or:** Split into a separate workflow file `.github/workflows/dashboard-ci.yml` that triggers on changes to `src/Freshliance.Dashboard/**` paths.

**Recommendation:** Update existing `ci.yml` to build the entire solution from root (add a root `Freshliance-API.sln` referencing all projects). This keeps one CI pipeline.

---

## 18. Solution File Strategy

Create root `Freshliance-API.sln` referencing:
```
sdk\FreshlianceGateway.Sdk\FreshlianceGateway.Sdk.csproj
sdk\FreshlianceGateway.Sdk.Tests\FreshlianceGateway.Sdk.Tests.csproj
sdk\FreshlianceGateway.Sdk.IntegrationTests\FreshlianceGateway.Sdk.IntegrationTests.csproj
sdk\FreshlianceGateway.Samples.Cli\FreshlianceGateway.Samples.Cli.csproj
src\Freshliance.Dashboard\Freshliance.Dashboard.csproj
src\Freshliance.Dashboard.Tests\Freshliance.Dashboard.Tests.csproj
src\Freshliance.Dashboard.IntegrationTests\Freshliance.Dashboard.IntegrationTests.csproj
src\Freshliance.Dashboard.E2ETests\Freshliance.Dashboard.E2ETests.csproj
```

CI then runs `dotnet build` and `dotnet test` from root. The existing `sdk/FreshlianceGateway.slnx` remains for SDK-only work but is not the CI entry point anymore.

---

## 19. Phase 5 Post-Implementation Audit — Gap Analysis

### 19.1 Critical (Blocks Functionality)

| # | Gap | File | Fix |
|---|-----|------|-----|
| **C1** | Templates page unreachable | `Templates.razor:1` | Missing `@page "/templates"`. Page 404s. Add the directive. |
| **C2** | Groups add/rename/bind never calls API | `Groups.razor.cs:111-206` | `OpenAddGroupDialog`, `OpenRenameDialog`, `OpenAddDevicesDialog` extract `result.Data` but discard it. Must call `IGroupService.CreateAsync`, `IGroupService.UpdateAsync`, `IGroupDeviceService.BindAsync` with the dialog result. |
| **C3** | Alarms `ReloadAsync` hangs on null grid | `Alarms.razor.cs:70-75` | Calls `SetLoading(true)` then delegates to `LoadGridData`. If `_grid` is null, `_isLoading` stays true permanently. Add null guard or try/finally. |

### 19.2 High (Missing Features / Dead Code)

| # | Gap | File | Fix |
|---|-----|------|-----|
| **H1** | DeviceDetail has no auto-refresh | `DeviceDetail.razor.cs` | Data loads once at init and on time-range change. Goes stale immediately. Add `System.Timers.Timer` that fires when user is on a "live" time preset (1h/6h/24h). Only refetch the latest page of each probe. Stop on Alarm History tab, 7d preset, or custom range. |
| **H2** | Dashboard group tree never updates | `Dashboard.razor.cs:158-166` | `OnDashboardUpdate` SignalR callback updates `_snapshot` (cards) only. `_groupTreeItems` is never refreshed. Refetch tree in the callback so group counts stay current. |
| **H3** | Dead `IHubContext<DashboardHub>` injection | `Dashboard.razor.cs:22` | Injected via `[Inject]` but never referenced. The page uses `HubConnection` (client-side) to subscribe. Remove the injection and `using Microsoft.AspNetCore.SignalR`. |
| **H4** | No Serilog logging on page service calls | All pages | Plan requires `Log.Information/Debug/Warning` on every SDK call. Only `PollingService` has logging. Add `ILogger<T>` injection and log calls at appropriate levels. |

### 19.3 Medium (Pattern Inconsistencies / Bugs)

| # | Gap | File | Fix |
|---|-----|------|-----|
| **M1** | Settings: no empty state | `Settings.razor:18` | If `_userInfo` is null after load (but no error), renders nothing. Add `EmptyState` with "No user data available." |
| **M2** | Dashboard timer at 1s for cosmetic text | `Dashboard.razor.cs:110` | Timer fires every 1s just to update "Xs ago" label. Reduce to 10s interval or merge into data-refresh timer. Replace `System.Timers.Timer` with `PeriodicTimer` for modern pattern. |
| **M3** | TimeRangePicker "Apply" button missing `data-testid` | `TimeRangePicker.razor` | The custom date range Apply button has no testid. Add `data-testid="btn-apply-range"`. |
| **M4** | TemplateDialog alarm row inputs missing `data-testid` | `TemplateDialog.razor` | Select inputs and numeric fields for alarm config rows have no testids. Add testids per the catalog. |
| **M5** | Groups error alert inconsistent pattern | `Groups.razor:8` | Uses `CloseIconClicked` dismissible alert vs fixed alert on all other pages. Normalize to fixed `MudAlert`. |
| **M6** | Groups right-panel error uses snackbar | `Groups.razor.cs:104-107` | `LoadAllocatedDevices` failure shows snackbar only. Should show inline `MudAlert`. |
| **M7** | Alarm handleStatus value scheme mismatch | `Alarms.razor.cs` | Dashboard uses `0=Unhandled, 1=InProgress, 2=Handled`. Alarms filter uses `1=Processing, 2=Processed, 3=Ignored`. API returns `1=Processing, 2=Processed, 3=Ignored` per the docs. Unify to API values. |
| **M8** | TemplateDialog `OnParametersSet` fragile guard | `TemplateDialog.razor:134-163` | Uses `_isEdit || ExistingDetail is null` as first-call-only guard. If Blazor re-renders, it won't re-init. Use `OnInitialized` instead. |

### 19.4 Low (Nice-to-Have)

| # | Gap | File | Fix |
|---|-----|------|-----|
| **L1** | ConfirmDialog files (razor.cs) missing | `ConfirmDialog.razor` | Shared component has no code-behind. `[CascadingParameter]` and `[Parameter]` are in `@code` block. Move to `.razor.cs` for consistency. |
| **L2** | `_Imports.razor` has dead SignalR Client import | `_Imports.razor:3` | `@using Microsoft.AspNetCore.SignalR.Client` — only Dashboard uses this. Remove after H3 fix. |
| **L3** | `csproj` has `SignalR.Client` NuGet unused post-removal | `Freshliance.Dashboard.csproj` | After removing `HubConnection` from Dashboard (H3), the `Microsoft.AspNetCore.SignalR.Client` package reference is dead. Remove it. |

---

## 20. Remaining Implementation Phases

### Phase 5.5 — Critical Bug Fixes (3 files, ~30 min)

| File | Change |
|------|--------|
| `Templates.razor` | Add `@page "/templates"` above `<PageTitle>` |
| `Groups.razor.cs` | In `OpenAddGroupDialog`: after dialog result, call `GroupService.CreateAsync(...)` then reload tree + snackbar. In `OpenRenameDialog`: call `GroupService.UpdateAsync(...)`. In `OpenAddDevicesDialog`: call `GroupDeviceService.BindAsync(...)`. |
| `Alarms.razor.cs` | Add `if (_grid is null) { _isLoading = false; return; }` guard at top of `LoadGridData`. Or wrap ReloadAsync in try/finally. |

### Phase 5.6 — Auto-Refresh + Live Updates (3 files, ~45 min)

| File | Change |
|------|--------|
| `DeviceDetail.razor.cs` | Add `_liveRefreshTimer` (`PeriodicTimer`). Start on init, stop on dispose. In tick handler: if on alarm tab or non-live time range, skip. Else refetch latest page per probe, merge into `_chartPoints`, call `StateHasChanged()`. Restart timer on time range change. |
| `Dashboard.razor.cs` | In `OnDashboardUpdate`: also refetch group tree via `GroupService.GetTreeAsync()`. Merge `_refreshTimer` (for data) with `_timer` (for "Xs ago" label) — single `PeriodicTimer` at 30s. Remove `IHubContext<DashboardHub>` injection. Remove `using Microsoft.AspNetCore.SignalR`. |
| `_Imports.razor` | Remove `@using Microsoft.AspNetCore.SignalR.Client` |
| `Freshliance.Dashboard.csproj` | Remove `Microsoft.AspNetCore.SignalR.Client` PackageReference |

### Phase 5.7 — Logging + Pattern Normalization (7 files, ~45 min)

| File | Change |
|------|--------|
| All page `.cs` files | Add `[Inject] private ILogger<PageName> Logger { get; set; } = null!;`. Add `Logger.LogInformation/Debug/Warning` for: initial load timing, SDK call results, errors caught, data counts. |
| `Dashboard.razor.cs` | Replace `System.Timers.Timer` with `PeriodicTimer` + fire-and-forget loop. |
| `Settings.razor` | Add `EmptyState` rendering when `_userInfo is null && _error is null`. |
| `Groups.razor` | Replace dismissible alert with fixed `MudAlert`. |
| `Groups.razor.cs` | Show inline `MudAlert` for right-panel load failure instead of snackbar. |
| `Alarms.razor.cs` | Unify handleStatus label scheme to API values (1=Processing, 2=Processed, 3=Ignored). |
| `TemplateDialog.razor` | Move init logic from `OnParametersSet` to `OnInitialized` with a `_initialized` flag. |

### Phase 5.8 — data-testid Gap Fills (2 files, ~15 min)

| File | Change |
|------|--------|
| `TimeRangePicker.razor` | Add `data-testid="btn-apply-range"` on Apply button. |
| `TemplateDialog.razor` | Add testids on alarm-row selects (`select-probe-type-{index}`, etc.) and numeric fields (`input-threshold-{index}`, etc.). |

### Phase 6 — Unit Tests (15 files, ~2h)

bUnit tests per Section 16.2 of this plan. 

### Phase 7 — Integration + E2E Tests (13 files, ~3h)

WireMock integration + Playwright E2E per Sections 16.3-16.4.

### Phase 8 — Verify & Polish (30 min)

- `dotnet build -warnaserror` → 0 warnings, 0 errors
- `dotnet test` → all ~128 tests green
- Manual smoke test: navigate all 7 pages, verify data loads, click through groups CRUD, test device detail charts
- PR to master, squash-merge, delete branch

---

## 21. Quick Win — Fix Order (Prioritized)

Run these sequentially. Each step is <15 minutes:

1. Templates `@page` directive (C1) — unblocks entire page
2. Groups API calls (C2) — unblocks CRUD functionality
3. Alarms null guard (C3) — prevents UI hang
4. Dashboard dead injection removal (H3) + SignalR.Client removal (L2, L3) — cleanup
5. DeviceDetail auto-refresh (H1) — live updates
6. Dashboard tree refresh (H2) — live group counts
7. Logging (H4) — observable system
8. Pattern normalizations (M1-M8) — consistency
9. data-testid fills (Phase 5.8)
10. Tests (Phase 6-7)
