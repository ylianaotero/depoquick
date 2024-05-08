namespace DepoQuick.Exceptions.ReservationExceptions;

public class ReservationWithEmptyMessageException : Exception
{
    public ReservationWithEmptyMessageException(string message) : base(message) {}
}