using System;
using System.Linq;
using System.Threading.Tasks;
using CSharpWars.Common.Extensions;
using CSharpWars.Common.Helpers.Interfaces;
using CSharpWars.Enums;
using CSharpWars.Processor.Middleware.Interfaces;
using CSharpWars.Processor.Moves;

namespace CSharpWars.Processor.Middleware
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
                try
                {
                    var bot = context.Bots.Single(x => x.Id == botProperty.BotId);
                    var botResult = Move.Build(botProperty, _randomHelper).Go();
                    bot.Orientation = botResult.Orientation;
                    bot.FromX = bot.X;
                    bot.FromY = bot.Y;
                    bot.X = botResult.X;
                    bot.Y = botResult.Y;
                    bot.CurrentHealth = botResult.CurrentHealth;
                    bot.CurrentStamina = botResult.CurrentStamina;
                    bot.Move = botResult.Move;
                    bot.Memory = botResult.Memory.Serialize();
                    bot.LastAttackX = botResult.LastAttackX;
                    bot.LastAttackY = botResult.LastAttackY;

                    context.UpdateBotProperties(bot);
                    context.UpdateMessages(bot, botProperty);

                    foreach (var otherBot in context.Bots.Where(x => x.Id != bot.Id))
                    {
                        otherBot.CurrentHealth -= botResult.GetInflictedDamage(otherBot.Id);
                        var teleportation = botResult.GetTeleportation(otherBot.Id);
                        if (teleportation != (-1, -1))
                        {
                            otherBot.X = teleportation.X;
                            otherBot.Y = teleportation.Y;
                        }

                        var otherBotProperties = botProperties.Single(x => x.BotId == otherBot.Id);
                        otherBotProperties.Update(otherBot);
                    }
                }
                catch (Exception ex)
                {

                }
            }

            foreach (var bot in context.Bots)
            {
                if (bot.CurrentHealth <= 0)
                {
                    bot.CurrentHealth = 0;
                    bot.TimeOfDeath = DateTime.UtcNow;
                    if (bot.Move != PossibleMoves.SelfDestruct)
                    {
                        bot.Move = PossibleMoves.Died;
                    }
                }

                if (bot.CurrentStamina <= 0)
                {
                    bot.CurrentStamina = 0;
                }
            }

            return Task.CompletedTask;
        }
    }
}