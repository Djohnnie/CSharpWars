using System;
using CSharpWars.Logging.Interfaces;

namespace CSharpWars.Logging
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}