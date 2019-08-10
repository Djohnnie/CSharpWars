using System;

namespace CSharpWars.DtoModel
{
    public class MessageToCreateDto
    {
        public Guid BotId { get; set; }

        public String Content { get; set; }

        public DateTime DateTime { get; set; }
    }
}