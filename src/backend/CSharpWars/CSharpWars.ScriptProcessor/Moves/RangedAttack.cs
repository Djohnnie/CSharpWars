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
            if (BotProperties.CurrentStamina > 0)
            {

            }

            return new BotResult();
        }
    }
}