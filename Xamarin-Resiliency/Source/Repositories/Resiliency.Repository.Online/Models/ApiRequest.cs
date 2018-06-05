using Resiliency.Repository.Online.InMemory;
using System.Collections.Generic;

namespace Resiliency.Repository.Online.Models
{
    public class ApiRequest
    {
        public string RequestUri { get; set; }
        public string Verb { get; set; } = HttpConstants.Verb.Get;
        public string JsonStringContent { get; set; }
        public IDictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
        public IDictionary<string, string> FormContent { get; set; } = new Dictionary<string, string>();
    }
}
