using System;
using System.Threading.Tasks;
using CSharpWars.DtoModel;
using CSharpWars.Enums;
using CSharpWars.Logging.Interfaces;
using CSharpWars.Logic.Interfaces;
using CSharpWars.Processor.Middleware.Interfaces;
using CSharpWars.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace CSharpWars.Processor.Middleware
{
    public class BotProcessingFactory : IBotProcessingFactory
    {
        private readonly IBotLogic _botLogic;
        private readonly IBotScriptCompiler _botScriptCompiler;
        private readonly IBotScriptCache _botScriptCache;
        private readonly ILogger _logger;

        public BotProcessingFactory(
            IBotLogic botLogic,
            IBotScriptCompiler botScriptCompiler,
            IBotScriptCache botScriptCache, ILogger logger)
        {
            _botLogic = botLogic;
            _botScriptCompiler = botScriptCompiler;
            _botScriptCache = botScriptCache;
            _logger = logger;
        }

        public async Task Process(BotDto bot, ProcessingContext context)
        {
            var botProperties = context.GetBotProperties(bot.Id);
            try
            {
                var botScript = await GetCompiledBotScript(bot);
                var scriptGlobals = ScriptGlobals.Build(botProperties);
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
                    var botScript = _botScriptCompiler.Compile(script);
                    _botScriptCache.StoreScript(bot.Id, botScript);
                }
                catch (Exception ex)
                {
                    _logger.Log($"{ex}");
                }
            }

            return _botScriptCache.LoadScript(bot.Id);
        }
    }
}