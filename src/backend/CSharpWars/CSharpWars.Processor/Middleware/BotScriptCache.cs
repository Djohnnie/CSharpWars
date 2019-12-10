using System;
using System.Collections.Concurrent;
using CSharpWars.Processor.Middleware.Interfaces;
using Microsoft.CodeAnalysis.Scripting;

namespace CSharpWars.Processor.Middleware
{
    public class BotScriptCache : IBotScriptCache
    {
        private readonly ConcurrentDictionary<Guid, Script> _scriptCache = new ConcurrentDictionary<Guid, Script>();

        public Boolean ScriptStored(Guid botId)
        {
            return _scriptCache.ContainsKey(botId);
        }

        public void StoreScript(Guid botId, Script script)
        {
            _scriptCache.AddOrUpdate(botId, script, (a, b) => script);
        }

        public Script LoadScript(Guid botId)
        {
            if (ScriptStored(botId))
            {
                return _scriptCache[botId];
            }
            throw new ArgumentException("No cached script for bot was found.", nameof(botId));
        }

        public void ClearScript(Guid botId)
        {
            if (ScriptStored(botId))
            {
                Script script;
                _scriptCache.TryRemove(botId, out script);
            }
        }
    }
}