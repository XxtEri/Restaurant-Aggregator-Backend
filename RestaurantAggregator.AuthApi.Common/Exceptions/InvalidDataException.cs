namespace RestaurantAggregator.AuthApi.Common.Exceptions;

public class InvalidDataException: Exception
{
    public InvalidDataException() { }
    
    public InvalidDataException(string message): base(message) { }
    
    public InvalidDataException(string message, Exception inner): base(message, inner) { }
    
    public InvalidDataException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context): base(info, context) { }
}