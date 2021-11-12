using CSharpWars.Processor.Middleware.Interfaces;

namespace CSharpWars.Processor.Middleware;

public class Processor : IProcessor
{
    private readonly IBotProcessingFactory _botProcessingFactory;

    public Processor(IBotProcessingFactory botProcessingFactory)
    {
        _botProcessingFactory = botProcessingFactory;
    }

    public Task Go(ProcessingContext context)
    {
        return Parallel.ForEachAsync(context.Bots, async (bot, _) =>
        {
            await _botProcessingFactory.Process(bot, context);
        });
    }
}