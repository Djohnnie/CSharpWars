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
            },
            new ScriptViewModel
            {
                Id = Guid.NewGuid(),
                Description = "Look around and range attack",
                Script = LookAroundAndRangeAttack
            },
            new ScriptViewModel
            {
                Id = Guid.NewGuid(),
                Description = "Teleport around at random",
                Script = TeleportAround
            },
            new ScriptViewModel
            {
                Id = Guid.NewGuid(),
                Description = "Hunt down other robots",
                Script = HuntDown
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

        public const String LookAroundAndRangeAttack =
            "var attacked = false;\r\n" +
            "foreach( var enemy in Vision.EnemyBots )\r\n" +
            "{\r\n" +
            "    RangedAttack( enemy.X , enemy.Y );\r\n" +
            "    attacked = true;\r\n" +
            "    break;\r\n" +
            "}\r\n" +
            "\r\n" +
            "if( !attacked )\r\n" +
            "{\r\n" +
            "    TurnLeft();\r\n" +
            "}\r\n";

        public const String TeleportAround =
            "var r = new Random();\r\n" +
            "var destinationX = r.Next(0, Width);\r\n" +
            "var destinationY = r.Next(0, Height);\r\n" +
            "Teleport( destinationX , destinationY );\r\n";

        public const String HuntDown =
            "bool inMyFace = false;\r\n" +
            "foreach (var enemyBot in Vision.EnemyBots)\r\n" +
            "{\r\n" +
            "    if (IsInMyFace(X, Y, enemyBot))\r\n" +
            "    {\r\n" +
            "        MeleeAttack();\r\n" +
            "        inMyFace = true;\r\n" +
            "        break;\r\n" +
            "    }\r\n" +
            "}\r\n" +
            "\r\n" +
            "if (!inMyFace)\r\n" +
            "{\r\n" +
            "    Bot targetBot = null;\r\n" +
            "\r\n" +
            "    foreach (var enemyBot in Vision.EnemyBots)\r\n" +
            "    {\r\n" +
            "        if (targetBot == null || CalculateDistance(X, Y, enemyBot) < CalculateDistance(X, Y, targetBot))\r\n" +
            "        {\r\n" +
            "            targetBot = enemyBot;\r\n" +
            "        }\r\n" +
            "    }\r\n" +
            "\r\n" +
            "    if (targetBot == null)\r\n" +
            "    {\r\n" +
            "        TurnLeft();\r\n" +
            "        StoreInMemory(\"DISTANCE\", 0);\r\n" +
            "    }\r\n" +
            "    else\r\n" +
            "    {\r\n" +
            "        double distance = LoadFromMemory<double>(\"DISTANCE\");\r\n" +
            "        if (distance == 0)\r\n" +
            "        {\r\n" +
            "            WalkForward();\r\n" +
            "        }\r\n" +
            "        else if (CalculateDistance(X, Y, targetBot) < distance)\r\n" +
            "        {\r\n" +
            "            WalkForward();\r\n" +
            "            StoreInMemory(\"DISTANCE\", CalculateDistance(X, Y, targetBot));\r\n" +
            "        }\r\n" +
            "        else\r\n" +
            "        {\r\n" +
            "            TurnRight();\r\n" +
            "        }\r\n" +
            "    }\r\n" +
            "}\r\n" +
            "\r\n" +
            "public bool IsInMyFace(int myX, int myY, Bot target)\r\n" +
            "{\r\n" +
            "    switch (Orientation)\r\n" +
            "    {\r\n" +
            "        case NORTH:\r\n" +
            "            return myX == target.X && myY == target.Y + 1;\r\n" +
            "        case EAST:\r\n" +
            "            return myY == target.Y && myX == target.X - 1;\r\n" +
            "        case SOUTH:\r\n" +
            "            return myX == target.X && myY == target.Y - 1;\r\n" +
            "        case WEST:\r\n" +
            "            return myY == target.Y && myX == target.X + 1;\r\n" +
            "    }\r\n" +
            "\r\n" +
            "    return false;\r\n" +
            "}\r\n" +
            "\r\n" +
            "public double CalculateDistance(int myX, int myY, Bot target)\r\n" +
            "{\r\n" +
            "    return Math.Sqrt(Math.Pow(myX - target.X, 2) + Math.Pow(myY - target.Y, 2));\r\n" +
            "}\r\n";
    }
}