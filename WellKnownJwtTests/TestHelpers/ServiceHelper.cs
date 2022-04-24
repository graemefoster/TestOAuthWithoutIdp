using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using Xunit.Abstractions;

namespace WellKnownJwtTests.TestHelpers;

public  class ServiceHelper
{
    private readonly string _clientId;
    private readonly CustomHandler _metadataProvider = new();

    public ServiceHelper(string clientId)
    {
        _clientId = clientId;
    }
    public WebApplicationFactory<Program> CreateWebApplicationFactory(ITestOutputHelper testOutputHelper)
    {
        
        return new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureLogging(logging =>
                    {
                        logging.Services.AddSingleton(() => new XUnitAspNetLogger(testOutputHelper));
                    })
                    .ConfigureServices(svc =>
                    {
                        svc.Configure<MicrosoftIdentityOptions>(options =>
                        {
                            options.TenantId = _metadataProvider.TenantId;
                        });
                        svc.Configure<JwtBearerOptions>(
                            "Bearer",
                            options =>
                            {
                                options.BackchannelHttpHandler = _metadataProvider;
                            });
                    });
            });
    }

    public JwtBuilder CreateJwtBuilder()
    {
        return new JwtBuilder(_metadataProvider.Key, _metadataProvider.TenantId, _clientId);
    }
}