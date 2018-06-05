namespace Resiliency.Repository.Online.Models
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string ReasonPhrase { get; set; }
        public bool IsSuccessStatusCode { get; set; }
        public string ResponseContent { get; set; }
    }
}
