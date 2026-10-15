namespace FamilyRecipes.Middleware
{
    public class RequestLoggingMiddleware
    {
        //Class variables
        private readonly RequestDelegate _next;
        private readonly string _logFile;
        private readonly object _lock = new();

        //Constructor
        public RequestLoggingMiddleware(
            RequestDelegate next, IWebHostEnvironment env)
        {
            _next = next;

            var logDir = Path.Combine(env.ContentRootPath, "logs");
            Directory.CreateDirectory(logDir);
            _logFile = Path.Combine(logDir, "requests.log");
        }

        //Asynchronous Task
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            finally
            {
                var message =
                    $"{DateTime.UtcNow:O} {context.Request.Method} " +
                    $"{context.Request.Path} -> {context.Response.StatusCode}";

                lock (_lock)
                {
                    File.AppendAllText(
                        _logFile, message + Environment.NewLine);
                }
            }
        }
    }
}