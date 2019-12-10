using System.Threading.Tasks;
using CSharpWars.Processor.Middleware.Interfaces;
using CSharpWars.Scripting.Model;

namespace CSharpWars.Processor.Middleware
{
    public class Preprocessor : IPreprocessor
    {
        public Task Go(ProcessingContext context)
        {
            foreach (var bot in context.Bots)
            {
                var botProperties = BotProperties.Build(bot, context.Arena, context.Bots);
                context.AddBotProperties(bot.Id, botProperties);
            }

            return Task.CompletedTask;
        }
    }
}