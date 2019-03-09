using CSharpWars.Scripting.Model;

namespace CSharpWars.ScriptProcessor.Moves
{
    public class EmptyMove : Move
    {
        public EmptyMove(BotProperties botProperties) : base(botProperties) { }

        public override BotResult Go()
        {
            // Build result based on current properties.
            var botResult = BotResult.Build(BotProperties);

            botResult.CurrentMove = BotProperties.CurrentMove;

            return botResult;
        }
    }
}