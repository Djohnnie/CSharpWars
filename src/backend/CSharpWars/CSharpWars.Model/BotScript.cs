using System;
using CSharpWars.Model.Interfaces;

namespace CSharpWars.Model
{
    public class BotScript : IHasId
    {
        public Guid Id { get; set; }
        public string Script { get; set; }
    }
}