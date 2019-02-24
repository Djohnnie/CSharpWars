using System;
using System.Threading.Tasks;
using CSharpWars.Common.Tools;

namespace CSharpWars.ScriptProcessor
{
    class Program
    {
        private const Int32 DELAY_MS = 2000;

        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            //Processor p = new Processor();

            while (true)
            {
                var start = DateTime.UtcNow;

                try
                {
                    using (var sw = new SimpleStopwatch())
                    {
                        //await p.GoPublic();
                        Console.WriteLine($"[ CSharpWars Script Processor - PROCESSING {sw.ElapsedMilliseconds}ms! ]");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ CSharpWars Script Processor - EXCEPTION - '{ex.Message}'! ]");
                }

                var timeTaken = DateTime.UtcNow - start;
                var delay = (Int32)(timeTaken.TotalMilliseconds < DELAY_MS ? DELAY_MS - timeTaken.TotalMilliseconds : 0);
                await Task.Delay(delay);
            }
        }
    }
}