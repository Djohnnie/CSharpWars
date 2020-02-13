using System;
using System.Collections.Generic;
using CSharpWars.Model.Interfaces;

namespace CSharpWars.Model
{
    public class Player : IHasId
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Salt { get; set; }
        public string Hashed { get; set; }
        public DateTime LastDeployment { get; set; }
        public virtual ICollection<Bot> Bots { get; set; }
    }
}