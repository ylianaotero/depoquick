namespace BusinessLogic.Exceptions.ControllerExceptions;

public class ReservationNotFoundException : Exception
{
    public ReservationNotFoundException(string message) : base(message) {}
}