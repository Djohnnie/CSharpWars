using System;
using System.Collections.Concurrent;
using CSharpWars.Processor.Middleware.Interfaces;
using Microsoft.CodeAnalysis.Scripting;

namespace CSharpWars.Processor.Middleware
{
    public class BotScriptCache : IBotScriptCache
    {
        private readonly ConcurrentDictionary<Guid, ScriptRunner<object>> _scriptCache = new ConcurrentDictionary<Guid, ScriptRunner<object>>();

        public bool ScriptStored(Guid botId)
        {
            return _scriptCache.ContainsKey(botId);
        }

        public void StoreScript(Guid botId, ScriptRunner<object> script)
        {
            _scriptCache.AddOrUpdate(botId, script, (a, b) => script);
        }

        public ScriptRunner<object> LoadScript(Guid botId)
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
                _scriptCache.TryRemove(botId, out _);
            }
        }
    }
}