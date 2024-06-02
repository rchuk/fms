using Fms.Data;
using Fms.Dtos;
using Fms.Exceptions;
using Microsoft.Extensions.Localization;

namespace Fms.Application;

public class PublicErrorHandlerMiddleware
{
    private readonly ILogger<PublicErrorHandlerMiddleware> _logger;
    private readonly RequestDelegate _next;

    public PublicErrorHandlerMiddleware(RequestDelegate next, ILogger<PublicErrorHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
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
            }
            else if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
            {
                await context.Response.WriteAsJsonAsync(new PublicErrorDto
                {
                    Description = localizer[Localization.ErrorMessages.forbidden]
                });
            }
        }
        catch (PublicClientException e)
        {
            await Respond(context, StatusCodes.Status400BadRequest, new PublicErrorDto 
            {
                Description = e.Description
            });
        }
        catch (PublicForbiddenException e)
        {
            await Respond(context, StatusCodes.Status403Forbidden, new PublicErrorDto
            {
                Description = e.Description
            });
        }
        catch (PublicNotFoundException e)
        {
            await Respond(context, StatusCodes.Status404NotFound, new PublicErrorDto
            {
                Description = e.Description
            });
        }
        catch (PublicServerException e)
        {
            await Respond(context, StatusCodes.Status500InternalServerError, new PublicErrorDto
            {
                Description = e.Description
            });
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, "Uncaught exception: {exception}", e);
            await Respond(context, StatusCodes.Status500InternalServerError, new PublicErrorDto {
                Description = localizer[Localization.ErrorMessages.unknown_general] 
            });
        }
    }

    private static async Task Respond<TDto>(HttpContext context, int statusCode, TDto dto)
    {
        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(dto);
    }
}

