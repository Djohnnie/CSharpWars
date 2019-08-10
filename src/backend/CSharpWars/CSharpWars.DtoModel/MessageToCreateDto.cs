using System;

namespace CSharpWars.DtoModel
{
    public class MessageToCreateDto
    {
        public String BotName { get; set; }

        public String Content { get; set; }

        public DateTime DateTime { get; set; }
    }
}