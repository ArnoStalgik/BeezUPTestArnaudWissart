using Logging;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class FilterController : ApiController
    {
        [HttpGet]
        [HttpPost]
        public HttpResponseMessage ReadCSV(string csvUri, string renderAs = "JSON")
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
            LogHandler logHandler = new LogHandler(renderAs, stream);
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(logHandler.ToString(), Encoding.UTF8);
            return response;
        }        
    }
}
