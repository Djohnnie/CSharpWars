using System;
using CSharpWars.Model.Interfaces;

namespace CSharpWars.Model
{
    public class BotScript : IHasId
    {
        public Guid Id { get; set; }
        public String Script { get; set; }
    }
}