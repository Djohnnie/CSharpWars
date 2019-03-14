using System;

namespace CSharpWars.DtoModel
{
    public class BotToCreateDto
    {
        public Guid PlayerId { get; set; }
        public String Name { get; set; }
        public Int32 MaximumHealth { get; set; }
        public Int32 MaximumStamina { get; set; }
        public String Script { get; set; }
    }
}