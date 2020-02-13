using System;

namespace CSharpWars.DtoModel
{
    public class ScriptValidationMessage
    {
        public string Message { get; set; }

        public int LocationStart { get; set; }

        public int LocationEnd { get; set; }
    }
}