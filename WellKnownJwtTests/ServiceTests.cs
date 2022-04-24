using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Shouldly;
using WellKnownJwtTests.TestHelpers;
using Xunit;
using Xunit.Abstractions;

namespace WellKnownJwtTests;

public class UnitTest1
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly ServiceHelper _jwtHelper;

    public UnitTest1(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _jwtHelper = new ServiceHelper("10508242-aeb3-4f26-8c09-cf4c2eba4ca3");
    }

    [Fact]
    public async Task OkWithValidToken()
    {
        var webFactory = _jwtHelper.CreateWebApplicationFactory(_testOutputHelper);
        var token = _jwtHelper.CreateJwtBuilder()
            .WithScopes("fish.read")
            .Build();

        var client = webFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var result = await client.GetStringAsync("/");
        result.ShouldBe("Hello World!");
    }

    [Fact]
    public async Task FailsWithWrongScope()
    {
        var webFactory = _jwtHelper.CreateWebApplicationFactory(_testOutputHelper);
        var token = _jwtHelper.CreateJwtBuilder()
            .WithScopes("fish.write")
            .Build();

        var client = webFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var result = await client.GetAsync("/");
        result.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}