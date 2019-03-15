using System.Threading.Tasks;
using CSharpWars.Scripting.Model;
using CSharpWars.ScriptProcessor.Middleware.Interfaces;

namespace CSharpWars.ScriptProcessor.Middleware
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