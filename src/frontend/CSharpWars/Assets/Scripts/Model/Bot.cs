using System;

namespace Assets.Scripts.Model
{
    public class Bot
    {
        public Guid Id { get; set; }
        public Int32 X { get; set; }
        public Int32 Y { get; set; }
        public PossibleOrientations Orientation { get; set; }
        public Int32 MaximumHealth { get; set; }
        public Int32 CurrentHealth { get; set; }
        public PossibleMoves Move { get; set; }
    }
}