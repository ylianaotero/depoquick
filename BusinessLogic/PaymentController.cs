using BusinessLogic.Exceptions.PaymentControllerExceptions;
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
    
    public void DeleteByReservation(Reservation reservation)
    {
        try
        {
            Payment payment = Get(reservation);
            Delete(payment.Id);
        }
        catch (PaymentNotFoundException paymentNotFoundException)
        {
            return; 
        }
    }
    
    public void CapturePayment(Reservation reservation)
    {
        Payment payment = Get(reservation);
        
        payment.Capture();
        Update(payment);
    }
    
    public Payment Get(Reservation reservation)
    {
        Payment payment = _paymentRepository.GetBy(p => p.Reservation == reservation).FirstOrDefault();
        
        if (payment == null)
        {
            throw new PaymentNotFoundException(PaymentNotFoundExceptionMessage); 
        }
        
        return payment; 
    }
    
    private void Delete(int paymentId)
    {
        _paymentRepository.Delete(paymentId);
    }
    
    private void Update(Payment payment)
    {
        _paymentRepository.Update(payment);
    }
}