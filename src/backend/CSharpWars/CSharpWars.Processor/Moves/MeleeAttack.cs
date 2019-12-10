using System;
using System.Linq;
using CSharpWars.Enums;
using CSharpWars.Processor.Middleware;
using CSharpWars.Scripting;
using CSharpWars.Scripting.Model;

namespace CSharpWars.Processor.Moves
{
    public class MeleeAttack : Move
    {
        public MeleeAttack(BotProperties botProperties) : base(botProperties) { }

        public override BotResult Go()
        {
            // Build result based on current properties.
            var botResult = BotResult.Build(BotProperties);

            var victimizedBot = FindVictimizedBot();
            if (victimizedBot != null)
            {
                var backstab = victimizedBot.Orientation == BotProperties.Orientation;
                botResult.InflictDamage(victimizedBot.Id, backstab ? Constants.MELEE_BACKSTAB_DAMAGE : Constants.MELEE_DAMAGE);
            }

            botResult.Move = PossibleMoves.MeleeAttack;

            return botResult;
        }

        private Bot FindVictimizedBot()
        {
            var neighbourLocation = GetNeighbourLocation();
            return BotProperties.Bots.SingleOrDefault(bot => bot.X == neighbourLocation.X && bot.Y == neighbourLocation.Y);
        }

        private (Int32 X, Int32 Y) GetNeighbourLocation()
        {
            switch (BotProperties.Orientation)
            {
                case PossibleOrientations.North:
                    return (BotProperties.X, BotProperties.Y - 1);
                case PossibleOrientations.East:
                    return (BotProperties.X + 1, BotProperties.Y);
                case PossibleOrientations.South:
                    return (BotProperties.X, BotProperties.Y + 1);
                case PossibleOrientations.West:
                    return (BotProperties.X - 1, BotProperties.Y);
            }

            return (-1, -1);
        }
    }
}