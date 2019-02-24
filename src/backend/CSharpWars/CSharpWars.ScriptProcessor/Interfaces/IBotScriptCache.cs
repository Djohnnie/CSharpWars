using System;
using Microsoft.CodeAnalysis.Scripting;

namespace CSharpWars.ScriptProcessor.Interfaces
{
    public interface IBotScriptCache
    {
        Boolean ScriptStored(Guid botId);

        void StoreScript(Guid botId, Script script);

        Script LoadScript(Guid botId);

        void ClearScript(Guid botId);
    }
}