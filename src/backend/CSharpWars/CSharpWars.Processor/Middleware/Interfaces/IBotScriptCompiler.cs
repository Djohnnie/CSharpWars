using System;
using Microsoft.CodeAnalysis.Scripting;

namespace CSharpWars.Processor.Middleware.Interfaces
{
    public interface IBotScriptCompiler
    {
        ScriptRunner<object> Compile(string script);
    }
}