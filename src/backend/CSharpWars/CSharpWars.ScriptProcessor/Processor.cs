using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CSharpWars.Common.Extensions;
using CSharpWars.Common.Helpers.Interfaces;
using CSharpWars.Common.Tools;
using CSharpWars.DtoModel;
using CSharpWars.Enums;
using CSharpWars.Logic.Interfaces;
using CSharpWars.Scripting;
using CSharpWars.Scripting.Model;
using CSharpWars.ScriptProcessor.Interfaces;
using CSharpWars.ScriptProcessor.Moves;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace CSharpWars.ScriptProcessor
{
    public class Processor : IProcessor
    {
        private readonly ConcurrentDictionary<Guid, BotProperties> _botProperties = new ConcurrentDictionary<Guid, BotProperties>();

        private readonly IBotScriptCache _botScriptCache;
        private readonly IArenaLogic _arenaLogic;
        private readonly IBotLogic _botLogic;
        private readonly IRandomHelper _randomHelper;

        public Processor(IBotScriptCache botScriptCache, IArenaLogic arenaLogic, IBotLogic botLogic, IRandomHelper randomHelper)
        {
            _botScriptCache = botScriptCache;
            _arenaLogic = arenaLogic;
            _botLogic = botLogic;
            _randomHelper = randomHelper;
        }

        public async Task Go()
        {
            var arena = await _arenaLogic.GetArena();
            var bots = await _botLogic.GetAllActiveBots();

            using (var sw = new SimpleStopwatch())
            {
                // Preprocessing
                foreach (var bot in bots)
                {
                    var botProperties = BotProperties.Build(bot, arena, bots);
                    _botProperties.TryAdd(bot.Id, botProperties);
                }
                Debug.WriteLine($"PRE-PROCESSING: {sw.ElapsedMilliseconds}");
            }

            using (var sw = new SimpleStopwatch())
            {
                // Processing
                var botProcessing = bots.Select(BotProcessingFactory);
                await Task.WhenAll(botProcessing);
                Debug.WriteLine($"PROCESSING: {sw.ElapsedMilliseconds}");
            }

            using (var sw = new SimpleStopwatch())
            {
                // Postprocessing
                PostProcess(bots);
                Debug.WriteLine($"POST-PROCESSING: {sw.ElapsedMilliseconds}");
            }

            using (var sw = new SimpleStopwatch())
            {
                // Update
                await _botLogic.UpdateBots(bots);
                Debug.WriteLine($"SAVING: {sw.ElapsedMilliseconds}");
            }

            // 4. Cleanup
            _botProperties.Clear();
        }

        private void PostProcess(IList<BotDto> bots)
        {
            var botProperties = _botProperties.Values.OrderBy(x => x.CurrentMove, new MoveComparer());
            foreach (var botProperty in botProperties)
            {
                var bot = bots.Single(x => x.Id == botProperty.BotId);
                var botResult = Move.Build(botProperty, _randomHelper).Go();
                bot.Orientation = botResult.Orientation;
                bot.X = botResult.X;
                bot.Y = botResult.Y;
                bot.CurrentHealth = botResult.CurrentHealth;
                bot.CurrentStamina = botResult.CurrentStamina;
                bot.Move = botResult.Move;
                bot.Memory = botResult.Memory.Serialize();
            }
        }

        private async Task BotProcessingFactory(BotDto bot)
        {
            var botProperties = _botProperties[bot.Id];
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