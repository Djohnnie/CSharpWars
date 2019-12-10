using System;
using System.Collections.Generic;
using System.Linq;
using CSharpWars.Enums;
using CSharpWars.Processor.Middleware;
using CSharpWars.Scripting;
using CSharpWars.Scripting.Model;

namespace CSharpWars.Processor.Moves
{
    public class SelfDestruct : Move
    {
        public SelfDestruct(BotProperties botProperties) : base(botProperties)
        {
        }

        public override BotResult Go()
        {
            // Build result based on current properties.
            var botResult = BotResult.Build(BotProperties);

            var botsInMaximumVicinity = FindBotsInVicinity(1);
            var botsInMediumVicinity = FindBotsInVicinity(2);
            var botsInMinimumVicinity = FindBotsInVicinity(3);

            foreach (var bot in botsInMinimumVicinity)
            {
                botResult.InflictDamage(bot.Id, Constants.SELF_DESTRUCT_MIN_DAMAGE);
            }

            foreach (var bot in botsInMediumVicinity)
            {
                botResult.InflictDamage(bot.Id, Constants.SELF_DESTRUCT_MED_DAMAGE);
            }

            foreach (var bot in botsInMaximumVicinity)
            {
                botResult.InflictDamage(bot.Id, Constants.SELF_DESTRUCT_MAX_DAMAGE);
            }

            botResult.CurrentHealth = 0;
            botResult.Move = PossibleMoves.SelfDestruct;

            return botResult;
        }

        private IList<Bot> FindBotsInVicinity(Int32 range)
        {
            var botsInVicinity = new List<Bot>();
            for (Int32 x = BotProperties.X - range; x <= BotProperties.X + range; x++)
            {
                for (Int32 y = BotProperties.Y - range; y <= BotProperties.Y + range; y++)
                {
                    if (x != BotProperties.X || y != BotProperties.Y)
                    {
                        var bot = BotProperties.Bots.FirstOrDefault(b => b.X == x && b.Y == y);
                        if (bot != null)
                        {
                            botsInVicinity.Add(bot);
                        }
                    }
                }
            }
            return botsInVicinity;
        }
    }
}