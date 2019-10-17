using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Logging
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
        public void AddLog(ResponseObject responseObject)
        {
            _responseObjects.Add(responseObject);
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
    }
}
