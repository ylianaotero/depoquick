namespace DepoQuick.Exceptions.PaymentExceptions;

public class CannotCapturePaymentIfDoesNotHaveAnAssociatedReservation : Exception
{
    public CannotCapturePaymentIfDoesNotHaveAnAssociatedReservation(string message) : base(message) {}
}