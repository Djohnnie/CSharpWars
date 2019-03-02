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
        public Orientations Orientation { get; set; }
        public Int32 CurrentPhysicalHealth { get; set; }
        public Int32 CurrentStamina { get; set; }
        public Dictionary<String, String> Memory { get; set; }
        public List<String> Messages { get; set; }
        public Enums.Moves CurrentMove { get; set; }

        public static BotResult Build(BotProperties botProperties)
        {
            return new BotResult
            {
                X = botProperties.X,
                Y = botProperties.Y,
                Orientation = botProperties.Orientation,
                CurrentStamina = botProperties.CurrentStamina,
                CurrentPhysicalHealth = botProperties.CurrentPhysicalHealth,
                Memory = botProperties.Memory,
                Messages = botProperties.Messages,
                CurrentMove = Enums.Moves.Idling
            };
        }
    }
}