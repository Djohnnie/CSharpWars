using System;
using CSharpWars.Model.Interfaces;

namespace CSharpWars.Model
{
    public class Template : IHasId
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Script { get; set; }
    }
}