using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace TestWeb.Models.Authentication
{
    public class Authentication:ActionFilterAttribute
    {
        private readonly int _role;
        public Authentication(int role)
        {
            _role = role;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userRole = context.HttpContext.Session.GetInt32("Role"); // Giả sử bạn lưu Role trong Session

            if (userRole == null || userRole != _role)
            {
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary {
                        {"Controller", "Account" },
                        {"Action", "Login" }
                    });
            }
        }
    }
}
