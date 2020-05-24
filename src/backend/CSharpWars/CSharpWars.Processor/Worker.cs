using System;
using System.Threading;
using System.Threading.Tasks;
using CSharpWars.Common.Tools;
using CSharpWars.Processor.Middleware.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CSharpWars.Processor
{
    public class Worker : BackgroundService
    {
        private const int DELAY_MS = 2000;

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<Worker> _logger;

        public Worker(IServiceScopeFactory scopeFactory, ILogger<Worker> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scopedServiceProvider = _scopeFactory.CreateScope();
                var start = DateTime.UtcNow;

                try
                {
                    using var sw = new SimpleStopwatch();
                    var middleware = scopedServiceProvider.ServiceProvider.GetService<IMiddleware>();
                    await middleware.Process();
                    _logger.LogInformation("[ CSharpWars Script Processor - PROCESSING {elapsedMilliseconds}ms! ]", sw.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "[ CSharpWars Script Processor - EXCEPTION - '{exceptionMessage}'! ]", ex.Message);
                }

                var timeTaken = DateTime.UtcNow - start;
                var delay = (int)(timeTaken.TotalMilliseconds < DELAY_MS ? DELAY_MS - timeTaken.TotalMilliseconds : 0);
                await Task.Delay(delay, stoppingToken);
            }
        }
    }
}