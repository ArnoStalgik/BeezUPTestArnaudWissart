using Logging;
using System;

namespace ConsoleApp
{
    class Program
    {
        private const string CSVFILENAME = "bigfile.csv";
        private const string ARGJSON = "JSON";
        static void Main(string[] args)
        {
            bool renderAsJson = false;
            if (args.Length > 0)
            {
                string arg = args[0];
                renderAsJson = arg.ToUpper() == ARGJSON;
            }
            LogHandler logs = new LogHandler(renderAsJson);
            logs.ReadFile(CSVFILENAME);            
            Console.Write(logs);
            Console.Write(Environment.NewLine + "Appuyez sur n'importe quelle touche pour quitter.");
            Console.ReadKey();
        }
    }
}