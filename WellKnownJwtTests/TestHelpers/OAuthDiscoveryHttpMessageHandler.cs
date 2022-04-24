using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace WellKnownJwtTests.TestHelpers;

public class CustomHandler : HttpMessageHandler
{
    public RsaSecurityKey Key = new(RSA.Create(2048));
    public string TenantId = Guid.NewGuid().ToString().ToLowerInvariant();

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Request for {request.RequestUri}");
        if (request.RequestUri.AbsoluteUri ==
            $"https://login.microsoftonline.com/{TenantId}/v2.0/.well-known/openid-configuration")
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content =
                new ByteArrayContent(Encoding.UTF8.GetBytes(
                    File.ReadAllText("./MetadataEndpointResponses/openid-configuration.json").Replace("<tenant-id>", TenantId)));
            return Task.FromResult(response);
        }

   
        if (request.RequestUri.AbsoluteUri == $"https://login.microsoftonline.com/{TenantId}/discovery/v2.0/keys")
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new ByteArrayContent(
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new
                    {
                        keys = new[]
                        {
                            JsonWebKeyConverter.ConvertFromSecurityKey(this.Key)
                        }
                    })
                ));
            return Task.FromResult(response);
        }

        throw new NotImplementedException();
    }
}