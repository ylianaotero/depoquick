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

        if (reservation == null)
        {
            throw new ReservationNotFoundException(ReservationNotFoundExceptionMessage);
        }
        return reservation;
    }
    
    public Reservation Get(int reservationId)
    {
        Reservation reservation = _reservationRepository.GetById(reservationId);
        if (reservation == null)
        {
            throw new ReservationNotFoundException(ReservationNotFoundExceptionMessage);
        }
        
        return reservation;
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
        User activeUser = _session.ActiveUser;
   
        if (!activeUser.IsAdministrator)
        {
            throw new ActionRestrictedToAdministratorException(ActionRestrictedToAdministratorExceptionMessage);
        } 
        
        _paymentController.CapturePayment(reservation);
            
        reservation.Status = 1;
            
        UpdateReservation(reservation);
    }

    public void RejectReservation(Reservation reservation, string reason)
    {
        User activeUser = _session.ActiveUser;
   
        if (!activeUser.IsAdministrator)
        {
            throw new ActionRestrictedToAdministratorException(ActionRestrictedToAdministratorExceptionMessage);
        }
        
        _paymentController.DeleteByReservation(reservation);
            
        reservation.Status = -1;
        reservation.Message = reason;
            
        UpdateReservation(reservation);
    }
    
    public void CancelRejectionOfReservation(Reservation reservation)
    {
        User activeUser = _session.ActiveUser;
        
        if (!activeUser.IsAdministrator)
        {
            throw new ActionRestrictedToAdministratorException(ActionRestrictedToAdministratorExceptionMessage);
        }
        
        if (reservation != null)
        {
            reservation.Status = 0;
            UpdateReservation(reservation);
        }
    }
    
    private void UpdateReservation(Reservation reservation)
    {
        _reservationRepository.Update(reservation);
    }
}