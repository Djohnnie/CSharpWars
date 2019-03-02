using System;
using System.Collections.Generic;
using System.Linq;
using CSharpWars.Scripting.Model;

namespace CSharpWars.ScriptProcessor.Moves
{

    public abstract class Move
    {
        private static readonly Dictionary<Enums.Moves, Func<BotProperties, Move>> _moves = new Dictionary<Enums.Moves, Func<BotProperties, Move>>
        {
            { Enums.Moves.WalkForward, p => new WalkForward(p) },
            { Enums.Moves.TurningLeft, p => new TurnLeft(p) },
            { Enums.Moves.TurningRight, p => new TurnRight(p) },
            { Enums.Moves.TurningAround, p => new TurnAround(p) },
            { Enums.Moves.Teleport, p => new Teleport(p) },
            { Enums.Moves.MeleeAttack, p => new MeleeAttack(p) },
            { Enums.Moves.RangedAttack, p => new RangedAttack(p) },
            { Enums.Moves.SelfDestruct, p => new SelfDestruct(p) }
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