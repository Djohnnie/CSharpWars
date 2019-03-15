using System.Linq;
using System.Threading.Tasks;
using CSharpWars.Common.Extensions;
using CSharpWars.Common.Helpers.Interfaces;
using CSharpWars.ScriptProcessor.Middleware.Interfaces;
using CSharpWars.ScriptProcessor.Moves;

namespace CSharpWars.ScriptProcessor.Middleware
{
    public class Postprocessor : IPostprocessor
    {
        private readonly IRandomHelper _randomHelper;

        public Postprocessor(IRandomHelper randomHelper)
        {
            _randomHelper = randomHelper;
        }

        public Task Go(ProcessingContext context)
        {
            var botProperties = context.GetOrderedBotProperties();
            foreach (var botProperty in botProperties)
            {
                var bot = context.Bots.Single(x => x.Id == botProperty.BotId);
                var botResult = Move.Build(botProperty, _randomHelper).Go();
                bot.Orientation = botResult.Orientation;
                bot.X = botResult.X;
                bot.Y = botResult.Y;
                bot.CurrentHealth = botResult.CurrentHealth;
                bot.CurrentStamina = botResult.CurrentStamina;
                bot.Move = botResult.Move;
                bot.Memory = botResult.Memory.Serialize();
            }

            return Task.CompletedTask;
        }
    }
}