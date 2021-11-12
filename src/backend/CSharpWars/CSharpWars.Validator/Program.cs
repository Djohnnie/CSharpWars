using System.Net;
using CSharpWars.Validator.Helpers;
using CSharpWars.Validator.Helpers.Interfaces;
using CSharpWars.Validator.Services;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseKestrel();
builder.WebHost.ConfigureKestrel(options =>
{
    var certificateFileName = Environment.GetEnvironmentVariable("CERTIFICATE_FILENAME");
    var certificatePassword = Environment.GetEnvironmentVariable("CERTIFICATE_PASSWORD");

    options.Listen(IPAddress.Any, 5000,
        listenOptions => { listenOptions.UseHttps(certificateFileName, certificatePassword); });
});

// Add services to the container.
builder.Services.AddScoped<IScriptValidationHelper, ScriptValidationHelper>();
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<ScriptValidatorService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();