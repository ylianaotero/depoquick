namespace DepoQuick.Domain.Exceptions.ControllerExceptions;

public class ExceptionReservationNotFound : Exception
{
    public ExceptionReservationNotFound(string message) : base(message) {}
}