using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CSharpWars.Common.Extensions;
using CSharpWars.DtoModel;
using CSharpWars.Logic.Interfaces;
using CSharpWars.Scripting;
using CSharpWars.Scripting.Model;
using CSharpWars.ScriptProcessor.Interfaces;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace CSharpWars.ScriptProcessor
{
    public class Processor : IProcessor
    {
        private readonly IBotScriptCache _botScriptCache;
        private readonly IArenaLogic _arenaLogic;
        private readonly IBotLogic _botLogic;

        public Processor(IBotScriptCache botScriptCache, IArenaLogic arenaLogic, IBotLogic botLogic)
        {
            _botScriptCache = botScriptCache;
            _arenaLogic = arenaLogic;
            _botLogic = botLogic;
        }

        public async Task Go()
        {
            var arena = await _arenaLogic.GetArena();
            var bots = await _botLogic.GetAllActiveBots();

            var botProcessing = bots.Select(bot => BotProcessingFactory(bot, arena));
            await Task.WhenAll(botProcessing);
        }

        private async Task BotProcessingFactory(BotDto bot, ArenaDto arena)
        {
            var botScript = GetCompiledBotScript(bot);
            var botProperties = BotProperties.Build(bot, arena);
            var scriptGlobals = new ScriptGlobals(botProperties);
            await botScript.RunAsync(scriptGlobals);
        }


        private Script GetCompiledBotScript(BotDto bot)
        {
            if (!_botScriptCache.ScriptStored(bot.Id))
            {
                var botScript = PrepareScript(bot.Script);
                botScript.Compile();
                _botScriptCache.StoreScript(bot.Id, botScript);
            }

            return _botScriptCache.LoadScript(bot.Id);
        }

        public static Script<Object> PrepareScript(String script)
        {
            var decodedScript = script.Base64Decode();
            var mscorlib = typeof(Object).Assembly;
            var systemCore = typeof(Enumerable).Assembly;
            var dynamic = typeof(DynamicAttribute).Assembly;
            var csharpScript = typeof(BotProperties).Assembly;
            var scriptOptions = ScriptOptions.Default.AddReferences(mscorlib, systemCore, dynamic, csharpScript);
            scriptOptions = scriptOptions.WithImports("System", "System.Linq", "System.Collections", "System.Collections.Generic", "CSharpWars.Scripting.Model", "System.Runtime.CompilerServices");
            var botScript = CSharpScript.Create(decodedScript, scriptOptions, typeof(ScriptGlobals));
            botScript.WithOptions(botScript.Options.AddReferences(mscorlib, systemCore));
            return botScript;
        }
    }
}