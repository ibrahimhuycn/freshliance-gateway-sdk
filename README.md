# Freshliance Gateway .NET SDK

.NET client library for the [Freshliance Gateway Partner API](https://api.freshliance.com/en/gateway/Overview.html). Provides typed access to all 28 endpoints with automatic RSA2 signature handling.

## Installation

```bash
dotnet add package FreshlianceGateway.Sdk --version 1.0.0-preview.1
```

## Quick Start

```csharp
using FreshlianceGateway.Sdk;
using FreshlianceGateway.Sdk.Services;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddFreshlianceGateway(options =>
{
    options.AppId = "your-app-id";
    options.PrivateKeyPem = File.ReadAllText("private.pem");
    options.PublicKeyPem = File.ReadAllText("public.pem"); // optional: verify response signatures
    options.AcceptLanguage = "en-US";
});

var provider = services.BuildServiceProvider();

// Use any of the 7 services
var userService = provider.GetRequiredService<IUserService>();
var response = await userService.GetAsync();
response.EnsureSuccess();
Console.WriteLine($"Email: {response.Data!.Email}");
```

## Services

| Service | Endpoints | Description |
|---------|-----------|-------------|
| `IUserService` | 2 | Get/update user profile |
| `IDeviceService` | 4 | Device categories, list, records, sub-devices |
| `IDeviceDataService` | 2 | Device data page, alarm records |
| `IGroupService` | 5 | Device group tree, list, CRUD |
| `IGroupDeviceService` | 4 | Allocate/unallocate devices to groups |
| `IRemoteCommandService` | 6 | Remote config, shutdown, delete commands |
| `IConfigTemplateService` | 5 | Sensor configuration templates CRUD |

## Response Handling

```csharp
var response = await deviceService.GetPageAsync(new()
{
    PageNum = 1,
    PageSize = 20,
    DeviceStatus = (int)DeviceStatus.Online
});

response.EnsureSuccess(); // throws FreshlianceException on failure

Console.WriteLine($"Total devices: {response.Data!.Total}");
foreach (var item in response.Data.Rows)
    Console.WriteLine($"  {item.DeviceInfo.DeviceSn} - {item.DeviceInfo.DeviceName}");
```

## Authentication

The SDK uses **RSA2 (SHA256WithRSA)** signing. Every request is signed with your private key and the server verifies with your registered public key.

1. Get an `appId` from Freshliance
2. Generate an RSA key pair:
   ```bash
   openssl genrsa -out private.pem 2048
   openssl rsa -in private.pem -pubout -out public.pem
   ```
3. Register your public key with Freshliance
4. Configure the SDK with your `appId` and `private.pem`

## Enums

All API integer codes have corresponding enums for type safety:

```csharp
DeviceStatus deviceStatus = DeviceStatus.Online;
ProductType productType = ProductType.Coeus;
TemperatureUnit unit = TemperatureUnit.Celsius;
AlarmProperty alarmProp = AlarmProperty.Temperature;
// ...and 14 more
```

## Sample CLI

```bash
cd sdk/FreshlianceGateway.Samples.Cli
dotnet run -- --mock          # show what each endpoint would send
dotnet run -- --app-id YOUR_ID --key-file private.pem  # real API call
```

## API Documentation

The Partner API docs have been crawled and saved as markdown in `gateway-api/`.

- [01 - Overview](gateway-api/01-Overview.md)
- [02 - Parameters](gateway-api/02-Parameter.md)
- [03 - Signature](gateway-api/03-Signature.md)
- [04 - Rules](gateway-api/04-Rules.md)
- [05 - Error Codes](gateway-api/05-ErrorCode.md)
- [06-12 - API Endpoints](gateway-api/)

## Requirements

- .NET 10.0+

## License

GNU General Public License v3.0
