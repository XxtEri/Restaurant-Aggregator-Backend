using System.Net;

namespace RestaurantAggregator.CommonFiles.Exceptions;

public class ExceptionStatusCodes
{
    private static readonly Dictionary<Type, HttpStatusCode> _exceptionStatusCodes = new Dictionary<Type, HttpStatusCode>
    {
        { typeof(NotCorrectDataException), HttpStatusCode.BadRequest },
        { typeof(NotFoundException), HttpStatusCode.NotFound },
        { typeof(ForbiddenException), HttpStatusCode.Forbidden },
        { typeof(DuplicateException), HttpStatusCode.Conflict },
        { typeof(InvalidResponseException), HttpStatusCode.BadRequest }
    };

    public static HttpStatusCode GetExceptionStatusCode(Exception exception)
    {
        var exceptionFound = _exceptionStatusCodes.TryGetValue(exception.GetType(), out var statusCode);

        return exceptionFound ? statusCode : HttpStatusCode.InternalServerError;
    }
}