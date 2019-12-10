using System;
using System.Threading.Tasks;
using CSharpWars.Common.Tools;
using CSharpWars.Logic.Interfaces;
using CSharpWars.Processor.Middleware.Interfaces;
using IProcessor = CSharpWars.Processor.Middleware.Interfaces.IProcessor;

namespace CSharpWars.Processor.Middleware
{
    public class Middleware : IMiddleware
    {
        private readonly IArenaLogic _arenaLogic;
        private readonly IBotLogic _botLogic;
        private readonly IMessageLogic _messageLogic;
        private readonly IPreprocessor _preprocessor;
        private readonly IProcessor _processor;
        private readonly IPostprocessor _postprocessor;

        public Middleware(
            IArenaLogic arenaLogic,
            IBotLogic botLogic,
            IMessageLogic messageLogic,
            IPreprocessor preprocessor,
            IProcessor processor,
            IPostprocessor postprocessor)
        {
            _arenaLogic = arenaLogic;
            _botLogic = botLogic;
            _messageLogic = messageLogic;
            _preprocessor = preprocessor;
            _processor = processor;
            _postprocessor = postprocessor;
        }

        public async Task Process()
        {
            using (var stopwatch = new SimpleStopwatch())
            {
                var arena = await _arenaLogic.GetArena();
                var elapsedArena = stopwatch.ElapsedMilliseconds;

                var bots = await _botLogic.GetAllLiveBots();
                var elapsedBots = stopwatch.ElapsedMilliseconds - elapsedArena;

                var context = ProcessingContext.Build(arena, bots);

                await _preprocessor.Go(context);
                var elapsedPreprocessing = stopwatch.ElapsedMilliseconds - elapsedBots - elapsedArena;

                await _processor.Go(context);
                var elapsedProcessing = stopwatch.ElapsedMilliseconds - elapsedPreprocessing - elapsedBots - elapsedArena;

                await _postprocessor.Go(context);
                var elapsedPostprocessing = stopwatch.ElapsedMilliseconds - elapsedProcessing - elapsedPreprocessing - elapsedBots - elapsedArena;

                await _botLogic.UpdateBots(context.Bots);
                var elapsedUpdateBots = stopwatch.ElapsedMilliseconds - elapsedPostprocessing - elapsedProcessing - elapsedPreprocessing - elapsedBots - elapsedArena;

                //await _messageLogic.CreateMessages(context.Messages);
                var elapsedCreateMessages = stopwatch.ElapsedMilliseconds - elapsedUpdateBots - elapsedPostprocessing - elapsedProcessing - elapsedPreprocessing - elapsedBots - elapsedArena;

                Console.WriteLine(
                    $"{elapsedArena}ms, {elapsedBots}ms, {elapsedPreprocessing}ms, {elapsedProcessing}ms, {elapsedPostprocessing}ms, {elapsedUpdateBots}ms, {elapsedCreateMessages}ms");
            }
        }
    }
}