using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Obskurnee;

public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
{
    public int Order { get; } = int.MaxValue - 10;

    public void OnActionExecuting(ActionExecutingContext context)
    {
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception is ObskurneeException exception)
        {
            context.Result = new ObjectResult(exception.Message)
            {
                StatusCode = ((ObskurneeException)context.Exception).StatusCode,
            };
            context.ExceptionHandled = true;
        }
    }
}
