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
        public LogHandler() { _responseObjects = new List<ResponseObject>(); }
        public void AddLog(ResponseObject responseObject)
        {
            _responseObjects.Add(responseObject);
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(_responseObjects, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }) + Environment.NewLine; ;
        }
    }
}
