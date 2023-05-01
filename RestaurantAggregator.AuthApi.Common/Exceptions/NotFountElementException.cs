namespace RestaurantAggregator.AuthApi.Common.Exceptions;

public class NotFountElementException: Exception
{
    public NotFountElementException() { }
    
    public NotFountElementException(string message): base(message) { }
    
    public NotFountElementException(string message, Exception inner): base(message, inner) { }
    
    public NotFountElementException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context): base(info, context) { }
}