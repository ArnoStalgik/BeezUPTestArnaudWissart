using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Logging
{
    public class ResponseObject
    {
        public int lineNumber { get; set; }
        public string type { get; set; }
        public string concatAB { get; set; }
        public int? sumCD { get; set; }
        public string errorMessage { get; set; }
        public ResponseObject()
        {
            type = "error";
            errorMessage = "La valeur de columnC n'est pas valide";
        }
    }
}
