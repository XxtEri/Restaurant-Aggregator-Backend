using System.Net;

namespace RestaurantAggregator.CommonFiles.Exceptions;

public class ExceptionStatusCodes
{
    private static readonly Dictionary<Type, HttpStatusCode> _exceptionStatusCodes = new Dictionary<Type, HttpStatusCode>
    {
        { typeof(NotCorrectDataException), HttpStatusCode.BadRequest},
        { typeof(NotFoundException), HttpStatusCode.NotFound}
    };

    public static HttpStatusCode GetExceptionStatusCode(Exception exception)
    {
        bool exceptionFound = _exceptionStatusCodes.TryGetValue(exception.GetType(), out var statusCode);

        return exceptionFound ? statusCode : HttpStatusCode.InternalServerError;
    }
}