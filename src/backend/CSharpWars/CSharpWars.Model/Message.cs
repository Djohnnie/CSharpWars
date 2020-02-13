using System;
using CSharpWars.Model.Interfaces;

namespace CSharpWars.Model
{
    public class Message : IHasId
    {
        public Guid Id { get; set; }

        public string BotName { get; set; }

        public string Content { get; set; }

        public DateTime DateTime { get; set; }
    }
}