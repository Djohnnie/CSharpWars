using System;
using System.Collections.Generic;
using CSharpWars.Enums;
using CSharpWars.Scripting.Model;

namespace CSharpWars.Processor.Middleware
{
    public class BotResult
    {
        private readonly Dictionary<Guid, int> _damageInflicted = new Dictionary<Guid, int>();
        private readonly Dictionary<Guid, (int X, int Y)> _teleportations = new Dictionary<Guid, (int X, int Y)>();

        public int X { get; set; }
        public int Y { get; set; }
        public PossibleOrientations Orientation { get; set; }
        public int CurrentHealth { get; set; }
        public int CurrentStamina { get; set; }
        public Dictionary<string, string> Memory { get; set; }
        public List<string> Messages { get; set; }
        public PossibleMoves Move { get; set; }
        public int LastAttackX { get; set; }
        public int LastAttackY { get; set; }

        private BotResult() { }

        public static BotResult Build(BotProperties botProperties)
        {
            return new BotResult
            {
                X = botProperties.X,
                Y = botProperties.Y,
                Orientation = botProperties.Orientation,
                CurrentHealth = botProperties.CurrentHealth,
                CurrentStamina = botProperties.CurrentStamina,
                Memory = botProperties.Memory,
                Messages = botProperties.Messages,
                Move = PossibleMoves.Idling,
                LastAttackX = botProperties.MoveDestinationX,
                LastAttackY = botProperties.MoveDestinationY
            };
        }

        public void InflictDamage(Guid botId, int damage)
        {
            if (!_damageInflicted.ContainsKey(botId))
            {
                _damageInflicted.Add(botId, 0);
            }

            _damageInflicted[botId] += damage;
        }

        public int GetInflictedDamage(Guid botId)
        {
            return _damageInflicted.ContainsKey(botId) ? _damageInflicted[botId] : 0;
        }

        public void Teleport(Guid botId, int destinationX, int destinationY)
        {
            if (!_teleportations.ContainsKey(botId))
            {
                _teleportations.Add(botId, (destinationX, destinationY));
            }

            _teleportations[botId] = (destinationX, destinationY);
        }

        public (int X, int Y) GetTeleportation(Guid botId)
        {
            return _teleportations.ContainsKey(botId) ? _teleportations[botId] : (-1, -1);
        }
    }
}