namespace BusinessLogic.Exceptions.ReservationControllerExceptions;

public class ReservationNotFoundException : Exception
{
    public ReservationNotFoundException(string message) : base(message) {}
}