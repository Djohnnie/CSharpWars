using System;
using CSharpWars.Model.Interfaces;

namespace CSharpWars.Model
{
    public class Message : IHasId
    {
        public Guid Id { get; set; }

        public String Content { get; set; }

        public DateTime DateTime { get; set; }

        public Bot Bot { get; set; }
    }
}