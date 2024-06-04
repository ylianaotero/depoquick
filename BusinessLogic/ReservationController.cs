using BusinessLogic.Exceptions.ControllerExceptions;
using BusinessLogic.Exceptions.ReservationControllerExceptions;
using DepoQuick.Domain;

namespace BusinessLogic;

public class ReservationController
{
    private const string ReservationNotFoundExceptionMessage = "No se encontró la reserva";
    private const string ActionRestrictedToAdministratorExceptionMessage = "Solo el administrador puede realizar esta acción";
    
    private IRepository<Reservation> _reservationRepository;
    
    private PaymentController _paymentController;
    
    private Session _session;
    
    public ReservationController(IRepository<Reservation> reservationRepository, Session session,
                                PaymentController paymentController)
    {
        _reservationRepository = reservationRepository;
        _paymentController = paymentController;
        _session = session;
    }
    
    public void Add(Reservation reservation)
    { 
        _reservationRepository.Add(reservation);
    }
    
    public Reservation Get(DateRange validDateRange)
    {
        DateTime initialDate = validDateRange.InitialDate.Date;
        DateTime finalDate = validDateRange.FinalDate.Date;
        
        Reservation reservation = _reservationRepository.GetBy(r => r.Date.InitialDate.Date == initialDate && r.Date.FinalDate.Date == finalDate).FirstOrDefault();

        ReservationIsFound(reservation); 
        
        return reservation;
    }
    
    public Reservation Get(int reservationId)
    {
        Reservation reservation = _reservationRepository.GetById(reservationId);

        ReservationIsFound(reservation); 
        
        return reservation;
    }

    private void ReservationIsFound(Reservation reservation)
    {
        if (reservation == null)
        {
            throw new ReservationNotFoundException(ReservationNotFoundExceptionMessage);
        }
    }
    
    
    public List<Reservation> GetReservations()
    {
        List<Reservation> reservations = _reservationRepository.GetAll();
        return reservations; 
    }
    
    public List<Reservation> GetReservationsByUserId(int id)
    {
        List<Reservation> reservationsById = _reservationRepository.GetBy(r => r.Client.Id == id);
        
        return reservationsById;
    }
    
    public void ApproveReservation(Reservation reservation)
    {
        ValidateActiveUser(); 
        
        ValidateExistanceOfReservation(reservation); 
        
        _paymentController.CapturePayment(reservation);
            
        reservation.Status = 1;
            
        UpdateReservation(reservation);
    }

    public void RejectReservation(Reservation reservation, string reason)
    {
        ValidateActiveUser();

        ValidateExistanceOfReservation(reservation); 
        
        _paymentController.DeleteByReservation(reservation);
        
        reservation.Status = -1;
        reservation.Message = reason;
            
        UpdateReservation(reservation);
    }
    
    private void ValidateActiveUser()
    {
        if (!_session.ActiveUserIsAdministrator())
        {
            throw new ActionRestrictedToAdministratorException(ActionRestrictedToAdministratorExceptionMessage);
        }
    }
    
    private void ValidateExistanceOfReservation(Reservation reservation)
    {
        if (!Exits(reservation))
        {
            throw new ReservationNotFoundException(ReservationNotFoundExceptionMessage); 
        }
    }
    
    private bool Exits(Reservation reservation)
    {
        Reservation _reservation = _reservationRepository.GetBy(p => p == reservation).FirstOrDefault();
        if (_reservation == null)
        {
            return false; 
        }
        return true;
    }
    
    public void CancelRejectionOfReservation(Reservation reservation)
    {
        ValidateActiveUser(); 
        
        ValidateExistanceOfReservation(reservation); 
        
        reservation.Status = 0;
        UpdateReservation(reservation);
    }
    
    private void UpdateReservation(Reservation reservation)
    {
        _reservationRepository.Update(reservation);
    }
    
    public void PayReservation(Reservation reservation)
    {
        Payment payment = new Payment(); 
        payment.Reservation = reservation;
        _paymentController.Add(payment); 
    }
    

}