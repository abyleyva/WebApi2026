using System.Text;

namespace WebApi2026.Middlewares
{// BasicAuthMiddleware.cs (estructura simplificada)
    public class BasicAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _user = "abyleyva";
        private readonly string _password = "543210";

        public BasicAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value ?? string.Empty;
            // Omitir Swagger/Scalar/OpenAPI para poder usar la documentación
            if (path.Contains("swagger", StringComparison.OrdinalIgnoreCase) ||
                path.Contains("scalar", StringComparison.OrdinalIgnoreCase) ||
                path.Contains("openapi", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }

            if (!context.Request.Headers.TryGetValue("Authorization", out var header) ||
                !header.ToString().StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            var token = header.ToString().Substring("Basic ".Length).Trim();
            var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(token));
            var parts = decoded.Split(':', 2);
            var user = parts.ElementAtOrDefault(0);
            var pass = parts.ElementAtOrDefault(1);

            if (user == _user && pass == _password)
                await _next(context);
            else
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        }
    }

    public static class BasicAuthExtensions
    {
        public static IApplicationBuilder UseBasicAuth(this IApplicationBuilder app)
            => app.UseMiddleware<BasicAuthMiddleware>();
    }
}
