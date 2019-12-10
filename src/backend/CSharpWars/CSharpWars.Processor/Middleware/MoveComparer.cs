using System;
using System.Collections.Generic;
using CSharpWars.Enums;

namespace CSharpWars.Processor.Middleware
{
    public class MoveComparer : IComparer<PossibleMoves>
    {
        private readonly Dictionary<PossibleMoves, Int32> _weights = new Dictionary<PossibleMoves, Int32>
        {
            { PossibleMoves.Idling, 0 },
            { PossibleMoves.Died, 0 },
            { PossibleMoves.ScriptError, 0 },
            { PossibleMoves.RangedAttack, 1 },
            { PossibleMoves.MeleeAttack, 2 },
            { PossibleMoves.SelfDestruct, 3 },
            { PossibleMoves.Teleport, 4 },
            { PossibleMoves.WalkForward, 5 },
            { PossibleMoves.TurningLeft, 6 },
            { PossibleMoves.TurningRight, 6 },
            { PossibleMoves.TurningAround, 6 }
        };

        public Int32 Compare(PossibleMoves x, PossibleMoves y)
        {
            return _weights[x].CompareTo(_weights[y]);
        }
    }
}