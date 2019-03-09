using System;
using System.Collections.Generic;
using CSharpWars.Model.Interfaces;

namespace CSharpWars.Model
{
    public class Player : IHasId, IHasSysId
    {
        public Guid Id { get; set; }
        public Int32 SysId { get; set; }
        public String Name { get; set; }
        public String Secret { get; set; }
        public virtual ICollection<Bot> Bots { get; set; }
    }
}