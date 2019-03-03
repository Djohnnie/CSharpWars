using System;
using System.Collections.Generic;

namespace CSharpWars.Model
{
    public class Player : ModelBase
    {
        public String Name { get; set; }
        public String Secret { get; set; }
        public virtual ICollection<Bot> Bots { get; set; }
    }
}