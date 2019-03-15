using System.Threading.Tasks;
using CSharpWars.Logic.Interfaces;
using CSharpWars.ScriptProcessor.Middleware.Interfaces;
using IProcessor = CSharpWars.ScriptProcessor.Middleware.Interfaces.IProcessor;

namespace CSharpWars.ScriptProcessor.Middleware
{
    public class Middleware : IMiddleware
    {
        private readonly IArenaLogic _arenaLogic;
        private readonly IBotLogic _botLogic;
        private readonly IPreprocessor _preprocessor;
        private readonly IProcessor _processor;
        private readonly IPostprocessor _postprocessor;

        public Middleware(
            IArenaLogic arenaLogic,
            IBotLogic botLogic,
            IPreprocessor preprocessor,
            IProcessor processor,
            IPostprocessor postprocessor)
        {
            _arenaLogic = arenaLogic;
            _botLogic = botLogic;
            _preprocessor = preprocessor;
            _processor = processor;
            _postprocessor = postprocessor;
        }

        public async Task Process()
        {
            var arena = await _arenaLogic.GetArena();
            var bots = await _botLogic.GetAllActiveBots();

            var context = ProcessingContext.Build(arena, bots);

            await _preprocessor.Go(context);
            await _processor.Go(context);
            await _postprocessor.Go(context);

            await _botLogic.UpdateBots(bots);
        }
    }
}