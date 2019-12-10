using System.Threading.Tasks;
using CSharpWars.Common.DependencyInjection;
using CSharpWars.Processor.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static System.Convert;
using static System.Environment;

namespace CSharpWars.Processor
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).RunConsoleAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.ConfigurationHelper(c =>
                    {
                        c.ConnectionString = GetEnvironmentVariable("CONNECTION_STRING");
                        c.ArenaSize = ToInt32(GetEnvironmentVariable("ARENA_SIZE"));
                    });
                    services.ConfigureScriptProcessor();
                    services.AddHostedService<Worker>();
                });
    }
}