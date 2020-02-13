using System;

namespace CSharpWars.Logic.Exceptions
{
    public class LogicException : Exception
    {
        public LogicException(string message) : base(message) { }
    }
}