using CSharpWars.Scripting.Model;

namespace CSharpWars.ScriptProcessor.Moves
{
    public class TurnLeft : Move
    {
        public TurnLeft(BotProperties botProperties) : base(botProperties)
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