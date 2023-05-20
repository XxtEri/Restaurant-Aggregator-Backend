using System.Net;

namespace RestaurantAggregator.AuthApi.Common.Exceptions;

public static class ExceptionStatusCodes
{
    private static Dictionary<Type, HttpStatusCode> _exceptionStatusCodes = new Dictionary<Type, HttpStatusCode>
    {
        { typeof(NotFoundElementException), HttpStatusCode.NotFound},
        { typeof(InvalidDataCustomException), HttpStatusCode.Unauthorized},
        { typeof(NotPermissionAccountException), HttpStatusCode.Unauthorized},
        { typeof(DataAlreadyUsedException), HttpStatusCode.BadRequest},
    };

    public static HttpStatusCode GetExceptionStatusCode(Exception exception)
    {
        bool exceptionFound = _exceptionStatusCodes.TryGetValue(exception.GetType(), out var statusCode);

        return exceptionFound ? statusCode : HttpStatusCode.InternalServerError;
    }
}