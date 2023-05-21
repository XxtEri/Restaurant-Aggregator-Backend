namespace RestaurantAggregator.CommonFiles.Exceptions;

public class DuplicateException: Exception
{
    public DuplicateException() { }
    
    public DuplicateException(string message): base(message) { }
    
    public DuplicateException(string message, Exception inner): base(message, inner) { }
    
    public DuplicateException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context): base(info, context) { }
}