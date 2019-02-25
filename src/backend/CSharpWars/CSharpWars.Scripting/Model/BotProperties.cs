using System;
using System.Collections.Generic;
using CSharpWars.DtoModel;
using CSharpWars.Enums;

namespace CSharpWars.Scripting.Model
{
    public class BotProperties
    {
        public int Width { get; }
        public Int32 Height { get; }
        public Int32 X { get; }
        public Int32 Y { get; }
        public Orientation Orientation { get; }
        public Move LastMove { get; }
        public Int32 MaximumPhysicalHealth { get; }
        public Int32 CurrentPhysicalHealth { get; }
        public Int32 MaximumStamina { get; }
        public Int32 CurrentStamina { get; }
        public Dictionary<String, String> Memory { get; }
        public List<String> Messages { get; }

        public Move CurrentMove { get; set; }


        public BotProperties(int width, int height, int x, int y, Orientation orientation, Move lastMove, int maximumPhysicalHealth,
            int currentPhysicalHealth, int maximumStamina, int currentStamina, Dictionary<string, string> memory, List<string> messages)
        {
            Width = width;
            Height = height;
            X = x;
            Y = y;
            Orientation = orientation;
            LastMove = lastMove;
            MaximumPhysicalHealth = maximumPhysicalHealth;
            CurrentPhysicalHealth = currentPhysicalHealth;
            MaximumStamina = maximumStamina;
            CurrentStamina = currentStamina;
            Memory = memory;
            Messages = messages;
        }

        public static BotProperties Build(BotDto bot, ArenaDto arena)
        {
            return new BotProperties(
                arena.Width,
                arena.Height,
                bot.LocationX,
                bot.LocationY,
                bot.Orientation,
                bot.PreviousMove,
                bot.MaximumHealth,
                bot.CurrentHealth,
                bot.MaximumStamina,
                bot.CurrentStamina,
                new Dictionary<string, string>(),
                new List<string>());
        }
    }
}