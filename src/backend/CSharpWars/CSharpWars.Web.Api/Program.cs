using System.Net;
using CSharpWars.Common.DependencyInjection;
using CSharpWars.DtoModel;
using CSharpWars.Logic.Interfaces;
using CSharpWars.Web.Api;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
builder.WebHost.UseKestrel();
builder.WebHost.ConfigureKestrel((context, options) =>
{
    var certificateFileName = context.Configuration.GetValue<string>("CERTIFICATE_FILENAME");
    var certificatePassword = context.Configuration.GetValue<string>("CERTIFICATE_PASSWORD");

    if (string.IsNullOrEmpty(certificateFileName) || string.IsNullOrEmpty(certificatePassword))
    {
        options.Listen(IPAddress.Any, 5000);
    }
    else
    {
        options.Listen(IPAddress.Any, 5000,
            listenOptions => { listenOptions.UseHttps(certificateFileName, certificatePassword); });
    }
});

// Add services to the container.
builder.Services.ConfigurationHelper(c =>
{
    c.ConnectionString = builder.Configuration.GetValue<string>("CONNECTION_STRING");
    c.ArenaSize = builder.Configuration.GetValue<int>("ARENA_SIZE");
    c.ValidationHost = builder.Configuration.GetValue<string>("VALIDATION_HOST");
    c.BotDeploymentLimit = builder.Configuration.GetValue<int>("BOT_DEPLOYMENT_LIMIT");
});

builder.Services.ConfigureWebApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseRouting();
app.UseHttpMetrics();
app.UseAuthorization();
app.UseCors("AllowAll");
app.MapControllers();
app.MapMetrics();


var apiPrefix = "/api";
app.MapGet($"{apiPrefix}/status", () => Results.Ok());

app.MapGet($"{apiPrefix}/arena", (IApiHelper<IArenaLogic> helper) => helper.Execute(l => l.GetArena()));
app.MapGet($"{apiPrefix}/bots", (IApiHelper<IBotLogic> helper) => helper.Execute(l => l.GetAllActiveBots()));
app.MapPost($"{apiPrefix}/bots", (BotToCreateDto bot, IApiHelper<IBotLogic> helper) => helper.Post(l => l.CreateBot(bot)));
app.MapGet($"{apiPrefix}/players", (IApiHelper<IPlayerLogic> helper) => helper.Execute(l => l.GetAllPlayers()));
app.MapGet($"{apiPrefix}/messages", (IApiHelper<IMessageLogic> helper) => helper.Execute(l => l.GetLastMessages()));
app.MapDelete($"{apiPrefix}/danger/messages", (IApiHelper<IDangerLogic> helper) => helper.Execute(l => l.CleanupMessages()));
app.MapDelete($"{apiPrefix}/danger/bots", (IApiHelper<IDangerLogic> helper) => helper.Execute(l => l.CleanupBots()));
app.MapDelete($"{apiPrefix}/danger/players", (IApiHelper<IDangerLogic> helper) => helper.Execute(l => l.CleanupPlayers()));


app.Run();