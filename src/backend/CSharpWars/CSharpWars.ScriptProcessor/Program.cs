using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using CSharpWars.Common.DependencyInjection;
using CSharpWars.Common.Tools;
using CSharpWars.ScriptProcessor.DependencyInjection;
using CSharpWars.ScriptProcessor.Middleware.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using static System.Console;
using static System.Convert;
using static System.Environment;

namespace CSharpWars.ScriptProcessor
{
    
    [ExcludeFromCodeCoverage]
    class Program
    {
        private const Int32 DELAY_MS = 2000;

        static void Main(string[] args)
        {
            WriteLine("CSharp Wars Processing Console");
            WriteLine("------------------------------");
            WriteLine();
            WriteLine("Press any key to exit!");
            WriteLine();

            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.ConfigurationHelper(c =>
            {
                c.ConnectionString = GetEnvironmentVariable("CONNECTION_STRING");
                c.ArenaSize = ToInt32(GetEnvironmentVariable("ARENA_SIZE"));
            });
            serviceCollection.ConfigureScriptProcessor();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var middleware = serviceProvider.GetService<IMiddleware>();

            while (!KeyAvailable)
            {
                var start = DateTime.UtcNow;

                try
                {
                    using (var sw = new SimpleStopwatch())
                    {
                        await middleware.Process();
                        WriteLine($"[ CSharpWars Script Processor - PROCESSING {sw.ElapsedMilliseconds}ms! ]");
                    }
                }
                catch (Exception ex)
                {
                    WriteLine($"[ CSharpWars Script Processor - EXCEPTION - '{ex.Message}'! ]");
                }

                var timeTaken = DateTime.UtcNow - start;
                var delay = (Int32)(timeTaken.TotalMilliseconds < DELAY_MS ? DELAY_MS - timeTaken.TotalMilliseconds : 0);
                await Task.Delay(delay);
            }
        }
    }
}