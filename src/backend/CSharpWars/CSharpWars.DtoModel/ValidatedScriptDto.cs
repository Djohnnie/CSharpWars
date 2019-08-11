using System;
using System.Collections.Generic;

namespace CSharpWars.DtoModel
{
    public class ValidatedScriptDto
    {
        public String Script { get; set; }

        public Int64 CompilationTimeInMilliseconds { get; set; }

        public Int64 RunTimeInMilliseconds { get; set; }

        public List<ScriptValidationMessage> Messages { get; set; }
    }
}