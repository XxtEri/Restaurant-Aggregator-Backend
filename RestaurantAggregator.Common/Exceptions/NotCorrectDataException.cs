namespace RestaurantAggregator.CommonFiles.Exceptions;

public class NotCorrectDataException: Exception
{
    public NotCorrectDataException() { }
    
    public NotCorrectDataException(string message): base(message) { }
    
    public NotCorrectDataException(string message, Exception inner): base(message, inner) { }
    
    public NotCorrectDataException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context): base(info, context) { }
}