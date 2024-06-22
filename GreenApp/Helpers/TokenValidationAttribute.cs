using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GreenApp.Helpers
{
    public class TokenValidationAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            string token = null;
            var authorizationHeader = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            {
                token = authorizationHeader.Substring("Bearer ".Length);
            }

            if (token == null)
            {
                context.Result = new ConflictObjectResult("Token not found");
                return;
            }

            (int userId, string userRole) = TokenReader.ReadToken(token);

            if (userId == -1)
            {
                context.Result = new ConflictObjectResult("Expired");
                return;
            }
            //Store userId & userRole for endpoints to grab it upon being called
            context.HttpContext.Items["UserId"] = userId;
            context.HttpContext.Items["UserRole"] = userRole;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
