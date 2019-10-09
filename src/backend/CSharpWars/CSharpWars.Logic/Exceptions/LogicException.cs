using System;

namespace CSharpWars.Logic.Exceptions
{
    public class LogicException : Exception
    {
        public LogicException(String message) : base(message) { }
    }
}