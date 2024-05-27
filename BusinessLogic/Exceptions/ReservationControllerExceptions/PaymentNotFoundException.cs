namespace BusinessLogic.Exceptions.ReservationControllerExceptions;

public class PaymentNotFoundException : Exception
    {
    public PaymentNotFoundException(string message) : base(message) {}
}