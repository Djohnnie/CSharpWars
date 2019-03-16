using System;
using CSharpWars.DtoModel;
using CSharpWars.Enums;

namespace CSharpWars.Scripting.Model
{
    public class Bot
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public String PlayerName { get; set; }
        public Int32 X { get; set; }
        public Int32 Y { get; set; }
        public PossibleOrientations Orientation { get; set; }

        private Bot() { }

        public static Bot Build(BotDto bot)
        {
            return new Bot
            {
                Id = bot.Id,
                Name = bot.Name,
                PlayerName = bot.PlayerName,
                X = bot.X,
                Y = bot.Y,
                Orientation = bot.Orientation
            };
        }

        public void Update(BotDto bot)
        {
            X = bot.X;
            Y = bot.Y;
            Orientation = bot.Orientation;
        }
    }
}