using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAD"));

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(
        "MyPolicy",
        new AuthorizationPolicyBuilder("Bearer")
            .RequireScope("fish.read")
            .Build());
});

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.MapGet("/", () => "Hello World!").RequireAuthorization("MyPolicy");

app.Run();

// Make the implicit Program class public so test projects can access it
public partial class Program { }
