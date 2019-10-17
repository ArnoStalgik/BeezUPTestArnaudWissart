using CsvHelper;
using System;
using System.IO;

namespace ConsoleApp
{
    class Program
    {
        private const string CSVFILENAME = "bigfile.csv";
        static void Main(string[] args)
        {
            using (var reader = new StreamReader(CSVFILENAME))
            using (var csv = new CsvReader(reader))
            {
                csv.Read();
                csv.ReadHeader();
                string columnA, columnB;
                int columnC, columnD;
                while (csv.Read())
                {
                    if (csv.TryGetField<int>("columnC", out columnC))
                    {
                        if (csv.TryGetField<int>("columnD", out columnD))
                        {
                            if (columnC + columnD > 100)
                            {
                                columnA = csv.GetField<string>("columnA");
                                columnB = csv.GetField<string>("columnB");
                                Console.WriteLine(columnA + columnB);
                            }
                        }
                    }
                }
            }
            Console.ReadKey();
        }
    }
}
