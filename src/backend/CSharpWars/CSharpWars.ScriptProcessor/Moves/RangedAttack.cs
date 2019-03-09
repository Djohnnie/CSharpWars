using CSharpWars.Enums;
using CSharpWars.Scripting.Model;

namespace CSharpWars.ScriptProcessor.Moves
{
    public class RangedAttack : Move
    {
        public RangedAttack(BotProperties botProperties) : base(botProperties)
        {
        }

        public override BotResult Go()
        {
            // Build result based on current properties.
            var botResult = BotResult.Build(BotProperties);

            botResult.CurrentMove = PossibleMoves.RangedAttack;

            return botResult;
        }
    }
}