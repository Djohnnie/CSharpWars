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
            },
            new ScriptViewModel
            {
                Id = Guid.NewGuid(),
                Description = "Walk back and forth",
                Script = WalkBackAndForth
            },
            new ScriptViewModel
            {
                Id = Guid.NewGuid(),
                Description = "Look around and self destruct",
                Script = LookAroundAndSelfDestruct
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

        public const String WalkBackAndForth =
            "var step = LoadFromMemory<Int32>(\"STEP\");\r\n" +
            "if( step % 5 == 0 )\r\n" +
            "{\r\n" +
            "    TurnAround();\r\n" +
            "}\r\n" +
            "else\r\n" +
            "{\r\n" +
            "    WalkForward();\r\n" +
            "}\r\n" +
            "step++;\r\n" +
            "StoreInMemory<Int32>(\"STEP\", step);\r\n";

        public const String LookAroundAndSelfDestruct =
            "var selfDestructed = false;\r\n" +
            "foreach( var enemy in Vision.EnemyBots )\r\n" +
            "{\r\n" +
            "    var distanceX = Math.Abs(enemy.X - X);\r\n" +
            "    var distanceY = Math.Abs(enemy.Y - Y);\r\n" +
            "    if( distanceX < 4 && distanceY < 4 )\r\n" +
            "    {\r\n" +
            "        SelfDestruct();\r\n" +
            "        selfDestructed = true;\r\n" +
            "        break;\r\n" +
            "    }\r\n" +
            "}\r\n" +
            "\r\n" +
            "if( !selfDestructed )\r\n" +
            "{\r\n" +
            "    TurnLeft();\r\n" +
            "}\r\n";
    }
}