using System;

namespace CSharpWars.Web.Models
{
    public class GameViewModel
    {
        public String PlayerName { get; set; }
        public String BotName { get; set; }
        public Int32 BotHealth { get; set; }
        public Int32 BotStamina { get; set; }
        public String BotScript { get; set; }
    }
}