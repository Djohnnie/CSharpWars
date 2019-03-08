using System.Linq;
using CSharpWars.Enums;
using CSharpWars.Scripting.Model;

namespace CSharpWars.ScriptProcessor.Moves
{
    public class MeleeAttack : Move
    {
        public MeleeAttack(BotProperties botProperties) : base(botProperties) { }

        public override BotResult Go()
        {
            // Build result based on current properties.
            var botResult = BotResult.Build(BotProperties);
            botResult.CurrentMove = PossibleMoves.MeleeAttack;

            var victimizedBot = FindVictimizedBot(botResult.X, botResult.Y);
            if (victimizedBot != null)
            {
                
            }

            return botResult;
        }

        private Bot FindVictimizedBot(int x, int y)
        {
            return BotProperties.Bots.SingleOrDefault(bot => bot.X == x && bot.Y == y);
        }
    }
}