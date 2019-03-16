using System;
using CSharpWars.DtoModel;
using CSharpWars.Enums;

namespace CSharpWars.Scripting.Model
{
    public class Bot
    {
        public Guid Id { get; set; }
        public Int32 X { get; set; }
        public Int32 Y { get; set; }
        public PossibleOrientations Orientation { get; set; }
        public String Name { get; set; }
        public Boolean Friendly { get; set; }

        public void Update(BotDto bot)
        {
            X = bot.X;
            Y = bot.Y;
            Orientation = bot.Orientation;
        }
    }
}