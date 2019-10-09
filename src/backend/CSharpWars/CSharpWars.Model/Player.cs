using System;
using System.Collections.Generic;
using CSharpWars.Model.Interfaces;

namespace CSharpWars.Model
{
    public class Player : IHasId
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public String Salt { get; set; }
        public String Hashed { get; set; }
        public DateTime LastDeployment { get; set; }
        public virtual ICollection<Bot> Bots { get; set; }
    }
}