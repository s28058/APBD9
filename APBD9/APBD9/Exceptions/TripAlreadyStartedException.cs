namespace APBD9.Exceptions;

public class TripAlreadyStartedException : Exception
{
    public TripAlreadyStartedException ()
    {}

    public TripAlreadyStartedException (string message) 
        : base(message)
    {}

    public TripAlreadyStartedException (string message, Exception innerException)
        : base (message, innerException)
    {}  
}