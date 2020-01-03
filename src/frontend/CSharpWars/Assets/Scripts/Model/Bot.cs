using System;

namespace Assets.Scripts.Model
{
    public class Bot
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PlayerName { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int FromX { get; set; }
        public int FromY { get; set; }
        public PossibleOrientations Orientation { get; set; }
        public int MaximumHealth { get; set; }
        public int CurrentHealth { get; set; }
        public int MaximumStamina { get; set; }
        public int CurrentStamina { get; set; }
        public PossibleMoves Move { get; set; }
        public int LastAttackX { get; set; }
        public int LastAttackY { get; set; }
    }
}