# Freshliance Gateway API — .NET 10 SDK Design Plan

## 1. Solution Structure

```
FreshlianceGateway.Sdk/
├── FreshlianceGateway.Sdk.sln
├── src/
│   └── FreshlianceGateway.Sdk/
│       ├── FreshlianceGateway.Sdk.csproj
│       ├── FreshlianceClient.cs
│       ├── FreshlianceOptions.cs
│       ├── IServiceCollectionExtensions.cs
│       ├── Core/
│       │   ├── FreshlianceResponse.cs
│       │   ├── FreshlianceException.cs
│       │   ├── ISignatureProvider.cs
│       │   └── Rsa2SignatureProvider.cs
│       ├── Models/
│       │   ├── Common/
│       │   │   ├── PageResult.cs
│       │   │   └── Enums.cs
│       │   ├── User/
│       │   │   ├── GetUserInfoRequest.cs
│       │   │   ├── GetUserInfoResponse.cs
│       │   │   └── UpdateUserInfoRequest.cs
│       │   ├── Device/
│       │   │   ├── GetCategoryRequest.cs
│       │   │   ├── CategoryResponse.cs
│       │   │   ├── GetDevicePageRequest.cs
│       │   │   ├── DevicePageResponse.cs
│       │   │   ├── GetRecordPageRequest.cs
│       │   │   ├── RecordPageResponse.cs
│       │   │   ├── GetSubDevicePageRequest.cs
│       │   │   └── SubDevicePageResponse.cs
│       │   ├── Data/
│       │   │   ├── GetDeviceDataRequest.cs
│       │   │   ├── DeviceDataResponse.cs
│       │   │   ├── GetAlarmDataRequest.cs
│       │   │   └── AlarmDataResponse.cs
│       │   ├── Group/
│       │   │   ├── GroupTreeResponse.cs
│       │   │   ├── GetGroupListRequest.cs
│       │   │   ├── GroupListResponse.cs
│       │   │   ├── CreateGroupRequest.cs
│       │   │   ├── UpdateGroupRequest.cs
│       │   │   ├── DeleteGroupRequest.cs
│       │   │   ├── GetAllocatedDeviceRequest.cs
│       │   │   ├── AllocatedDeviceResponse.cs
│       │   │   ├── GetUnallocatedDeviceRequest.cs
│       │   │   ├── UnallocatedDeviceResponse.cs
│       │   │   ├── BindDeviceRequest.cs
│       │   │   └── UnbindDeviceRequest.cs
│       │   ├── Command/
│       │   │   ├── UpdateParameterRequest.cs
│       │   │   ├── ShutdownRequest.cs
│       │   │   ├── SaveDataConfigRequest.cs
│       │   │   ├── DirectConfigRequest.cs
│       │   │   └── DeleteCommandRequest.cs
│       │   └── Template/
│       │       ├── GetTemplatePageRequest.cs
│       │       ├── TemplatePageResponse.cs
│       │       ├── GetTemplateDetailRequest.cs
│       │       ├── TemplateDetailResponse.cs
│       │       ├── CreateTemplateRequest.cs
│       │       ├── UpdateTemplateRequest.cs
│       │       └── DeleteTemplateRequest.cs
│       └── Services/
│           ├── IUserService.cs
│           ├── IDeviceService.cs
│           ├── IDeviceDataService.cs
│           ├── IGroupService.cs
│           ├── IGroupDeviceService.cs
│           ├── IRemoteCommandService.cs
│           ├── IConfigTemplateService.cs
│           └── (implementations in same folder or an Impl/ folder)
```

---

## 2. Core Abstractions

### 2.1 `FreshlianceOptions`

```csharp
public class FreshlianceOptions
{
    public required string AppId { get; set; }
    public required string PrivateKeyPem { get; set; }    // RSA private key in PEM format
    public string? FreshliancePublicKeyPem { get; set; }  // optional: verify response signatures
    public string BaseUrl { get; set; } = "https://api.freshliance.com/api";
    public string Format { get; set; } = "JSON";
    public string Charset { get; set; } = "UTF-8";
    public string SignType { get; set; } = "RSA2";
    public string Version { get; set; } = "1.0";
    public string? AcceptLanguage { get; set; }           // e.g. "en-US"
    public int TimeoutSeconds { get; set; } = 30;
}
```

### 2.2 `FreshlianceResponse<T>`

```csharp
public class FreshlianceResponse<T>
{
    public string Code { get; set; } = "";
    public string Msg { get; set; } = "";
    public string? SubCode { get; set; }
    public string? SubMsg { get; set; }
    public string Sign { get; set; } = "";
    public T? Data { get; set; }

    public bool IsSuccess => Code == "0";
    public void EnsureSuccess()
    {
        if (!IsSuccess)
            throw new FreshlianceException(Code, SubCode, Msg, SubMsg);
    }
}
```

### 2.3 `PageResult<T>`

```csharp
public class PageResult<T>
{
    public int Total { get; set; }
    public List<T> Rows { get; set; } = [];
}
```

### 2.4 `FreshlianceException`

```csharp
public class FreshlianceException : Exception
{
    public string Code { get; }
    public string? SubCode { get; }
    public string? SubMsg { get; }

    public FreshlianceException(string code, string? subCode, string? msg, string? subMsg)
        : base($"[{code}] {msg} — {subCode}: {subMsg}") { … }
}
```

---

## 3. Signature Provider

### 3.1 `ISignatureProvider`

```csharp
internal interface ISignatureProvider
{
    string Sign(Dictionary<string, string> parameters);
    bool Verify(Dictionary<string, string> parameters, string signature);
}
```

### 3.2 `Rsa2SignatureProvider` algorithm

1. Filter: drop `sign`, drop null/empty values
2. Sort keys by ASCII ascending (ordinal)
3. Concatenate as `key1=value1&key2=value2&…`
4. Sign with `SHA256WithRSA` using private key → Base64
5. Verify: same steps 1–3, then verify against public key

---

## 4. `FreshlianceClient` (HTTP + signing engine)

```csharp
public class FreshlianceClient
{
    private readonly HttpClient _http;
    private readonly FreshlianceOptions _options;
    private readonly ISignatureProvider _signer;
    private readonly JsonSerializerOptions _json;

    public FreshlianceClient(HttpClient http, IOptions<FreshlianceOptions> options, ISignatureProvider signer);

    public async Task<FreshlianceResponse<T>> PostAsync<T>(string method, object? bizContent);
    // Builds parameters dict → signs → serializes bizContent → POSTs → deserializes response
}
```

---

## 5. Service Interfaces

### 5.1 `IUserService`

| Method | API Method | Description |
|--------|------------|-------------|
| `GetAsync()` | `gw.userInfo.get` | Obtain user info |
| `UpdateAsync(req)` | `gw.userInfo.update` | Modify user info |

### 5.2 `IDeviceService`

| Method | API Method | Description |
|--------|------------|-------------|
| `GetCategoriesAsync()` | `gw.device.category` | Device product categories |
| `GetPageAsync(req)` | `gw.deviceInfo.page` | Device list page |
| `GetRecordPageAsync(req)` | `gw.deviceInfo.recordPage` | Usage record page |
| `GetSubDevicePageAsync(req)` | `gw.deviceInfo.subPage` | Gateway sub-devices page |

### 5.3 `IDeviceDataService`

| Method | API Method | Description |
|--------|------------|-------------|
| `GetDataPageAsync(req)` | `gw.deviceData.page` | Device data page |
| `GetAlarmPageAsync(req)` | `gw.deviceAlarmData.page` | Alarm record page |

### 5.4 `IGroupService`

| Method | API Method | Description |
|--------|------------|-------------|
| `GetTreeAsync()` | `gw.deviceGroup.treeList` | Group tree |
| `GetListAsync(parentId, keyword)` | `gw.deviceGroup.list` | Sub-groups by parent |
| `CreateAsync(parentId, name)` | `gw.deviceGroup.create` | Create group |
| `UpdateAsync(groupId, name)` | `gw.deviceGroup.update` | Rename group |
| `DeleteAsync(groupId)` | `gw.deviceGroup.delete` | Delete group |

### 5.5 `IGroupDeviceService`

| Method | API Method | Description |
|--------|------------|-------------|
| `GetAllocatedPageAsync(req)` | `gw.groupDevice.pageAllocatedDevice` | Grouped devices |
| `GetUnallocatedPageAsync(req)` | `gw.groupDevice.pageUnAllocatedDevice` | Addable devices |
| `BindAsync(userDeviceIds, groupId)` | `gw.groupDevice.bindDevice` | Add to group |
| `UnbindAsync(userDeviceId, groupId)` | `gw.groupDevice.unbindDevice` | Remove from group |

### 5.6 `IRemoteCommandService`

| Method | API Method | Description |
|--------|------------|-------------|
| `UpdateParameterAsync(req)` | `gw.deviceCmd.updateParameter` | Buzzer, temp unit |
| `SaveDataShutdownAsync(recordId)` | `gw.deviceCmd.saveDataShutdown` | Graceful shutdown |
| `DirectShutdownAsync(recordId)` | `gw.deviceCmd.directShutdown` | Force shutdown |
| `SaveDataConfigAsync(req)` | `gw.deviceCmd.saveDataConfig` | Graceful reconfig |
| `DirectConfigAsync(req)` | `gw.deviceCmd.directConfig` | Immediate reconfig |
| `DeleteAsync(commandId)` | `gw.deviceCmd.delete` | Cancel pending command |

### 5.7 `IConfigTemplateService`

| Method | API Method | Description |
|--------|------------|-------------|
| `GetPageAsync(req)` | `gw.configTemplate.page` | Template list |
| `GetAsync(configId)` | `gw.configTemplate.get` | Template details |
| `CreateAsync(req)` | `gw.configTemplate.create` | Create template |
| `UpdateAsync(req)` | `gw.configTemplate.update` | Update template |
| `DeleteAsync(configId)` | `gw.configTemplate.delete` | Delete template |

---

## 6. Enums

```csharp
public enum DeviceStatus   { Inactive = 0, Online = 1, Offline = 2, Abnormal = 3 }
public enum AlarmStatus    { Normal = 1, Alarm = 2 }
public enum ProductType    { Gateway = 1, Sensor = 2, Coeus = 3, Gsp = 4 }
public enum Language       { English = 1, Chinese = 2, French = 3, German = 4, Russian = 5, Spanish = 6 }
public enum DateFormat     { YyyyMmDd = 1, DdMmYyyy = 2, MmDdYyyy = 3 }
public enum TemperatureUnit { Celsius = 1, Fahrenheit = 2 }
public enum ProbeType      { BuiltIn = 0, External1 = 1, External2 = 2 }
public enum ProbeProperty  { Invalid = 0, Temperature = 1, TemperatureHumidity = 2, Humidity = 3, Illumination = 4, Co2 = 5 }
public enum AlarmProperty  { Temperature = 1, Humidity = 2, Illumination = 3, Co2 = 4 }
public enum AlarmType      { Low = 1, High = 2 }
public enum AlarmWay       { Single = 1, Cumulative = 2 }
public enum RecordStatus   { InUse = 1, Stopped = 2 }
public enum BuzzerStatus   { On = 1, Off = 2 }
public enum PowerStatus    { Normal = 0, LowBattery = 1 }
public enum HandleStatus   { Processing = 1, Processed = 2, Ignored = 3 }
public enum ProbeDataStatus { Normal = 0, NotConnected = 1, Mismatched = 2 }
public enum NotifyFlag     { Available = 1, NotAvailable = 2 }
```

---

## 7. DI Registration

```csharp
// In Program.cs / Startup
builder.Services.AddFreshlianceGateway(options =>
{
    options.AppId = builder.Configuration["Freshliance:AppId"]!;
    options.PrivateKeyPem = builder.Configuration["Freshliance:PrivateKey"]!;
    options.AcceptLanguage = "en-US";
});
```

Extension method registers:
- `IOptions<FreshlianceOptions>` from config
- `ISignatureProvider` as singleton `Rsa2SignatureProvider`
- `FreshlianceClient` as scoped
- All 7 service implementations as scoped

---

## 8. Usage Example

```csharp
var user = await userService.GetAsync();
Console.WriteLine($"Email: {user.Email}, Zone: {user.TimeZone}");

var devices = await deviceService.GetPageAsync(new()
{
    PageNum = 1,
    PageSize = 20,
    DeviceStatus = DeviceStatus.Online
});
Console.WriteLine($"Found {devices.Total} devices");

var groups = await groupService.GetTreeAsync();
foreach (var g in groups)
    Console.WriteLine($"  Group: {g.GroupName} ({g.DeviceGroupCount.DeviceCount} devices)");

var template = await templateService.CreateAsync(new()
{
    SensorConfig = new() { TemplateName = "Warehouse A", CollectInterval = 5 },
    CategoryId = 9,
    ProductCode = "97",
    SensorAlarmList = [
        new() { AlarmZone = "L", AlarmProperty = AlarmProperty.Temperature, AlarmType = AlarmType.Low,
                ProbeType = ProbeType.BuiltIn, AlarmThreshold = 2.0 }
    ]
});
```

---

## 9. Key Design Decisions

| Decision | Rationale |
|----------|-----------|
| **`FreshlianceClient` as internal engine, services as public surface** | Users interact with typed services, not raw `PostAsync<T>` |
| **`bizContent` serialized as JSON string before signing** | API requires bizContent as a string; the JSON must be compact (no spaces) to match the signing string used by the server |
| **No response signature verification by default** | Optional feature gated by `FreshliancePublicKeyPem` in options |
| **All response models use `FreshlianceResponse<T>` wrapper** | Consistent error handling; `EnsureSuccess()` for throw-on-failure |
| **`DateTimeOffset` for timestamps, but serialize as long (ms epoch)** | API uses millisecond timestamps; custom `JsonConverter` handles this |
| **`IHttpClientFactory` via `AddHttpClient<T>`** | Respects .NET best practices for socket exhaustion |
| **`System.Text.Json` with `JsonSerializerContext` source-gen** | Native AOT-ready, trim-safe, fast |
| **No `Newtonsoft.Json` dependency** | .NET 10 prefers STJ |

---

## 10. Request Signing Detail

For `gw.deviceInfo.page` with `bizContent = { "pageNum": 1, "pageSize": 20 }`:

```
Step 1: Build dictionary
  { appId, bizContent="{\"pageNum\":1,\"pageSize\":20}", charset="UTF-8",
    format="JSON", method="gw.deviceInfo.page", signType="RSA2",
    timestamp="1755662900000", version="1.0" }

Step 2: Filter (drop sign, empty) → no change

Step 3: Sort keys by ASCII ordinal
  appId → bizContent → charset → format → method → signType → timestamp → version

Step 4: Concatenate
  appId=xxx&bizContent={"pageNum":1,"pageSize":20}&charset=UTF-8&format=JSON
  &method=gw.deviceInfo.page&signType=RSA2&timestamp=1755662900000&version=1.0

Step 5: SHA256WithRSA(privateKey) → Base64 → sign param

Step 6: POST JSON body = { appId, bizContent, charset, format, method, signType,
                          timestamp, version, sign }
```

---

## 11. Implementation Priority

| Phase | Scope |
|-------|-------|
| 1 | `FreshlianceOptions`, `FreshlianceResponse<T>`, `FreshlianceException`, `Rsa2SignatureProvider`, `FreshlianceClient`, enums, DI extension |
| 2 | `IUserService` + impl, `IDeviceService` + impl |
| 3 | `IDeviceDataService`, `IGroupService`, `IGroupDeviceService` |
| 4 | `IRemoteCommandService`, `IConfigTemplateService` |
| 5 | Response signature verification, unit tests, XML docs |

---

## 12. Tests Strategy

- **Unit tests**: `Rsa2SignatureProvider` (known-answer test from docs example), request serialization
- **Integration tests**: Against a sandbox appId; all 28 endpoints
- Use `WireMock.Net` or `Microsoft.AspNetCore.TestHost` for HTTP mocking
