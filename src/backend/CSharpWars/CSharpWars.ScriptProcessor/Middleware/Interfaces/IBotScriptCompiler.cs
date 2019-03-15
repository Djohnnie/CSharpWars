using System;
using Microsoft.CodeAnalysis.Scripting;

namespace CSharpWars.ScriptProcessor.Middleware.Interfaces
{
    public interface IBotScriptCompiler
    {
        Script Compile(String script);
    }
}