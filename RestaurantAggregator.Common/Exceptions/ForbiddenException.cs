namespace RestaurantAggregator.CommonFiles.Exceptions;

public class ForbiddenException: Exception
{
    public ForbiddenException() { }
    
    public ForbiddenException(string message): base(message) { }
    
    public ForbiddenException(string message, Exception inner): base(message, inner) { }
    
    public ForbiddenException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context): base(info, context) { }
}