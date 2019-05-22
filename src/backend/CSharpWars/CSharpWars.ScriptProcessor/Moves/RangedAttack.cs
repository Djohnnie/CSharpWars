using System.Linq;
using CSharpWars.Enums;
using CSharpWars.Scripting;
using CSharpWars.Scripting.Model;
using CSharpWars.ScriptProcessor.Middleware;

namespace CSharpWars.ScriptProcessor.Moves
{
    public class RangedAttack : Move
    {
        public RangedAttack(BotProperties botProperties) : base(botProperties) { }

        public override BotResult Go()
        {
            // Build result based on current properties.
            var botResult = BotResult.Build(BotProperties);

            var victimizedBot = FindVictimizedBot();
            if (victimizedBot != null)
            {
                botResult.InflictDamage(victimizedBot.Id, Constants.RANGED_DAMAGE);
            }

            botResult.Move = PossibleMoves.RangedAttack;

            return botResult;
        }

        private Bot FindVictimizedBot()
        {
            return BotProperties.Bots.FirstOrDefault(bot => bot.X == BotProperties.MoveDestinationX && bot.Y == BotProperties.MoveDestinationY);
        }
    }
}