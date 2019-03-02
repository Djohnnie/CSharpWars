using CSharpWars.Enums;
using CSharpWars.Scripting;
using CSharpWars.Scripting.Model;

namespace CSharpWars.ScriptProcessor.Moves
{
    public class WalkForward : Move
    {
        public WalkForward(BotProperties botProperties) : base(botProperties)
        {
        }

        public override BotResult Go()
        {
            var botResult = BotResult.Build(BotProperties);
            var destinationX = BotProperties.X;
            var destinationY = BotProperties.Y;

            switch (BotProperties.Orientation)
            {
                case Orientations.North:
                    destinationY--;
                    break;
                case Orientations.East:
                    destinationX++;
                    break;
                case Orientations.South:
                    destinationY++;
                    break;
                case Orientations.West:
                    destinationX--;
                    break;
            }

            if (BotProperties.CurrentStamina - Constants.STAMINA_ON_MOVE >= 0 && !WillCollide(destinationX, destinationY))
            {
                botResult.CurrentStamina -= Constants.STAMINA_ON_MOVE;
                botResult.CurrentMove = Enums.Moves.WalkForward;
            }

            return botResult;
        }
    }
}