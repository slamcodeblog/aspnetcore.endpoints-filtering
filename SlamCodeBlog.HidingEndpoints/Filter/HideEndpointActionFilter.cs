using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace SlamCodeBlog.HidingEndpoints.Filter
{
    public class HideEndpointAttribute : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            context.Result = new NotFoundResult();
        }

        public override void OnActionExecuted(ActionExecutedContext context) { }
    }
}
