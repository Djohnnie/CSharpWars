using System;
using System.Collections.Generic;

namespace CSharpWars.Web.Models
{
    public class PlayViewModel
    {
        public String HappyMessage { get; set; }
        public String SadMessage { get; set; }
        public String PlayerName { get; set; }
        public String BotName { get; set; }
        public Int32 BotHealth { get; set; }
        public Int32 BotStamina { get; set; }
        public String Script { get; set; }
        public Guid SelectedScript { get; set; }
        public List<ScriptViewModel> Scripts { get; set; }
    }
}