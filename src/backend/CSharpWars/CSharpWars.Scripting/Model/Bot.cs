using System;
using CSharpWars.Enums;

namespace CSharpWars.Scripting.Model
{
    public class Bot
    {
        public Int32 X { get; set; }
        public Int32 Y { get; set; }
        public Orientations Orientation { get; set; }
        public String Name { get; set; }
        public Boolean Friendly { get; set; }
    }
}