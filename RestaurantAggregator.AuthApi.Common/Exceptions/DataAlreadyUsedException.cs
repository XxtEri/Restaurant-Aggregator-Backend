namespace RestaurantAggregator.AuthApi.Common.Exceptions;

public class DataAlreadyUsedException: Exception
{
    public DataAlreadyUsedException() { }
    
    public DataAlreadyUsedException(string message): base(message) { }
    
    public DataAlreadyUsedException(string message, Exception inner): base(message, inner) { }
    
    public DataAlreadyUsedException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context): base(info, context) { }
}