using System;

namespace CSharpWars.Web.Constants
{
    public static class BotScripts
    {
        public const String WalkAround =
            "var step = LoadFromMemory<Int32>(\"STEP\");\r\n" +
            "if( step % 3 == 0 )\r\n" +
            "{\r\n" +
            "    TurnLeft();\r\n" +
            "}\r\n" +
            "else\r\n" +
            "{\r\n" +
            "    MoveForward();\r\n" +
            "}\r\n" +
            "step++\r\n" +
            "StoreInMemory<Int32>(\"STEP\", step);\r\n";

    }
}