namespace RestaurantAggregator.CommonFiles.Exceptions;

public class NotPermissionAccountException: Exception
{
    public NotPermissionAccountException() { }
    
    public NotPermissionAccountException(string message): base(message) { }
    
    public NotPermissionAccountException(string message, Exception inner): base(message, inner) { }
    
    public NotPermissionAccountException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context): base(info, context) { }
}