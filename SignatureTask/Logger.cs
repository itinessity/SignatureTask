using System;
using System.Collections.Generic;
using System.Text;

namespace SignatureTask
{
   public static class Logger
    {
        public static void Log(string message)
        {
            Console.WriteLine($"{message}");
        }

        public static void Log(Exception e)
        {
            Console.WriteLine($"Message: {e.Message}");
            Console.WriteLine("Stack trace:");
            Console.WriteLine(e.StackTrace);
        }
    }
}
