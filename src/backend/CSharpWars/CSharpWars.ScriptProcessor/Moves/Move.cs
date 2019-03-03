using System;
using System.Collections.Generic;
using System.Linq;
using CSharpWars.Enums;
using CSharpWars.Scripting.Model;

namespace CSharpWars.ScriptProcessor.Moves
{

    public abstract class Move
    {
        private static readonly Dictionary<PossibleMoves, Func<BotProperties, Move>> _moves = new Dictionary<PossibleMoves, Func<BotProperties, Move>>
        {
            { PossibleMoves.WalkForward, p => new WalkForward(p) },
            { PossibleMoves.TurningLeft, p => new TurnLeft(p) },
            { PossibleMoves.TurningRight, p => new TurnRight(p) },
            { PossibleMoves.TurningAround, p => new TurnAround(p) },
            { PossibleMoves.Teleport, p => new Teleport(p) },
            { PossibleMoves.MeleeAttack, p => new MeleeAttack(p) },
            { PossibleMoves.RangedAttack, p => new RangedAttack(p) },
            { PossibleMoves.SelfDestruct, p => new SelfDestruct(p) }
        };

        protected readonly BotProperties BotProperties;

        protected Move(BotProperties botProperties)
        {
            BotProperties = botProperties;
        }

        public static Move Build(BotProperties botProperties)
        {
            return _moves[botProperties.CurrentMove](botProperties);
        }

        public abstract BotResult Go();

        protected Boolean WillCollide(Int32 x, Int32 y)
        {
            var collidingEdge = x < 0 || x >= BotProperties.Width || y < 0 || y >= BotProperties.Height;
            var collidingBot = BotProperties.Bots.SingleOrDefault(b => b.X == x && b.Y == y);
            return collidingBot != null || collidingEdge;
        }
    }
}