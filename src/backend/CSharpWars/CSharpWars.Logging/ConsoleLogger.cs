using System;
using CSharpWars.Logging.Interfaces;

namespace CSharpWars.Logging
{
    public class ConsoleLogger : ILogger
    {
        public void Log(String message)
        {
            Console.WriteLine(message);
        }
    }
}