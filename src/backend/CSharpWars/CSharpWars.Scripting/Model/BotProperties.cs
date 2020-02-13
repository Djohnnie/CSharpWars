using System;
using System.Collections.Generic;
using System.Linq;
using CSharpWars.Common.Extensions;
using CSharpWars.DtoModel;
using CSharpWars.Enums;

namespace CSharpWars.Scripting.Model
{
    public class BotProperties
    {
        public Guid BotId { get; private set; }
        public string Name { get; private set; }
        public string PlayerName { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public PossibleOrientations Orientation { get; private set; }
        public PossibleMoves LastMove { get; private set; }
        public int MaximumHealth { get; private set; }
        public int CurrentHealth { get; private set; }
        public int MaximumStamina { get; private set; }
        public int CurrentStamina { get; private set; }
        public Dictionary<string, string> Memory { get; private set; }
        public List<string> Messages { get; private set; }
        public List<Bot> Bots { get; set; }
        public PossibleMoves CurrentMove { get; set; }
        public int MoveDestinationX { get; set; }
        public int MoveDestinationY { get; set; }

        private BotProperties() { }

        public void Update(BotDto bot)
        {
            CurrentHealth = bot.CurrentHealth;
            X = bot.X;
            Y = bot.Y;
        }

        public static BotProperties Build(BotDto bot, ArenaDto arena, IList<BotDto> bots)
        {
            return new BotProperties
            {
                BotId = bot.Id,
                Name = bot.Name,
                PlayerName = bot.PlayerName,
                Width = arena.Width,
                Height = arena.Height,
                X = bot.X,
                Y = bot.Y,
                Orientation = bot.Orientation,
                LastMove = bot.Move,
                MaximumHealth = bot.MaximumHealth,
                CurrentHealth = bot.CurrentHealth,
                MaximumStamina = bot.MaximumStamina,
                CurrentStamina = bot.CurrentStamina,
                Memory = bot.Memory.Deserialize<Dictionary<string, string>>(),
                Messages = new List<string>(),
                Bots = BuildBots(bots),
                CurrentMove = PossibleMoves.Idling
            };
        }

        private static List<Bot> BuildBots(IList<BotDto> bots)
        {
            return bots.Select(Bot.Build).ToList();
        }
    }
}