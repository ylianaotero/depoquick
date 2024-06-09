using BusinessLogic.Exceptions.ReservationControllerExceptions;
using BusinessLogic.Exceptions.UserControllerExceptions;
using DepoQuick.Domain;

namespace BusinessLogic;

public class ReservationController
{
    private const string ReservationNotFoundExceptionMessage = "No se encontró la reserva";
    private const string ActionRestrictedToAdministratorExceptionMessage = "Solo el administrador puede realizar esta acción";
    
    private const string MessageForAnApprovedReservation = " ha sido aprobada";
    private const string MessageForAnRejectedReservation = " ha sido rechazada";
    
    private NotificationController _notificationController;
    
    private IRepository<Reservation> _reservationRepository;
    
    private PaymentController _paymentController;
    
    private Session _session;
    
    public ReservationController(IRepository<Reservation> reservationRepository, Session session,
                                PaymentController paymentController, NotificationController notificationController)
    {
        _reservationRepository = reservationRepository;
        _paymentController = paymentController;
        _notificationController = notificationController; 
        _session = session;
    }
    
    public void Add(Reservation reservation)
    {
        reservation.RequestedAt = DateTime.Now;
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
        RestrictActionToAdministrator(); 
        
        ValidateExistanceOfReservation(reservation); 
        
        _paymentController.CapturePayment(reservation);
            
        reservation.Status = 1;
        
        _notificationController.Notify(reservation.Client, reservation, 
            "Su reserva del deposito "+reservation.Deposit.Id+" en las fechas "
            +reservation.Date.InitialDate.ToString("dd/MM/yyyy")+" a "
            +reservation.Date.FinalDate.ToString("dd/MM/yyyy") +MessageForAnApprovedReservation , DateTime.Now);
            
        UpdateReservation(reservation);
    }

    public void RejectReservation(Reservation reservation, string reason)
    {
        RestrictActionToAdministrator();

        ValidateExistanceOfReservation(reservation); 
        
        reservation.Message = reason;
        reservation.Status = -1;
        
        _notificationController.Notify(reservation.Client, reservation,
            "Su reserva del deposito "+reservation.Deposit.Id+" en las fechas "
            +reservation.Date.InitialDate.ToString("dd/MM/yyyy")+" a "
            +reservation.Date.FinalDate.ToString("dd/MM/yyyy")+ MessageForAnRejectedReservation , DateTime.Now);
            
        UpdateReservation(reservation);
    }
    

    public void AddPrice(Reservation reservation, int price)
    {
        reservation.Price = price;
    }

    public int GetPrice(Reservation reservation)
    {
        return reservation.Price;
    }
    
    public bool PromotionHasBeenApplied(Reservation reservation)
    {
        return reservation.Deposit.Promotions.Any(promotion => promotion.ValidityDate.IsDateInRange(reservation.RequestedAt));
    }
    
    private void RestrictActionToAdministrator()
    {
        if (!(UserIsLogged() && UserLoggedIsAnAdministrator()))
        {
            throw new ActionRestrictedToAdministratorException(ActionRestrictedToAdministratorExceptionMessage); 

        }
    }
    
    private bool UserLoggedIsAnAdministrator()
    {
        return _session.ActiveUser.IsAdministrator; 
    }
    
    private bool UserIsLogged()
    {
        return _session.UserLoggedIn(); 
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