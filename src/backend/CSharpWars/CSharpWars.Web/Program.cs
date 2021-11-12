//using System;
//using System.Net;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Hosting;

//namespace CSharpWars.Web
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            var certificateFileName = Environment.GetEnvironmentVariable("CERTIFICATE_FILENAME");
//            var certificatePassword = Environment.GetEnvironmentVariable("CERTIFICATE_PASSWORD");
//            CreateHostBuilder(args, certificateFileName, certificatePassword).Build().Run();
//        }

//        public static IHostBuilder CreateHostBuilder(string[] args, string certificateFileName, string certificatePassword) =>
//            Host.CreateDefaultBuilder(args)
//                .ConfigureAppConfiguration((hostContext, configurationBuilder) =>
//                {
//                    configurationBuilder.AddEnvironmentVariables();
//                })
//                .ConfigureWebHostDefaults(webBuilder =>
//                {
//                    webBuilder.UseKestrel();
//                    webBuilder.ConfigureKestrel((hostContext, options) =>
//                    {
//                        if (string.IsNullOrEmpty(certificateFileName) || string.IsNullOrEmpty(certificatePassword))
//                        {
//                            options.Listen(IPAddress.Any, 5000);
//                        }
//                        else
//                        {
//                            options.Listen(IPAddress.Any, 5000,
//                                listenOptions => { listenOptions.UseHttps(certificateFileName, certificatePassword); });
//                        }
//                    });
//                    webBuilder.UseStartup<Startup>();
//                });
//    }
//}

using System.Net;
using CSharpWars.Common.DependencyInjection;
using CSharpWars.Web.DependencyInjection;
using Prometheus;
using static System.Convert;
using static System.Environment;

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
    c.ConnectionString = GetEnvironmentVariable("CONNECTION_STRING");
    c.ArenaSize = ToInt32(GetEnvironmentVariable("ARENA_SIZE"));
    c.ValidationHost = GetEnvironmentVariable("VALIDATION_HOST");
    c.PointsLimit = ToInt32(GetEnvironmentVariable("POINTS_LIMIT"));
    c.BotDeploymentLimit = ToInt32(GetEnvironmentVariable("BOT_DEPLOYMENT_LIMIT"));
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