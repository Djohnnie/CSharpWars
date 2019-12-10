using System;
using System.Collections.Generic;
using CSharpWars.Enums;
using CSharpWars.Scripting.Model;

namespace CSharpWars.Processor.Middleware
{
    public class BotResult
    {
        private readonly Dictionary<Guid, Int32> _damageInflicted = new Dictionary<Guid, Int32>();
        private readonly Dictionary<Guid, (Int32 X, Int32 Y)> _teleportations = new Dictionary<Guid, (Int32 X, Int32 Y)>();

        public Int32 X { get; set; }
        public Int32 Y { get; set; }
        public PossibleOrientations Orientation { get; set; }
        public Int32 CurrentHealth { get; set; }
        public Int32 CurrentStamina { get; set; }
        public Dictionary<String, String> Memory { get; set; }
        public List<String> Messages { get; set; }
        public PossibleMoves Move { get; set; }
        public Int32 LastAttackX { get; set; }
        public Int32 LastAttackY { get; set; }

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

        public void InflictDamage(Guid botId, Int32 damage)
        {
            if (!_damageInflicted.ContainsKey(botId))
            {
                _damageInflicted.Add(botId, 0);
            }

            _damageInflicted[botId] += damage;
        }

        public Int32 GetInflictedDamage(Guid botId)
        {
            return _damageInflicted.ContainsKey(botId) ? _damageInflicted[botId] : 0;
        }

        public void Teleport(Guid botId, Int32 destinationX, Int32 destinationY)
        {
            if (!_teleportations.ContainsKey(botId))
            {
                _teleportations.Add(botId, (destinationX, destinationY));
            }

            _teleportations[botId] = (destinationX, destinationY);
        }

        public (Int32 X, Int32 Y) GetTeleportation(Guid botId)
        {
            return _teleportations.ContainsKey(botId) ? _teleportations[botId] : (-1, -1);
        }
    }
}