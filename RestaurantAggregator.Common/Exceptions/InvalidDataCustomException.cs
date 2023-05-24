namespace RestaurantAggregator.CommonFiles.Exceptions;

public class InvalidDataCustomException: Exception
{
    public InvalidDataCustomException() { }
    
    public InvalidDataCustomException(string message): base(message) { }
    
    public InvalidDataCustomException(string message, Exception inner): base(message, inner) { }
    
    public InvalidDataCustomException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context): base(info, context) { }
}