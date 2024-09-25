using System;

namespace TextRPG
{
    public static class Extension
    {
        public static void PrintWithColor(this string text, ConsoleColor color, bool isEnter)
        {
            Console.ForegroundColor = color;
            if (isEnter) Console.WriteLine(text);
            else Console.Write(text);
            Console.ResetColor();
        }

        public static void MakeDivider()
        {
            Console.Write(" | ");
        }
    }
}

