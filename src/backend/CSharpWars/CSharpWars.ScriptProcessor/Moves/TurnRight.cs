using CSharpWars.Scripting.Model;

namespace CSharpWars.ScriptProcessor.Moves
{
    public class TurnRight : Move
    {
        public TurnRight(BotProperties botProperties) : base(botProperties)
        {
        }

        public override BotResult Go()
        {
            if (BotProperties.CurrentStamina > 0)
            {

            }

            return new BotResult();
        }
    }
}