namespace Resiliency.Repository.Online.InMemory
{
    public static class HttpConstants
    {
        public static class ContentType
        {
            public const string ApplicationJson = "application/json";
            public const string ApplicationFormUrlEncoded = "application/x-www-form-urlencoded";
        }

        public static class Verb
        {
            public const string Get = "GET";
            public const string Put = "PUT";
            public const string Post = "POST";
            public const string Delete = "DELETE";
        }

        public static class Headers
        {
            public const string Authorization = "Authorization";
        }
    }
}
