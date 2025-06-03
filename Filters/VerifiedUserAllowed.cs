using Meloman.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Meloman.Filters{
    public class VerifiedUserAllowed : ActionFilterAttribute
    {
        private readonly MelomanContext _context;

        public VerifiedUserAllowed(MelomanContext context)
        {
            _context = context;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userId = context.HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            var user = _context.User.FirstOrDefault(u => u.Id == userId);

            if (user == null | (user!.Role != "user" && user!.Role != "admin"))
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }
            base.OnActionExecuting(context);
        }
    }
}