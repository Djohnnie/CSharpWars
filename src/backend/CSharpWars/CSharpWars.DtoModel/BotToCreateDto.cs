using System;

namespace CSharpWars.DtoModel
{
    public class BotToCreateDto
    {
        public Guid PlayerId { get; set; }
        public string Name { get; set; }
        public int MaximumHealth { get; set; }
        public int MaximumStamina { get; set; }
        public string Script { get; set; }
    }
}