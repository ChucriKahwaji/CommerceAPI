namespace API
{
    public class ErrorResponse
    {
        public string CorrelationId { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; } // Optional, only included in Development
    }
}
