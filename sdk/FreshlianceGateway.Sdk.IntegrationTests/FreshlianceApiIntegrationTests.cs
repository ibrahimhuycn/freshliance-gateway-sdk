using System.Text.Json;
using FluentAssertions;
using FreshlianceGateway.Sdk.Models;
using FreshlianceGateway.Sdk.Models.Device;
using FreshlianceGateway.Sdk.Models.Group;
using FreshlianceGateway.Sdk.Services;
using Xunit;

namespace FreshlianceGateway.Sdk.IntegrationTests;

public class FreshlianceApiIntegrationTests : IClassFixture<WireMockServerFixture>
{
    private readonly WireMockServerFixture _fixture;

    public FreshlianceApiIntegrationTests(WireMockServerFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetUserInfo_ReturnsUserData()
    {
        _fixture.ResetAndStubSuccess("""
            {
                "code": "0",
                "msg": "success",
                "sign": "mock-signature",
                "data": {
                    "email": "test@freshliance.com",
                    "timeZone": "Asia/Shanghai",
                    "language": 1,
                    "dateFormat": 1,
                    "temperatureUnit": 1,
                    "chnSmsNum": 100,
                    "intSmsNum": 50,
                    "chnVoiceNum": 10,
                    "intVoiceNum": 5,
                    "nickName": "TestUser"
                }
            }
            """);

        var client = _fixture.CreateClient();
        var service = new UserService(client);

        var response = await service.GetAsync(TestContext.Current.CancellationToken);

        response.Should().NotBeNull();
        response.Code.Should().Be("0");
        response.IsSuccess.Should().BeTrue();
        response.Data.Should().NotBeNull();
        response.Data!.Email.Should().Be("test@freshliance.com");
        response.Data.TimeZone.Should().Be("Asia/Shanghai");
        response.Data.Language.Should().Be(1);
        response.Data.DateFormat.Should().Be(1);
        response.Data.TemperatureUnit.Should().Be(1);
        response.Data.ChnSmsNum.Should().Be(100);
        response.Data.IntSmsNum.Should().Be(50);
        response.Data.ChnVoiceNum.Should().Be(10);
        response.Data.IntVoiceNum.Should().Be(5);
        response.Data.Nickname.Should().Be("TestUser");
    }

    [Fact]
    public async Task GetDeviceCategories_ReturnsCategoryList()
    {
        _fixture.ResetAndStubSuccess("""
            {
                "code": "0",
                "msg": "success",
                "sign": "mock-signature",
                "data": [
                    {
                        "categoryId": 1,
                        "categoryName": "Vaccine",
                        "inTemHigh": 8.0,
                        "inTemLow": 2.0,
                        "extTemHigh": 10.0,
                        "extTemLow": 0.0,
                        "productSensor": 2,
                        "remark": null
                    },
                    {
                        "categoryId": 2,
                        "categoryName": "Food",
                        "inTemHigh": 25.0,
                        "inTemLow": -5.0,
                        "extTemHigh": 30.0,
                        "extTemLow": -10.0,
                        "productSensor": 1,
                        "remark": "Food storage"
                    }
                ]
            }
            """);

        var client = _fixture.CreateClient();
        var service = new DeviceService(client);

        var response = await service.GetCategoriesAsync(TestContext.Current.CancellationToken);

        response.Should().NotBeNull();
        response.IsSuccess.Should().BeTrue();
        response.Data.Should().NotBeNull();
        response.Data.Should().HaveCount(2);

        response.Data![0].CategoryId.Should().Be(1);
        response.Data[0].CategoryName.Should().Be("Vaccine");
        response.Data[0].InTemHigh.Should().Be(8.0);
        response.Data[0].InTemLow.Should().Be(2.0);
        response.Data[0].ProductSensor.Should().Be(2);
        response.Data[0].Remark.Should().BeNull();

        response.Data[1].CategoryId.Should().Be(2);
        response.Data[1].CategoryName.Should().Be("Food");
        response.Data[1].Remark.Should().Be("Food storage");
    }

    [Fact]
    public async Task GetDevicePage_ReturnsPagedDevices()
    {
        _fixture.ResetAndStubSuccess("""
            {
                "code": "0",
                "msg": "success",
                "sign": "mock-signature",
                "data": {
                    "total": 42,
                    "rows": [
                        {
                            "deviceInfo": {
                                "userDeviceId": 1001,
                                "productCode": "ABC",
                                "deviceSn": "SN-001",
                                "deviceCode": "DC001",
                                "deviceName": "Warehouse Sensor A",
                                "deviceId": 2001,
                                "recordId": 3001,
                                "productType": 2,
                                "productModel": "Model-X",
                                "userParentId": 0,
                                "deviceStatus": 1,
                                "statusTime": 1700000000000,
                                "alarmStatus": 1,
                                "alarmTime": 0,
                                "devicePower": 85,
                                "powerTime": 1700000000000,
                                "gatewaySn": null,
                                "productScene": null
                            },
                            "userParentId": 0,
                            "subDeviceLastDataList": [],
                            "deviceDeviceStateCount": null
                        },
                        {
                            "deviceInfo": {
                                "userDeviceId": 1002,
                                "productCode": "DEF",
                                "deviceSn": "SN-002",
                                "deviceCode": "DC002",
                                "deviceName": "Cold Room B",
                                "deviceId": 2002,
                                "recordId": 3002,
                                "productType": 1,
                                "productModel": "Model-Y",
                                "userParentId": 0,
                                "deviceStatus": 0,
                                "statusTime": 1700000000000,
                                "alarmStatus": 1,
                                "alarmTime": 0,
                                "devicePower": null,
                                "powerTime": 0,
                                "gatewaySn": "GW-001",
                                "productScene": 1
                            },
                            "userParentId": 5,
                            "subDeviceLastDataList": [],
                            "deviceDeviceStateCount": null
                        }
                    ]
                }
            }
            """);

        var client = _fixture.CreateClient();
        var service = new DeviceService(client);

        var request = new GetDevicePageRequest { PageNum = 1, PageSize = 10 };
        var response = await service.GetPageAsync(request, TestContext.Current.CancellationToken);

        response.Should().NotBeNull();
        response.IsSuccess.Should().BeTrue();
        response.Data.Should().NotBeNull();
        response.Data!.Total.Should().Be(42);
        response.Data.Rows.Should().HaveCount(2);

        response.Data.Rows[0].DeviceInfo.DeviceSn.Should().Be("SN-001");
        response.Data.Rows[0].DeviceInfo.DeviceName.Should().Be("Warehouse Sensor A");
        response.Data.Rows[0].DeviceInfo.DeviceStatus.Should().Be(1);
        response.Data.Rows[0].DeviceInfo.DevicePower.Should().Be(85);

        response.Data.Rows[1].DeviceInfo.DeviceSn.Should().Be("SN-002");
        response.Data.Rows[1].DeviceInfo.DeviceName.Should().Be("Cold Room B");
        response.Data.Rows[1].DeviceInfo.DeviceStatus.Should().Be(0);
        response.Data.Rows[1].DeviceInfo.DevicePower.Should().BeNull();
        response.Data.Rows[1].DeviceInfo.GatewaySn.Should().Be("GW-001");
        response.Data.Rows[1].UserParentId.Should().Be(5);
    }

    [Fact]
    public async Task CreateGroup_ReturnsTrue()
    {
        _fixture.ResetAndStubSuccess("""
            {
                "code": "0",
                "msg": "success",
                "sign": "mock-signature",
                "data": true
            }
            """);

        var client = _fixture.CreateClient();
        var service = new GroupService(client);

        var request = new CreateGroupRequest { ParentId = 0, GroupName = "New Group" };
        var response = await service.CreateAsync(request, TestContext.Current.CancellationToken);

        response.Should().NotBeNull();
        response.IsSuccess.Should().BeTrue();
        response.Data.Should().BeTrue();
    }

    [Fact]
    public async Task ErrorCode_ThrowsFreshlianceException()
    {
        _fixture.ResetAndStubSuccess("""
            {
                "code": "40000",
                "msg": "Invalid parameter",
                "subCode": "isv.invalid-parameter",
                "subMsg": "Parameter deviceSn is missing",
                "sign": "mock-signature",
                "data": null
            }
            """);

        var client = _fixture.CreateClient();
        var service = new UserService(client);

        var response = await service.GetAsync(TestContext.Current.CancellationToken);

        response.Should().NotBeNull();
        response.Code.Should().Be("40000");
        response.IsSuccess.Should().BeFalse();
        response.Msg.Should().Be("Invalid parameter");
        response.SubCode.Should().Be("isv.invalid-parameter");
        response.SubMsg.Should().Be("Parameter deviceSn is missing");

        var act = () => response.EnsureSuccess();
        act.Should().Throw<FreshlianceException>()
            .Where(ex => ex.Code == "40000"
                && ex.SubCode == "isv.invalid-parameter"
                && ex.Message.Contains("Invalid parameter"));
    }

    [Fact]
    public async Task HttpError_ThrowsFreshlianceException()
    {
        _fixture.ResetAndStubError(500, """{"error":"internal server error"}""");

        var client = _fixture.CreateClient();
        var service = new UserService(client);

        var act = () => service.GetAsync(TestContext.Current.CancellationToken);

        (await act.Should().ThrowAsync<FreshlianceException>())
            .Where(ex => ex.Code == "500"
                && ex.Message.Contains("HTTP"));
    }
}
