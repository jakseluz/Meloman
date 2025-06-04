using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Meloman.Data;
using Meloman.Models;

namespace Meloman.Filters
{
    /// <summary>
    /// Sprawdza nagłówki X-Username i X-ApiKey, weryfikuje w bazie.
    /// </summary>
    public class ApiKeyAuthorizeAttribute : ActionFilterAttribute
    {
        private readonly string[] _allowedRoles;
        public ApiKeyAuthorizeAttribute(params string[] allowedRoles)
        {
            _allowedRoles = allowedRoles ?? Array.Empty<string>();
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var httpContext = context.HttpContext;

            if (!httpContext.Request.Headers.TryGetValue("X-Username", out var usernameHeader) || !httpContext.Request.Headers.TryGetValue("X-ApiKey", out var apiKeyHeader))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            string username = usernameHeader.ToString();
            string apiKey = apiKeyHeader.ToString();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(apiKey))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // Get DbContext from DI
            var dbContext = httpContext.RequestServices.GetService<MelomanContext>();
            if (dbContext == null)
            {
                context.Result = new StatusCodeResult(500);
                return;
            }

            var user = dbContext.User.FirstOrDefault(u => u.Username == username && u.ApiKey == apiKey);

            if (user == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (_allowedRoles.Length > 0 && !_allowedRoles.Contains(user.Role))
            {
                context.Result = new ForbidResult();
                return;
            }

            httpContext.Items["AuthenticatedUser"] = user;

            base.OnActionExecuting(context);
        }
    }
}