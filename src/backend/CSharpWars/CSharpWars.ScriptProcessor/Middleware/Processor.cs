using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CSharpWars.Common.Extensions;
using CSharpWars.DtoModel;
using CSharpWars.Enums;
using CSharpWars.Logic.Interfaces;
using CSharpWars.Scripting;
using CSharpWars.Scripting.Model;
using CSharpWars.ScriptProcessor.Interfaces;
using CSharpWars.ScriptProcessor.Middleware.Interfaces;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace CSharpWars.ScriptProcessor.Middleware
{
    public class Processor : IProcessor
    {
        private readonly IBotLogic _botLogic;
        private readonly IBotScriptCache _botScriptCache;

        public Processor(IBotLogic botLogic, IBotScriptCache botScriptCache)
        {
            _botLogic = botLogic;
            _botScriptCache = botScriptCache;
        }

        public async Task Go(ProcessingContext context)
        {
            var botProcessing = context.Bots.Select(b => BotProcessingFactory(b, context));
            await Task.WhenAll(botProcessing);
        }

        private async Task BotProcessingFactory(BotDto bot, ProcessingContext context)
        {
            var botProperties = context.GetBotProperties(bot.Id);
            try
            {
                var botScript = await GetCompiledBotScript(bot);
                var scriptGlobals = new ScriptGlobals(botProperties);
                await botScript.RunAsync(scriptGlobals);
            }
            catch
            {
                botProperties.CurrentMove = PossibleMoves.ScriptError;
            }
        }

        private async Task<Script> GetCompiledBotScript(BotDto bot)
        {
            if (!_botScriptCache.ScriptStored(bot.Id))
            {
                try
                {
                    var script = await _botLogic.GetBotScript(bot.Id);
                    var botScript = PrepareScript(script);
                    botScript.Compile();
                    _botScriptCache.StoreScript(bot.Id, botScript);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"{ex}");
                }
            }

            return _botScriptCache.LoadScript(bot.Id);
        }

        private static Script<Object> PrepareScript(String script)
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