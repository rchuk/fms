using Fms.Data;
using Fms.Dtos;
using Fms.Exceptions;
using Microsoft.Extensions.Localization;

namespace Fms.Application;

public class PublicErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public PublicErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context, IStringLocalizer<ErrorMessages> localizer)
    {
        try
        {
            await _next(context);
            
            if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
            {
                await context.Response.WriteAsJsonAsync(new PublicErrorDto
                {
                    Description = localizer[Localization.ErrorMessages.unathorized]
                });
            } else if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
            {
                await context.Response.WriteAsJsonAsync(new PublicErrorDto
                {
                    Description = localizer[Localization.ErrorMessages.forbidden]
                });
            }
        }
        catch (PublicClientException e)
        {
            await context.Response.WriteAsJsonAsync(new PublicClientErrorDto
            {
                Description = e.Description
            });
        }
        catch (PublicServerException e)
        {
            await context.Response.WriteAsJsonAsync(new PublicErrorDto
            {
                Description = e.Description
            });
        }
        catch (Exception)
        {
            await context.Response.WriteAsJsonAsync(new PublicErrorDto
            {
                Description = localizer[Localization.ErrorMessages.unknown_general]
            });
        }
    }
}

