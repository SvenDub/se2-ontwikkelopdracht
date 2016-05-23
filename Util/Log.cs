using System;

namespace Ontwikkelopdracht.Models
{
    public static class Log
    {
        public static void I(string tag, string msg) => PrintLn(tag, "INFO ", msg, ConsoleColor.DarkGreen);
        public static void D(string tag, string msg) => PrintLn(tag, "DEBUG", msg, ConsoleColor.Blue);
        public static void W(string tag, string msg) => PrintLn(tag, "WARN ", msg, ConsoleColor.Yellow);
        public static void E(string tag, string msg) => PrintLn(tag, "ERROR", msg, ConsoleColor.Red);

        public static void PrintLn(string tag, string level, string msg, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write($"{DateTime.Now.ToString("HH:mm:ss:fff")} [{tag.PadRight(10).Substring(0, 10)}] [{level}] ");
            Console.ResetColor();
            Console.WriteLine(msg);
        }
    }
}