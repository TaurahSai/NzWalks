using Microsoft.AspNetCore.Mvc.Filters;

namespace NzWalks.API.CustomActionFilters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid == false)
            {
                context.Result = new Microsoft.AspNetCore.Mvc.BadRequestObjectResult(context.ModelState);
            }
        }
    }
}
