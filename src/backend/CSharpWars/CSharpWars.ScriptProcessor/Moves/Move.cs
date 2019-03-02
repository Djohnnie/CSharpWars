using System;
using System.Collections.Generic;
using System.Linq;
using CSharpWars.DtoModel;
using CSharpWars.Enums;
using CSharpWars.Scripting.Model;

namespace CSharpWars.ScriptProcessor.Moves
{
    public abstract class Move
    {
        private static readonly Dictionary<Enums.Moves, Type> _moves = new Dictionary<Enums.Moves, Type>
        {
            {Enums.Moves.WalkForward, typeof(WalkForward)},
            {Enums.Moves.TurningLeft, typeof(TurnLeft)},
            {Enums.Moves.TurningRight, typeof(TurnRight)},
            {Enums.Moves.TurningAround, typeof(TurnAround)},
            {Enums.Moves.Teleport, typeof(Teleport)},
            {Enums.Moves.MeleeAttack, typeof(MeleeAttack)},
            {Enums.Moves.RangedAttack, typeof(RangedAttack)},
            {Enums.Moves.SelfDestruct, typeof(SelfDestruct)}
        };

        protected readonly BotProperties BotProperties;

        protected Move(BotProperties botProperties)
        {
            BotProperties = botProperties;
        }

        public static Move Build(BotProperties botProperties)
        {
            return (Move)Activator.CreateInstance(_moves[botProperties.CurrentMove]);
        }

        public abstract BotResult Go();

        protected Boolean WillCollide()
        {
            var collidingEdge = false;
            Bot collidingBot = null;
            switch (BotProperties.Orientation)
            {
                case Orientations.North:
                    if (BotProperties.Y == 0) collidingEdge = true;
                    collidingBot = BotProperties.Bots.SingleOrDefault(b => b.X == BotProperties.X && b.Y == BotProperties.Y - 1);
                    break;
                case Orientations.East:
                    if (BotProperties.X == BotProperties.Width - 1) collidingEdge = true;
                    collidingBot = BotProperties.Bots.SingleOrDefault(b => b.X == BotProperties.X + 1 && b.Y == BotProperties.Y);
                    break;
                case Orientations.South:
                    if (BotProperties.Y == BotProperties.Height - 1) collidingEdge = true;
                    collidingBot = BotProperties.Bots.SingleOrDefault(b => b.X == BotProperties.X && b.Y == BotProperties.Y + 1);
                    break;
                case Orientations.West:
                    if (BotProperties.X == 0) collidingEdge = true;
                    collidingBot = BotProperties.Bots.SingleOrDefault(b => b.X == BotProperties.X - 1 && b.Y == BotProperties.Y);
                    break;
            }
            return collidingBot != null || collidingEdge;
        }

        protected Boolean WillCollide(Int32 x, Int32 y)
        {
            var collidingEdge = x < 0 || x >= BotProperties.Width || y < 0 || y >= BotProperties.Height;
            var collidingBot = BotProperties.Bots.SingleOrDefault(b => b.X == x && b.Y == y);
            return collidingBot != null || collidingEdge;
        }
    }
}