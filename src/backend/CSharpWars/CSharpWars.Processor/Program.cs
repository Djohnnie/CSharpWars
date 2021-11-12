using CSharpWars.Common.DependencyInjection;
using CSharpWars.Processor;
using CSharpWars.Processor.DependencyInjection;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

IHost host = Host.CreateDefaultBuilder(args)
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
    })
    .Build();

await host.RunAsync();