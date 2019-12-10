using CSharpWars.Enums;
using CSharpWars.Processor.Middleware;
using CSharpWars.Scripting;
using CSharpWars.Scripting.Model;

namespace CSharpWars.Processor.Moves
{
    /// <summary>
    /// Class containing logic for walking forward.
    /// </summary>
    /// <remarks>
    /// Performing this move makes the robot walk forward in the direction he is currently oriented.
    /// This move consumes one stamina point.
    /// </remarks>
    public class WalkForward : Move
    {
        public WalkForward(BotProperties botProperties) : base(botProperties) { }

        public override BotResult Go()
        {
            // Build result based on current properties.
            var botResult = BotResult.Build(BotProperties);

            // Only perform move if enough stamina is available.
            if (BotProperties.CurrentStamina - Constants.STAMINA_ON_MOVE >= 0)
            {
                var destinationX = BotProperties.X;
                var destinationY = BotProperties.Y;

                switch (BotProperties.Orientation)
                {
                    case PossibleOrientations.North:
                        destinationY--;
                        break;
                    case PossibleOrientations.East:
                        destinationX++;
                        break;
                    case PossibleOrientations.South:
                        destinationY++;
                        break;
                    case PossibleOrientations.West:
                        destinationX--;
                        break;
                }

                if (!WillCollide(destinationX, destinationY))
                {
                    botResult.CurrentStamina -= Constants.STAMINA_ON_MOVE;
                    botResult.Move = PossibleMoves.WalkForward;
                    botResult.X = destinationX;
                    botResult.Y = destinationY;
                }
            }

            return botResult;
        }
    }
}