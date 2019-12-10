using System;
using System.Collections.Generic;
using System.Linq;
using CSharpWars.Common.Helpers.Interfaces;
using CSharpWars.Enums;
using CSharpWars.Processor.Middleware;
using CSharpWars.Scripting.Model;

namespace CSharpWars.Processor.Moves
{

    public abstract class Move
    {
        private static readonly Dictionary<PossibleMoves, Func<BotProperties, IRandomHelper, Move>> _moves = new Dictionary<PossibleMoves, Func<BotProperties, IRandomHelper, Move>>
        {
            { PossibleMoves.WalkForward, (p, rh) => new WalkForward(p) },
            { PossibleMoves.TurningLeft, (p, rh) => new TurnLeft(p) },
            { PossibleMoves.TurningRight, (p, rh) => new TurnRight(p) },
            { PossibleMoves.TurningAround, (p, rh) => new TurnAround(p) },
            { PossibleMoves.Teleport, (p, rh) => new Teleport(p, rh) },
            { PossibleMoves.MeleeAttack, (p, rh) => new MeleeAttack(p) },
            { PossibleMoves.RangedAttack, (p, rh) => new RangedAttack(p) },
            { PossibleMoves.SelfDestruct, (p, rh) => new SelfDestruct(p) },
            { PossibleMoves.Idling, (p, rh) => new EmptyMove(p) },
            { PossibleMoves.ScriptError, (p, rh) => new EmptyMove(p) },
        };

        protected readonly BotProperties BotProperties;

        protected Move(BotProperties botProperties)
        {
            BotProperties = botProperties;
        }

        public static Move Build(BotProperties botProperties, IRandomHelper randomHelper)
        {
            return _moves[botProperties.CurrentMove](botProperties, randomHelper);
        }

        public abstract BotResult Go();

        protected Boolean WillCollide(Int32 x, Int32 y)
        {
            var collidingEdge = x < 0 || x >= BotProperties.Width || y < 0 || y >= BotProperties.Height;
            var collidingBot = BotProperties.Bots.FirstOrDefault(b => b.X == x && b.Y == y);
            return collidingBot != null || collidingEdge;
        }
    }
}