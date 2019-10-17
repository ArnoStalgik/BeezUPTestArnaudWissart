using ConsoleApp.Logging;
using CsvHelper;
using System;
using System.IO;

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
            ResponseObject currentLog;
            using (var reader = new StreamReader(CSVFILENAME))
            using (var csv = new CsvReader(reader))
            {
                csv.Read();
                csv.ReadHeader();
                string columnA, columnB;
                int columnC, columnD, lineNumber = 2; // lineNumber = 2 (to match excel row numbers)
                while (csv.Read())
                {
                    currentLog = new ResponseObject { lineNumber = lineNumber };
                    if (csv.TryGetField<int>("columnC", out columnC))
                    {
                        currentLog.errorMessage = "La valeur de columnD n'est pas valide";
                        if (csv.TryGetField<int>("columnD", out columnD))
                        {
                            int sumCD = columnC + columnD;
                            currentLog.errorMessage = "La somme de columnC et columnD ne dépasse pas 100";
                            if (sumCD > 100)
                            {
                                columnA = csv.GetField<string>("columnA");
                                columnB = csv.GetField<string>("columnB");
                                currentLog.type = "ok";
                                currentLog.concatAB = columnA + columnB;
                                currentLog.sumCD = sumCD;
                                currentLog.errorMessage = null;
                            }
                        }
                    }
                    logs.AddLog(currentLog);
                    lineNumber++;
                }
                Console.Write(logs);
            }
            Console.ReadKey();
        }
    }
}