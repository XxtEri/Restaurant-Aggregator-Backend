using Microsoft.AspNetCore.Builder;

namespace RestaurantAggregator.CommonFiles.Middlewares;

public static class MiddlewareException
{
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionMiddleware>();
    }
}