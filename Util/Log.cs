using System;

namespace Ontwikkelopdracht.Models
{
    public static class Log
    {
        public static void I(string tag, string msg) => PrintLn(tag, msg, ConsoleColor.DarkGreen);
        public static void D(string tag, string msg) => PrintLn(tag, msg, ConsoleColor.Blue);
        public static void W(string tag, string msg) => PrintLn(tag, msg, ConsoleColor.Yellow);
        public static void E(string tag, string msg) => PrintLn(tag, msg, ConsoleColor.Red);

        public static void PrintLn(string tag, string msg, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write($"[{tag.PadRight(10).Substring(0, 10)}] ");
            Console.ResetColor();
            Console.WriteLine(msg);
        }
    }
}