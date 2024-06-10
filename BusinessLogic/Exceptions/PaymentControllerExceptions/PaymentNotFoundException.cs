namespace BusinessLogic.Exceptions.PaymentControllerExceptions;

public class PaymentNotFoundException : Exception
    {
    public PaymentNotFoundException(string message) : base(message) {}
}