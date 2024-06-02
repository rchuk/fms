using System.Transactions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Fms.Application.Attributes;

using System;

[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
public class TransactionalAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        using var transactionScope = new TransactionScope();
        var actionExecutedContext = await next();
       
        if (actionExecutedContext.Exception == null)
            transactionScope.Complete();
    }
}
