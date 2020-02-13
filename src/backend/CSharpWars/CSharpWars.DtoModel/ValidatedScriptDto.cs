using System;
using System.Collections.Generic;

namespace CSharpWars.DtoModel
{
    public class ValidatedScriptDto
    {
        public string Script { get; set; }

        public long CompilationTimeInMilliseconds { get; set; }

        public long RunTimeInMilliseconds { get; set; }

        public List<ScriptValidationMessage> Messages { get; set; }
    }
}