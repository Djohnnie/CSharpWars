using System;
using System.Collections.Generic;
using CSharpWars.Enums;
using CSharpWars.Scripting.Model;

namespace CSharpWars.ScriptProcessor
{
    public class BotResult
    {
        public Int32 X { get; set; }
        public Int32 Y { get; set; }
        public PossibleOrientations Orientation { get; set; }
        public Int32 CurrentHealth { get; set; }
        public Int32 CurrentStamina { get; set; }
        public Dictionary<String, String> Memory { get; set; }
        public List<String> Messages { get; set; }
        public PossibleMoves CurrentMove { get; set; }

        public static BotResult Build(BotProperties botProperties)
        {
            return new BotResult
            {
                X = botProperties.X,
                Y = botProperties.Y,
                Orientation = botProperties.Orientation,
                CurrentStamina = botProperties.CurrentStamina,
                CurrentHealth = botProperties.CurrentHealth,
                Memory = botProperties.Memory,
                Messages = botProperties.Messages,
                CurrentMove = PossibleMoves.Idling
            };
        }
    }
}