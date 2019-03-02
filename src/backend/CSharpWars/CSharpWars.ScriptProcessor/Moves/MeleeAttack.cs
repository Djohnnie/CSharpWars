using CSharpWars.Scripting.Model;

namespace CSharpWars.ScriptProcessor.Moves
{
    public class MeleeAttack : Move
    {
        public MeleeAttack(BotProperties botProperties) : base(botProperties)
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