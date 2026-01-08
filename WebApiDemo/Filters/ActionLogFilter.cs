using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace WebApiDemo.Filters
{
    public class ActionLogFilter : IAsyncActionFilter
    {
        private readonly ILogger<ActionLogFilter> _logger;

        public ActionLogFilter(ILogger<ActionLogFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // ===== 実行前 =====
            var actionName = context.ActionDescriptor.DisplayName;

            var arguments = JsonSerializer.Serialize(context.ActionArguments);

            _logger.LogInformation("Action Executing: {Action}, Args: {Args}",actionName,arguments);

            // 実行 Action
            var executedContext = await next();

            // ===== 実行後 =====
            if (executedContext.Result is ObjectResult objectResult)
            {
                var resultJson = JsonSerializer.Serialize(objectResult.Value);

                _logger.LogInformation("Action Executed: {Action}, Result: {Result}", 
                    actionName, resultJson);
            }
            else
            {
                _logger.LogInformation(
                    "Action Executed: {Action}, ResultType: {Type}",
                    actionName, executedContext.Result?.GetType().Name);
            }
        }
    }
}
