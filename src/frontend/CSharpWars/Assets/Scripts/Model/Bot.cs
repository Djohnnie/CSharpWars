using System;

namespace Assets.Scripts.Model
{
    public class Bot
    {
        public Guid Id { get; set; }
        public Int32 LocationX { get; set; }
        public Int32 LocationY { get; set; }
        public PossibleOrientations Orientation { get; set; }
    }
}