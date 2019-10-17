using CsvHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Logging
{
    public class LogHandler
    {
        private List<ResponseObject> _responseObjects;

        private bool _renderAsJSON;
        public LogHandler() { _responseObjects = new List<ResponseObject>(); }
        public LogHandler(bool renderAsJSON) 
        { 
            _responseObjects = new List<ResponseObject>();
            _renderAsJSON = renderAsJSON;
        }
        public void ReadFile (Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                ReadFile(reader);
            }
        }
        public void ReadFile(string fileName)
        {            
            using (var reader = new StreamReader(fileName))
            {
                ReadFile(reader);
            }            
        }
        private void ReadFile(StreamReader reader)
        {
            ResponseObject currentLog;
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
                    _responseObjects.Add(currentLog);
                    lineNumber++;
                }
            }
        }
        public override string ToString()
        {
            string returnValue = string.Empty;
            if (_renderAsJSON)
            {
                returnValue = JsonConvert.SerializeObject(_responseObjects, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }) + Environment.NewLine;
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (ResponseObject log in _responseObjects)
                {
                    if (!string.IsNullOrEmpty(log.concatAB))
                    {
                        sb.AppendLine(log.concatAB);
                    }
                }
                returnValue = sb.ToString();
            }
            return returnValue;
        }
        public IEnumerable<ResponseObject> GetLogs()
        {
            return _responseObjects;
        }
    }
}
