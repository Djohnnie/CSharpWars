using System;
using System.Collections.Generic;

namespace CSharpWars.Model
{
    public class Player : ModelBase
    {
        public Int32 SysId { get; set; }
        public String Name { get; set; }
        public String Secret { get; set; }
        public virtual ICollection<Bot> Bots { get; set; }
    }
}