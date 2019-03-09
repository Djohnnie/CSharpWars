using CSharpWars.Enums;
using CSharpWars.Scripting.Model;

namespace CSharpWars.ScriptProcessor.Moves
{
    public class Teleport : Move
    {
        public Teleport(BotProperties botProperties) : base(botProperties)
        {
        }

        public override BotResult Go()
        {
            // Build result based on current properties.
            var botResult = BotResult.Build(BotProperties);

            botResult.CurrentMove = PossibleMoves.Teleport;

            return botResult;
        }
    }
}