using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Text.Json;

public class VisitorLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHttpClientFactory _httpClientFactory;

    public VisitorLoggingMiddleware(RequestDelegate next, IHttpClientFactory httpClientFactory)
    {
        _next = next;
        _httpClientFactory = httpClientFactory;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.ToString().ToLower();

        // İstemediğin URL patternleri veya dosya uzantıları için filtre
        if (path.StartsWith("/uploads")
            || path.StartsWith("/kelly-1.0.0")
            || path.EndsWith(".png")
            || path.EndsWith(".jpg")
            || path.EndsWith(".jpeg")
            || path.EndsWith(".gif")
            || path.EndsWith(".svg")
            || path.EndsWith(".ico")
            || path.EndsWith(".css")
            || path.EndsWith(".js")
            || path.EndsWith(".json")
            || path.EndsWith(".map"))
        {
            await _next(context);
            return;
        }

        // Log visitor information
        var visitorInfo = new
        {
            IpAddress = context.Connection.RemoteIpAddress?.ToString(),
            UserAgent = context.Request.Headers["User-Agent"].ToString(),
            VisitDate = DateTime.UtcNow,
            ReferrerUrl = context.Request.Headers["Referer"].ToString(),
            PageVisited = context.Request.Path.ToString()
        };

        using (var client = _httpClientFactory.CreateClient())
        {
            var jsonContent = JsonConvert.SerializeObject(visitorInfo);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Send the log to the logging API
            await client.PostAsync("https://localhost:7121/api/VisitorLogs", content);
        }

        // Call the next middleware in the pipeline
        await _next(context);
    }
}

/*
 
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public DateTime VisitDate { get; set; }
        public string? ReferrerUrl { get; set; }
        public string? PageVisited { get; set; }



{
  "totalVisitors": 11,
  "dailyVisitors": 10,
  "dailyRequests": 10,
  "instagramDailyVisitors": 0,
  "linkedinDailyVisitors": 0,
  "androidDailyVisitors": 2,
  "appleDailyVisitors": 6,
  "otherDailyVisitors": 2
}
 
 */
