using CSharpWars.Enums;
using CSharpWars.Processor.Middleware;
using CSharpWars.Scripting.Model;

namespace CSharpWars.Processor.Moves
{
    /// <summary>
    /// Class containing logic for turning around.
    /// </summary>
    /// <remarks>
    /// Performing this move makes the robot turn 180°.
    /// This move does not consume stamina because the robot will not move away from its current location in the arena grid.
    /// </remarks>
    public class TurnAround : Move
    {
        public TurnAround(BotProperties botProperties) : base(botProperties) { }

        public override BotResult Go()
        {
            // Build result based on current properties.
            var botResult = BotResult.Build(BotProperties);

            botResult.Move = PossibleMoves.TurningAround;

            switch (BotProperties.Orientation)
            {
                case PossibleOrientations.North:
                    botResult.Orientation = PossibleOrientations.South;
                    break;
                case PossibleOrientations.East:
                    botResult.Orientation = PossibleOrientations.West;
                    break;
                case PossibleOrientations.South:
                    botResult.Orientation = PossibleOrientations.North;
                    break;
                case PossibleOrientations.West:
                    botResult.Orientation = PossibleOrientations.East;
                    break;
            }

            return botResult;
        }
    }
}