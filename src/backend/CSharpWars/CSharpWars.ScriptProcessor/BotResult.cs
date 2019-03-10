using System;
using System.Collections.Generic;
using CSharpWars.Enums;
using CSharpWars.Scripting.Model;

namespace CSharpWars.ScriptProcessor
{
    public class BotResult
    {
        private readonly Dictionary<Guid, Int32> _damageInflicted = new Dictionary<Guid, Int32>();

        public Int32 X { get; set; }
        public Int32 Y { get; set; }
        public PossibleOrientations Orientation { get; set; }
        public Int32 CurrentHealth { get; set; }
        public Int32 CurrentStamina { get; set; }
        public Dictionary<String, String> Memory { get; set; }
        public List<String> Messages { get; set; }
        public PossibleMoves Move { get; set; }

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
                Move = PossibleMoves.Idling
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
    }
}