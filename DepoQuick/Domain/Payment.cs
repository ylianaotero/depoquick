using System.ComponentModel.DataAnnotations;
using DepoQuick.Exceptions.PaymentExceptions;

namespace DepoQuick.Domain;

public class Payment
{
    private const string InitialStatus = "reservado";
    private const string CapturedStatus = "capturado";
    private const string CannotCapturePaymentMessage = "No se puede capturar un pago no asociado con una reserva";
    
    [Key]
    public int Id { get; set; }
    
    public String Status { get; private set; }
    
    public Reservation Reservation { get; set; }

    public Payment()
    {
        Status = InitialStatus;
    }

    public void Capture()
    {
        if (Reservation == null)
        {
            throw new CannotCapturePaymentIfDoesNotHaveAnAssociatedReservation(CannotCapturePaymentMessage); 
        }
       
        Status = CapturedStatus;
    }
}