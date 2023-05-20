namespace RestaurantAggregator.CommonFiles.Exceptions;

public class NotFoundException: Exception
{
    public NotFoundException() { }
    
    public NotFoundException(string message): base(message) { }
    
    public NotFoundException(string message, Exception inner): base(message, inner) { }
    
    public NotFoundException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context): base(info, context) { }
}