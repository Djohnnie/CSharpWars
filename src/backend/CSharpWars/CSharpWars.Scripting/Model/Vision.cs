using System;
using System.Collections.Generic;
using CSharpWars.Enums;

namespace CSharpWars.Scripting.Model
{
    public class Vision
    {
        public IList<Bot> Bots { get; set; } = new List<Bot>();
        public IList<Bot> FriendlyBots { get; set; } = new List<Bot>();
        public IList<Bot> EnemyBots { get; set; } = new List<Bot>();

        private Vision() { }

        public static Vision Build(BotProperties botProperties)
        {
            var vision = new Vision();
            foreach (var bot in botProperties.Bots)
            {
                if (bot.Id != botProperties.BotId && BotIsVisible(bot, botProperties))
                {
                    vision.Bots.Add(bot);
                    if (bot.PlayerName == botProperties.PlayerName)
                    {
                        vision.FriendlyBots.Add(bot);
                    }
                    else
                    {
                        vision.EnemyBots.Add(bot);
                    }
                }
            }

            return vision;
        }

        private static Boolean BotIsVisible(Bot bot, BotProperties botProperties)
        {
            var result = false;

            switch (botProperties.Orientation)
            {
                case PossibleOrientations.North:
                    result = bot.Y < botProperties.Y;
                    break;
                case PossibleOrientations.East:
                    result = bot.X > botProperties.X;
                    break;
                case PossibleOrientations.South:
                    result = bot.Y > botProperties.Y;
                    break;
                case PossibleOrientations.West:
                    result = bot.X < botProperties.X;
                    break;
            }

            return result;
        }
    }
}