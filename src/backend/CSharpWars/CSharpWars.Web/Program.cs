using System.Net;
using CSharpWars.Common.DependencyInjection;
using CSharpWars.Web.DependencyInjection;
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
builder.Services.AddControllersWithViews();
builder.Services.ConfigurationHelper(c =>
{
    c.ConnectionString = builder.Configuration.GetValue<string>("CONNECTION_STRING");
    c.ArenaSize = builder.Configuration.GetValue<int>("ARENA_SIZE");
    c.ValidationHost = builder.Configuration.GetValue<string>("VALIDATION_HOST");
    c.PointsLimit = builder.Configuration.GetValue<int>("POINTS_LIMIT");
    c.BotDeploymentLimit = builder.Configuration.GetValue<int>("BOT_DEPLOYMENT_LIMIT");
});

builder.Services.ConfigureWeb();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseHttpMetrics();
app.UseCookiePolicy();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapMetrics();

app.Run();