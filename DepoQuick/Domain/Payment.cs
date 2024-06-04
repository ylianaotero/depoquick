using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DepoQuick.Exceptions.PaymentExceptions;

namespace DepoQuick.Domain;

public class Payment
{
    [Key]
    public int Id { get; set; }
    
    public String Status { get; private set; }
    
    public Reservation Reservation { get; set; }

    public Payment()
    {
        Status = "reservado";
    }

    public void Capture()
    {
        if (ThereIsAReservationAssociated())
        {
            throw new CannotCapturePaymentIfDoesNotHaveAnAssociatedReservation(
                "No se puede capturar un pago no asociado con una reserva"); 
        }
        else
        {
            Status = "capturado";
        }
    }

    private bool ThereIsAReservationAssociated()
    {
        return this.Reservation == null; 
    }
}