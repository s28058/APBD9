namespace APBD9.Exceptions;

public class ClientAlreadyOnTripException : Exception
{
    public ClientAlreadyOnTripException ()
    {}

    public ClientAlreadyOnTripException (string message) 
        : base(message)
    {}

    public ClientAlreadyOnTripException (string message, Exception innerException)
        : base (message, innerException)
    {}  
}