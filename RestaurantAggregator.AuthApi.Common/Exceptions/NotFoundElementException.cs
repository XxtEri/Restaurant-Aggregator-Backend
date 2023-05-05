namespace RestaurantAggregator.AuthApi.Common.Exceptions;

public class NotFoundElementException: Exception
{
    public NotFoundElementException() { }
    
    public NotFoundElementException(string message): base(message) { }
    
    public NotFoundElementException(string message, Exception inner): base(message, inner) { }
    
    public NotFoundElementException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context): base(info, context) { }
}