using System;
using System.Threading.Tasks;
using CSharpWars.Common.DependencyInjection;
using CSharpWars.Processor.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

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
                .ConfigureAppConfiguration((hostContext, configBuilder) =>
                {
                    configBuilder.AddEnvironmentVariables();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.ConfigurationHelper(c =>
                    {
                        c.ConnectionString = hostContext.Configuration.GetValue<string>("CONNECTION_STRING");
                        c.ArenaSize = hostContext.Configuration.GetValue<int>("ARENA_SIZE");
                    });
                    services.ConfigureScriptProcessor();
                    services.AddHostedService<Worker>();
                })
                .ConfigureLogging((hostContext, logging) =>
                {
                    var elasticHost = hostContext.Configuration.GetValue<string>("ELASTIC_HOST");
                    Log.Logger = new LoggerConfiguration()
                        .Enrich.FromLogContext()
                        .Enrich.WithExceptionDetails()
                        .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticHost))
                        {
                            AutoRegisterTemplate = true,
                            IndexFormat = "csharpwars-processor-{0:yyyy.MM}"
                        }).CreateLogger();
                    logging.AddSerilog();
                });
    }
}