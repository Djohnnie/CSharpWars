using System;

namespace CSharpWars.DtoModel
{
    public class MessageToCreateDto
    {
        public string BotName { get; set; }

        public string Content { get; set; }

        public DateTime DateTime { get; set; }
    }
}