using CSharpWars.Scripting.Model;
using CSharpWars.ScriptProcessor.Middleware;

namespace CSharpWars.ScriptProcessor.Moves
{
    public class EmptyMove : Move
    {
        public EmptyMove(BotProperties botProperties) : base(botProperties) { }

        public override BotResult Go()
        {
            // Build result based on current properties.
            var botResult = BotResult.Build(BotProperties);

            botResult.Move = BotProperties.CurrentMove;

            return botResult;
        }
    }
}