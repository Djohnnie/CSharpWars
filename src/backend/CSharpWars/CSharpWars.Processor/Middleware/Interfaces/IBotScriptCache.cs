using System;
using Microsoft.CodeAnalysis.Scripting;

namespace CSharpWars.Processor.Middleware.Interfaces
{
    public interface IBotScriptCache
    {
        bool ScriptStored(Guid botId);

        void StoreScript(Guid botId, ScriptRunner<object> script);

        ScriptRunner<object> LoadScript(Guid botId);

        void ClearScript(Guid botId);
    }
}