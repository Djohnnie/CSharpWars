using System;
using CSharpWars.Enums;

namespace CSharpWars.Model
{
    public class Bot : ModelBase
    {
        public Int32 SysId { get; set; }
        public String Name { get; set; }
        public Int32 LocationX { get; set; }
        public Int32 LocationY { get; set; }
        public PossibleOrientations Orientation { get; set; }
        public Int32 MaximumHealth { get; set; }
        public Int32 CurrentHealth { get; set; }
        public Int32 MaximumStamina { get; set; }
        public Int32 CurrentStamina { get; set; }
        public String Script { get; set; }
        public String Memory { get; set; }
        public PossibleMoves PreviousMove { get; set; }
        public virtual Player Team { get; set; }
    }
}