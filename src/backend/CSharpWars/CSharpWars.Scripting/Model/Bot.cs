using System;
using CSharpWars.DtoModel;
using CSharpWars.Enums;

namespace CSharpWars.Scripting.Model
{
    public class Bot
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PlayerName { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
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