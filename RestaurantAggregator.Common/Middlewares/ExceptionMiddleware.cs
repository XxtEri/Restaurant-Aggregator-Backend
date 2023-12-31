using Microsoft.AspNetCore.Http;
using RestaurantAggregator.CommonFiles.Exceptions;

namespace RestaurantAggregator.CommonFiles.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _requestDelegate;

    public ExceptionMiddleware(RequestDelegate requestDelegate)
    {
        _requestDelegate = requestDelegate;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _requestDelegate(httpContext);
        }
        catch (Exception e)
        {
            httpContext.Response.StatusCode = (int)ExceptionStatusCodes.GetExceptionStatusCode(e);
            await httpContext.Response.WriteAsync(e.Message);
        } 
    }
}