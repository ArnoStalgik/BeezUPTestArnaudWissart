using CsvHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Logging
{
    public class LogHandler
    {
        private List<ResponseObject> _responseObjects;

        private LogResponseType _logResponseType;
        public LogHandler() { _responseObjects = new List<ResponseObject>(); }
        public LogHandler(string logResponseType, Stream stream)
        {
            _responseObjects = new List<ResponseObject>();
            _logResponseType = SetResponseTypeFromParameter(logResponseType);
            ReadFile(stream);
        }
        public LogHandler(string logResponseType, string fileName)
        {
            _responseObjects = new List<ResponseObject>();
            _logResponseType = SetResponseTypeFromParameter(logResponseType);
            ReadFile(fileName);
        }
        private void ReadFile (Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                ReadFile(reader);
            }
        }
        private void ReadFile(string fileName)
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
            switch(_logResponseType)
            {
                case LogResponseType.JSON:
                    returnValue = JsonConvert.SerializeObject(_responseObjects, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }) + Environment.NewLine;
                    break;
                case LogResponseType.PlainText:
                    StringBuilder sb = new StringBuilder();
                    foreach (ResponseObject log in _responseObjects)
                    {
                        if (!string.IsNullOrEmpty(log.concatAB))
                        {
                            sb.AppendLine(log.concatAB);
                        }
                    }
                    returnValue = sb.ToString();
                    break;
                case LogResponseType.XML:
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ResponseObject>));
                    using (StringWriter textWriter = new StringWriter())
                    {
                        xmlSerializer.Serialize(textWriter, _responseObjects);
                        returnValue = textWriter.ToString();
                    }
                    break;
            }
            return returnValue;
        }
        private LogResponseType SetResponseTypeFromParameter(string param)
        {
            switch (param.ToUpper())
            {
                case "TEXT":
                    return LogResponseType.PlainText;
                case "XML":
                    return LogResponseType.XML;
            }
            // Default value
            return LogResponseType.JSON;
        }
    }
}
