using Logging;
using System;

namespace ConsoleApp
{
    class Program
    {
        private const string CSVFILENAME = "bigfile.csv";
        static void Main(string[] args)
        {
            string arg = "JSON"; // Default render value
            if (args.Length > 0)
            {
                arg = args[0];
            }
            LogHandler logs = new LogHandler(arg, CSVFILENAME);
            Console.Write(logs);
            Console.Write(Environment.NewLine + "Appuyez sur n'importe quelle touche pour quitter.");
            Console.ReadKey();
        }
    }
}