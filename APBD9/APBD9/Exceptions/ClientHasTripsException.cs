namespace APBD9.Exceptions;

public class ClientHasTripsException : Exception 
{
    public ClientHasTripsException ()
    {}

    public ClientHasTripsException (string message) 
        : base(message)
    {}

    public ClientHasTripsException (string message, Exception innerException)
        : base (message, innerException)
    {}  
}