namespace Logging
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
        public bool ShouldSerializesumCD()
        {
            return sumCD.HasValue;
        }
    }
}
