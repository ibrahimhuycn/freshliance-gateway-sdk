using System.Text.Json;
using FreshlianceGateway.Sdk;
using FreshlianceGateway.Sdk.Core;
using FreshlianceGateway.Sdk.Models;
using FreshlianceGateway.Sdk.Models.Data;
using FreshlianceGateway.Sdk.Models.Group;
using FreshlianceGateway.Sdk.Models.User;
using FreshlianceGateway.Sdk.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FreshlianceGateway.Samples.Cli;

internal static class Program
{
    private static async Task<int> Main(string[] args)
    {
        bool mock = false;
        bool groupSpaceTest = false;
        bool alarmProbe = false;
        string? appId = null;
        string? keyFile = null;

        for (int i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "--mock":
                    mock = true;
                    break;
                case "--group-space-test":
                    groupSpaceTest = true;
                    break;
                case "--alarm-probe":
                    alarmProbe = true;
                    break;
                case "--app-id":
                    if (i + 1 < args.Length) appId = args[++i];
                    break;
                case "--key-file":
                    if (i + 1 < args.Length) keyFile = args[++i];
                    break;
                case "--help":
                case "-h":
                    PrintHelp();
                    return 0;
            }
        }

        Console.WriteLine("Freshliance Gateway SDK - Sample CLI");
        Console.WriteLine("=====================================");
        Console.WriteLine();

        if (mock)
        {
            RunMockDemo();
            return 0;
        }

        if (appId is not null && keyFile is not null)
        {
            if (groupSpaceTest)
                await RunGroupSpaceTestAsync(appId, keyFile);
            else if (alarmProbe)
                await RunAlarmProbeAsync(appId, keyFile);
            else
                await RunRealDemoAsync(appId, keyFile);
            return 0;
        }

        PrintUsage();
        return 0;
    }

    private static void PrintHelp()
    {
        Console.WriteLine("Freshliance Gateway SDK - Sample CLI");
        Console.WriteLine();
        Console.WriteLine("Usage:");
        Console.WriteLine("  --mock              Run in mock mode (demonstrates API surface)");
        Console.WriteLine("  --app-id <id>       Freshliance application ID");
        Console.WriteLine("  --key-file <path>   Path to RSA private key PEM file");
        Console.WriteLine("  --group-space-test  Create a group with spaces and read it back (real API)");
        Console.WriteLine("  --alarm-probe       Probe which alarm-page method the API accepts (real API)");
        Console.WriteLine("  --help, -h          Show this help");
        Console.WriteLine();
        Console.WriteLine("Examples:");
        Console.WriteLine("  FreshlianceGateway.Samples.Cli --mock");
        Console.WriteLine("  FreshlianceGateway.Samples.Cli --app-id myAppId --key-file ./private.pem");
    }

    private static void PrintUsage()
    {
        Console.WriteLine("No valid options provided.");
        Console.WriteLine();
        Console.WriteLine("Usage:");
        Console.WriteLine("  --mock              Run in mock mode (demonstrates API surface)");
        Console.WriteLine("  --app-id <id>       Freshliance application ID");
        Console.WriteLine("  --key-file <path>   Path to RSA private key PEM file");
        Console.WriteLine("  --help              Show help");
        Console.WriteLine();
        Console.WriteLine("Example: FreshlianceGateway.Samples.Cli --mock");
    }

    private static void RunMockDemo()
    {
        Console.WriteLine("[MOCK MODE] Demonstrating SDK API surface");
        Console.WriteLine();

        DemoFreshlianceOptions();
        DemoServiceCollectionExtensions();
        DemoUserService();
        DemoDeviceService();
        DemoDeviceDataService();
        DemoGroupService();
        DemoGroupDeviceService();
        DemoRemoteCommandService();
        DemoConfigTemplateService();

        Console.WriteLine("=====================================");
        Console.WriteLine("Parse Sample Response");
        Console.WriteLine("=====================================");
        const string json = """{"code":"0","msg":"success","data":{"email":"demo@freshliance.com","nickName":"Demo User","language":1}}""";
        var response = JsonSerializer.Deserialize<FreshlianceResponse<UserInfoResponse>>(json);
        Console.WriteLine($"  Code     = {response?.Code}");
        Console.WriteLine($"  Msg      = {response?.Msg}");
        Console.WriteLine($"  IsSuccess = {response?.IsSuccess}");
        Console.WriteLine($"  Data.Email     = {response?.Data?.Email}");
        Console.WriteLine($"  Data.Nickname  = {response?.Data?.Nickname}");
        Console.WriteLine($"  Data.Language  = {response?.Data?.Language}");
        Console.WriteLine();
        Console.WriteLine("Done. Use real credentials for actual API calls.");
    }

    private static void DemoFreshlianceOptions()
    {
        Console.WriteLine("=====================================");
        Console.WriteLine("FreshlianceOptions");
        Console.WriteLine("=====================================");
        var options = new FreshlianceOptions
        {
            AppId = "demo-app-id",
            PrivateKeyPem = "-----BEGIN RSA PRIVATE KEY-----\n...mock...\n-----END RSA PRIVATE KEY-----",
            AcceptLanguage = "en-US"
        };
        Console.WriteLine($"  AppId          = {options.AppId}");
        Console.WriteLine($"  BaseUrl        = {options.BaseUrl}");
        Console.WriteLine($"  Format         = {options.Format}");
        Console.WriteLine($"  Charset        = {options.Charset}");
        Console.WriteLine($"  SignType       = {options.SignType}");
        Console.WriteLine($"  Version        = {options.Version}");
        Console.WriteLine($"  TimeoutSeconds = {options.TimeoutSeconds}");
        Console.WriteLine($"  AcceptLanguage = {options.AcceptLanguage}");
        Console.WriteLine();
    }

    private static void DemoServiceCollectionExtensions()
    {
        Console.WriteLine("=====================================");
        Console.WriteLine("ServiceCollectionExtensions (DI Registration)");
        Console.WriteLine("=====================================");
        Console.WriteLine("  services.AddFreshlianceGateway(options => {");
        Console.WriteLine("      options.AppId = \"demo-app-id\";");
        Console.WriteLine("      options.PrivateKeyPem = \"...\";");
        Console.WriteLine("  });");
        Console.WriteLine();
        Console.WriteLine("  Registered components:");
        Console.WriteLine("    Singleton: ISignatureProvider -> Rsa2SignatureProvider");
        Console.WriteLine("    Scoped:    FreshlianceClient (HTTP client)");
        Console.WriteLine("    Scoped:    IUserService -> UserService");
        Console.WriteLine("    Scoped:    IDeviceService -> DeviceService");
        Console.WriteLine("    Scoped:    IDeviceDataService -> DeviceDataService");
        Console.WriteLine("    Scoped:    IGroupService -> GroupService");
        Console.WriteLine("    Scoped:    IGroupDeviceService -> GroupDeviceService");
        Console.WriteLine("    Scoped:    IRemoteCommandService -> RemoteCommandService");
        Console.WriteLine("    Scoped:    IConfigTemplateService -> ConfigTemplateService");
        Console.WriteLine();
    }

    private static void DemoUserService()
    {
        Console.WriteLine("=====================================");
        Console.WriteLine("IUserService");
        Console.WriteLine("=====================================");
        Console.WriteLine("  GetAsync()");
        Console.WriteLine("    -> POST gw.userInfo.get (no bizContent)");
        Console.WriteLine("    -> FreshlianceResponse<UserInfoResponse>");
        Console.WriteLine();
        Console.WriteLine("  UpdateAsync(request)");
        Console.WriteLine("    -> POST gw.userInfo.update");
        Console.WriteLine("    -> bizContent: {nickname:\"John\", timeZone:\"America/New_York\", temperatureUnit:1}");
        Console.WriteLine("    -> FreshlianceResponse<bool>");
        Console.WriteLine();
    }

    private static void DemoDeviceService()
    {
        Console.WriteLine("=====================================");
        Console.WriteLine("IDeviceService");
        Console.WriteLine("=====================================");
        Console.WriteLine("  GetCategoriesAsync()");
        Console.WriteLine("    -> POST gw.device.categories.get (no bizContent)");
        Console.WriteLine("    -> FreshlianceResponse<List<CategoryResponse>>");
        Console.WriteLine();
        Console.WriteLine("  GetPageAsync(request)");
        Console.WriteLine("    -> POST gw.device.page.get");
        Console.WriteLine("    -> bizContent: {pageNum:1, pageSize:20, deviceStatus:1}");
        Console.WriteLine("    -> FreshlianceResponse<PageResult<DevicePageItemResponse>>");
        Console.WriteLine();
        Console.WriteLine("  GetRecordPageAsync(request)");
        Console.WriteLine("    -> POST gw.device.record.page.get");
        Console.WriteLine("    -> bizContent: {pageNum:1, pageSize:20, deviceSn:\"SN12345\"}");
        Console.WriteLine("    -> FreshlianceResponse<PageResult<RecordPageItemResponse>>");
        Console.WriteLine();
        Console.WriteLine("  GetSubDevicePageAsync(request)");
        Console.WriteLine("    -> POST gw.subDevice.page.get");
        Console.WriteLine("    -> bizContent: {userDeviceId:42, pageNum:1, pageSize:20}");
        Console.WriteLine("    -> FreshlianceResponse<PageResult<SubDevicePageItemResponse>>");
        Console.WriteLine();
    }

    private static void DemoDeviceDataService()
    {
        Console.WriteLine("=====================================");
        Console.WriteLine("IDeviceDataService");
        Console.WriteLine("=====================================");
        Console.WriteLine("  GetDataPageAsync(request)");
        Console.WriteLine("    -> POST gw.deviceData.page.get");
        Console.WriteLine("    -> bizContent: {recordId:100, probeType:1, pageNum:1, pageSize:10}");
        Console.WriteLine("    -> FreshlianceResponse<PageResult<DeviceDataResponse>>");
        Console.WriteLine();
        Console.WriteLine("  GetAlarmPageAsync(request)");
        Console.WriteLine("    -> POST gw.alarmData.page.get");
        Console.WriteLine("    -> bizContent: {recordId:100, alarmProperty:1, pageNum:1, pageSize:10}");
        Console.WriteLine("    -> FreshlianceResponse<PageResult<AlarmDataResponse>>");
        Console.WriteLine();
    }

    private static void DemoGroupService()
    {
        Console.WriteLine("=====================================");
        Console.WriteLine("IGroupService");
        Console.WriteLine("=====================================");
        Console.WriteLine("  GetTreeAsync()");
        Console.WriteLine("    -> POST gw.group.tree.get (no bizContent)");
        Console.WriteLine("    -> FreshlianceResponse<List<GroupTreeNodeResponse>>");
        Console.WriteLine();
        Console.WriteLine("  GetListAsync(request)");
        Console.WriteLine("    -> POST gw.group.list.get");
        Console.WriteLine("    -> bizContent: {parentId:0}");
        Console.WriteLine("    -> FreshlianceResponse<List<GroupListItemResponse>>");
        Console.WriteLine();
        Console.WriteLine("  CreateAsync(request)");
        Console.WriteLine("    -> POST gw.group.create");
        Console.WriteLine("    -> bizContent: {parentId:0, groupName:\"Warehouse A\"}");
        Console.WriteLine("    -> FreshlianceResponse<bool>");
        Console.WriteLine();
        Console.WriteLine("  UpdateAsync(request)");
        Console.WriteLine("    -> POST gw.group.update");
        Console.WriteLine("    -> bizContent: {groupId:5, groupName:\"Warehouse B\"}");
        Console.WriteLine("    -> FreshlianceResponse<bool>");
        Console.WriteLine();
        Console.WriteLine("  DeleteAsync(request)");
        Console.WriteLine("    -> POST gw.group.delete");
        Console.WriteLine("    -> bizContent: {groupId:5}");
        Console.WriteLine("    -> FreshlianceResponse<bool>");
        Console.WriteLine();
    }

    private static void DemoGroupDeviceService()
    {
        Console.WriteLine("=====================================");
        Console.WriteLine("IGroupDeviceService");
        Console.WriteLine("=====================================");
        Console.WriteLine("  GetUnallocatedPageAsync(request)");
        Console.WriteLine("    -> POST gw.groupDevice.unallocated.page.get");
        Console.WriteLine("    -> bizContent: {pageNum:1, pageSize:10, groupId:1}");
        Console.WriteLine("    -> FreshlianceResponse<PageResult<UnallocatedDeviceResponse>>");
        Console.WriteLine();
        Console.WriteLine("  GetAllocatedPageAsync(request)");
        Console.WriteLine("    -> POST gw.groupDevice.allocated.page.get");
        Console.WriteLine("    -> bizContent: {pageNum:1, pageSize:10, groupId:1}");
        Console.WriteLine("    -> FreshlianceResponse<PageResult<AllocatedDeviceResponse>>");
        Console.WriteLine();
        Console.WriteLine("  BindAsync(request)");
        Console.WriteLine("    -> POST gw.groupDevice.bind");
        Console.WriteLine("    -> bizContent: {userDeviceIds:[101,102], groupId:1}");
        Console.WriteLine("    -> FreshlianceResponse<bool>");
        Console.WriteLine();
        Console.WriteLine("  UnbindAsync(request)");
        Console.WriteLine("    -> POST gw.groupDevice.unbind");
        Console.WriteLine("    -> bizContent: {userDeviceId:101, groupId:1}");
        Console.WriteLine("    -> FreshlianceResponse<bool>");
        Console.WriteLine();
    }

    private static void DemoRemoteCommandService()
    {
        Console.WriteLine("=====================================");
        Console.WriteLine("IRemoteCommandService");
        Console.WriteLine("=====================================");
        Console.WriteLine("  UpdateParameterAsync(request)");
        Console.WriteLine("    -> POST gw.command.updateParameter");
        Console.WriteLine("    -> bizContent: {recordId:100, buzzerStatus:1, temperatureUnit:0}");
        Console.WriteLine("    -> FreshlianceResponse<bool>");
        Console.WriteLine();
        Console.WriteLine("  SaveDataShutdownAsync(request)");
        Console.WriteLine("    -> POST gw.command.saveDataShutdown");
        Console.WriteLine("    -> bizContent: {recordId:100}");
        Console.WriteLine("    -> FreshlianceResponse<bool>");
        Console.WriteLine();
        Console.WriteLine("  DirectShutdownAsync(request)");
        Console.WriteLine("    -> POST gw.command.directShutdown");
        Console.WriteLine("    -> bizContent: {recordId:100}");
        Console.WriteLine("    -> FreshlianceResponse<bool>");
        Console.WriteLine();
        Console.WriteLine("  SaveDataConfigAsync(request)");
        Console.WriteLine("    -> POST gw.command.saveDataConfig");
        Console.WriteLine("    -> bizContent: {issuedDeviceCmd:{recordId:100}, collectInterval:5, uploadInterval:60, ...}");
        Console.WriteLine("    -> FreshlianceResponse<int> (returns command ID)");
        Console.WriteLine();
        Console.WriteLine("  DirectConfigAsync(request)");
        Console.WriteLine("    -> POST gw.command.directConfig");
        Console.WriteLine("    -> bizContent: {issuedDeviceCmd:{recordId:100}, collectInterval:5, uploadInterval:60, ...}");
        Console.WriteLine("    -> FreshlianceResponse<int> (returns command ID)");
        Console.WriteLine();
        Console.WriteLine("  DeleteCommandAsync(request)");
        Console.WriteLine("    -> POST gw.command.delete");
        Console.WriteLine("    -> bizContent: {id:200}");
        Console.WriteLine("    -> FreshlianceResponse<bool>");
        Console.WriteLine();
    }

    private static void DemoConfigTemplateService()
    {
        Console.WriteLine("=====================================");
        Console.WriteLine("IConfigTemplateService");
        Console.WriteLine("=====================================");
        Console.WriteLine("  GetPageAsync(request)");
        Console.WriteLine("    -> POST gw.template.page.get");
        Console.WriteLine("    -> bizContent: {pageNum:1, pageSize:10, templateName:\"Standard\"}");
        Console.WriteLine("    -> FreshlianceResponse<PageResult<TemplatePageItemResponse>>");
        Console.WriteLine();
        Console.WriteLine("  GetAsync(request)");
        Console.WriteLine("    -> POST gw.template.get");
        Console.WriteLine("    -> bizContent: {configId:123456789}");
        Console.WriteLine("    -> FreshlianceResponse<TemplateDetailResponse>");
        Console.WriteLine();
        Console.WriteLine("  CreateAsync(request)");
        Console.WriteLine("    -> POST gw.template.create");
        Console.WriteLine("    -> bizContent: {sensorConfig:{...}, categoryId:1, productCode:\"TEMP-001\"}");
        Console.WriteLine("    -> FreshlianceResponse<bool>");
        Console.WriteLine();
        Console.WriteLine("  UpdateAsync(request)");
        Console.WriteLine("    -> POST gw.template.update");
        Console.WriteLine("    -> bizContent: {configId:123456789, sensorConfig:{...}}");
        Console.WriteLine("    -> FreshlianceResponse<bool>");
        Console.WriteLine();
        Console.WriteLine("  DeleteAsync(request)");
        Console.WriteLine("    -> POST gw.template.delete");
        Console.WriteLine("    -> bizContent: {configId:5}");
        Console.WriteLine("    -> FreshlianceResponse<bool>");
        Console.WriteLine();
    }

    private sealed class LoggingHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
        {
            if (request.Content is not null)
            {
                var body = await request.Content.ReadAsStringAsync(ct);
                Console.WriteLine("  >> OUTGOING BODY: " + body);
            }
            return await base.SendAsync(request, ct);
        }
    }

    private static async Task RunAlarmProbeAsync(string appId, string keyFile)
    {
        var key = await File.ReadAllTextAsync(keyFile);

        var services = new ServiceCollection();
        services.AddFreshlianceGateway(options =>
        {
            options.AppId = appId;
            options.PrivateKeyPem = key;
        });

        var provider = services.BuildServiceProvider();
        await using var scope = provider.CreateAsyncScope();
        var client = scope.ServiceProvider.GetRequiredService<FreshlianceClient>();

        Console.WriteLine("=====================================");
        Console.WriteLine("[ALARM METHOD PROBE]");
        Console.WriteLine("  Comparing gw.deviceAlarmData.page vs gw.deviceAlarm.page");
        Console.WriteLine();

        var request = new GetAlarmDataRequest { RecordId = 1, AlarmProperty = 1, PageNum = 1, PageSize = 1 };
        foreach (var method in new[] { "gw.deviceAlarmData.page", "gw.deviceAlarm.page" })
        {
            try
            {
                var resp = await client.PostAsync<PageResult<AlarmDataResponse>>(method, request);
                Console.WriteLine($"  {method,-26} -> code={resp.Code}, msg={resp.Msg}, total={resp.Data?.Total}");
            }
            catch (FreshlianceException ex)
            {
                Console.WriteLine($"  {method,-26} -> EXCEPTION code={ex.Code}, msg={ex.Message}");
            }
        }
        Console.WriteLine();
        Console.WriteLine("  A method-not-found / invalid-method error identifies the wrong name.");
    }

    private static async Task RunGroupSpaceTestAsync(string appId, string keyFile)
    {
        var key = await File.ReadAllTextAsync(keyFile);

        var services = new ServiceCollection();
        services.AddFreshlianceGateway(options =>
        {
            options.AppId = appId;
            options.PrivateKeyPem = key;
        }).AddHttpMessageHandler(() => new LoggingHandler());

        var provider = services.BuildServiceProvider();
        await using var scope = provider.CreateAsyncScope();
        var groupService = scope.ServiceProvider.GetRequiredService<IGroupService>();

        var name = $"CLI Space Test {DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
        Console.WriteLine("=====================================");
        Console.WriteLine("[GROUP SPACE TEST]");
        Console.WriteLine($"  Sending groupName = [{name}]  (len={name.Length}, spaces={name.Count(c => c == ' ')})");
        Console.WriteLine();

        try
        {
            var createResp = await groupService.CreateAsync(
                new CreateGroupRequest { ParentId = 0, GroupName = name });
            Console.WriteLine($"  Create -> code={createResp.Code}, msg={createResp.Msg}, data={createResp.Data}");
            Console.WriteLine();

            Console.WriteLine("  Reading back via GetTreeAsync()...");
            var tree = await groupService.GetTreeAsync();
            if (!tree.IsSuccess)
            {
                Console.WriteLine($"  Tree error: {tree.Code} - {tree.Msg}");
                return;
            }

            var match = FindByPrefix(tree.Data, "CLI Space Test");
            if (match is null)
            {
                Console.WriteLine("  Created group NOT found in tree.");
                return;
            }

            var stored = match.GroupName;
            Console.WriteLine($"  Stored groupName = [{stored}]  (len={stored.Length}, spaces={stored.Count(c => c == ' ')})");
            Console.WriteLine(stored == name
                ? "  RESULT: spaces PRESERVED (matches sent value)."
                : "  RESULT: spaces CHANGED by API (sent != stored).");
        }
        catch (FreshlianceException ex)
        {
            Console.WriteLine($"  Exception: {ex.Code} - {ex.Message}");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"  HTTP Error: {ex.Message}");
        }
    }

    private static GroupTreeNodeResponse? FindByPrefix(IEnumerable<GroupTreeNodeResponse>? nodes, string prefix)
    {
        if (nodes is null) return null;
        foreach (var node in nodes)
        {
            if (node.GroupName.Replace(" ", "").StartsWith(prefix.Replace(" ", ""), StringComparison.Ordinal))
                return node;
            var child = FindByPrefix(node.SubDeviceGroupList, prefix);
            if (child is not null) return child;
        }
        return null;
    }

    private static async Task RunRealDemoAsync(string appId, string keyFile)
    {
        var key = await File.ReadAllTextAsync(keyFile);

        var services = new ServiceCollection();
        services.AddFreshlianceGateway(options =>
        {
            options.AppId = appId;
            options.PrivateKeyPem = key;
        });

        var provider = services.BuildServiceProvider();
        await using var scope = provider.CreateAsyncScope();
        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

        Console.WriteLine("[REAL MODE] Calling GetAsync()...");
        try
        {
            var response = await userService.GetAsync();
            if (response.IsSuccess)
                Console.WriteLine($"  Success! Nickname: {response.Data?.Nickname}, Email: {response.Data?.Email}");
            else
                Console.WriteLine($"  API Error: {response.Code} - {response.Msg}");
        }
        catch (FreshlianceException ex)
        {
            Console.WriteLine($"  Exception: {ex.Code} - {ex.Message}");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"  HTTP Error: {ex.Message}");
        }
    }
}
