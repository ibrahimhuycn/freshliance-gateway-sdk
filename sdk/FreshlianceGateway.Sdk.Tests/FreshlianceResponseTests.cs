using FluentAssertions;
using Xunit;

namespace FreshlianceGateway.Sdk.Tests;

public class FreshlianceResponseTests
{
    [Fact]
    public void IsSuccess_CodeIsZero_ReturnsTrue()
    {
        var response = new FreshlianceResponse<object>
        {
            Code = "0",
            Msg = "success"
        };

        response.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void IsSuccess_CodeIsNonZero_ReturnsFalse()
    {
        var response = new FreshlianceResponse<object>
        {
            Code = "40001",
            Msg = "invalid parameter"
        };

        response.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void EnsureSuccess_SuccessCode_DoesNotThrow()
    {
        var response = new FreshlianceResponse<object>
        {
            Code = "0",
            Msg = "success"
        };

        var act = () => response.EnsureSuccess();

        act.Should().NotThrow();
    }

    [Fact]
    public void EnsureSuccess_ErrorCode_ThrowsFreshlianceException()
    {
        var response = new FreshlianceResponse<object>
        {
            Code = "40001",
            Msg = "invalid parameter"
        };

        var act = () => response.EnsureSuccess();

        act.Should().Throw<FreshlianceException>()
            .Where(ex => ex.Code == "40001" && ex.Message.Contains("invalid parameter"));
    }

    [Fact]
    public void EnsureSuccess_ErrorCode_HasSubCodeAndSubMsg()
    {
        var response = new FreshlianceResponse<object>
        {
            Code = "40001",
            Msg = "invalid parameter",
            SubCode = "SUB_ERR_001",
            SubMsg = "the 'name' field is required"
        };

        var act = () => response.EnsureSuccess();

        act.Should().Throw<FreshlianceException>()
            .Where(ex =>
                ex.Code == "40001" &&
                ex.SubCode == "SUB_ERR_001" &&
                ex.SubMsg == "the 'name' field is required");
    }
}
