using System;
using System.Collections.Generic;
using CSharpWars.Web.Models;

namespace CSharpWars.Web.Constants
{
    public static class BotScripts
    {
        public static List<ScriptViewModel> All = new List<ScriptViewModel>
        {
            new ScriptViewModel
            {
                Id = Guid.NewGuid(),
                Description = "Walk around in circles",
                Script = WalkAround
            }
        };

        public const String WalkAround =
            "var step = LoadFromMemory<Int32>(\"STEP\");\r\n" +
            "if( step % 3 == 0 )\r\n" +
            "{\r\n" +
            "    TurnLeft();\r\n" +
            "}\r\n" +
            "else\r\n" +
            "{\r\n" +
            "    WalkForward();\r\n" +
            "}\r\n" +
            "step++;\r\n" +
            "StoreInMemory<Int32>(\"STEP\", step);\r\n";
    }
}