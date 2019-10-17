using Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class FilterController : ApiController
    {
        [HttpGet]
        [HttpPost]
        public HttpResponseMessage ReadCSV(string csvUri)
        {
            WebClient client = new WebClient();
            Stream stream;
            try
            {
                stream = client.OpenRead(csvUri);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, $"Un problème a été détecté lors de la tentative d'accès au fichier. Message d'erreur : {ex.Message}");
            }
            LogHandler logHandler = new LogHandler(true); // true to force render as JSON as asked in Step 1 
            logHandler.ReadFile(stream);
            return Request.CreateResponse(HttpStatusCode.OK, logHandler.GetLogs());
        }
    }
}
