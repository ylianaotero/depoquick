using BusinessLogic.Exceptions.ReservationControllerExceptions;
using DepoQuick.Domain;

namespace BusinessLogic;

public class PaymentController
{
    private const string PaymentNotFoundExceptionMessage = "No se encontro pago asociado a la reserva";
    
    private IRepository<Payment> _paymentRepository;
    
    public PaymentController(IRepository<Payment> paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }
    
    public void Add(Payment payment)
    {
        _paymentRepository.Add(payment);
    }
    
    private void Delete(int paymentId)
    {
        _paymentRepository.Delete(paymentId);
    }
    
    public void DeleteByReservation(Reservation reservation)
    {
        Payment payment = SearchForAPayment(reservation);
        
        if (payment == null)
        {
            throw new PaymentNotFoundException(PaymentNotFoundExceptionMessage); 
        }
        else
        {
            Delete(payment.Id);
        }
    }
    
    public Payment GetPaymentByReservation(Reservation reservation)
    {
        Payment payment = SearchForAPayment(reservation);
        
        if (payment == null)
        {
            throw new PaymentNotFoundException(PaymentNotFoundExceptionMessage); 
        }
        else
        {
            return payment; 
        }
    }
    
    public void CapturePayment(Reservation reservation)
    {
        Payment payment = SearchForAPayment(reservation);
        
        payment.Capture();
        Update(payment);
    }
    
    private Payment SearchForAPayment(Reservation reservation)
    {
        return _paymentRepository.GetBy(p => p.Reservation == reservation).FirstOrDefault();
    }
    
    private void Update(Payment payment)
    {
        _paymentRepository.Update(payment);
    }
}