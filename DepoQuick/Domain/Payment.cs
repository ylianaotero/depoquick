using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DepoQuick.Exceptions.PaymentExceptions;

namespace DepoQuick.Domain;

public class Payment
{
    [Key]
    public int Id { get; set; }
    
    public int Status { get; private set; }
    
    
    public Reservation Reservation { get; set; }

    public Payment()
    {
        Status = 0;
    }

    public void Capture()
    {
        if (ThereIsAReservationAssociated())
        {
            throw new CannotCapturePaymentIfDoesNotHaveAnAssociatedReservation(
                "No se puede capturar un pago no asociado con una reservacion"); 
        }
        else
        {
            Status = 1;
        }
    }

    private bool ThereIsAReservationAssociated()
    {
        return this.Reservation == null; 
    }
}