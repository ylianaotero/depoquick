using BusinessLogic.Exceptions.ReservationControllerExceptions;
using BusinessLogic.Exceptions.UserControllerExceptions;
using DepoQuick.Domain;

namespace BusinessLogic.Controllers;

public class ReservationController
{
    private const string ReservationNotFoundExceptionMessage = "No se encontró la reserva";
    private const string ActionRestrictedToAdministratorExceptionMessage = "Solo el administrador puede realizar esta acción";
    
    private const string MessageForAnApprovedReservation = " ha sido aprobada. ¡Gracias por confiar en nosotros!";
    private const string MessageForAnRejectedReservation = " ha sido rechazada. Si pagaste, te devolveremos el dinero a tu cuenta.";
    
    private NotificationController _notificationController;
    
    private IRepository<Reservation> _reservationRepository;
    
    private PaymentController _paymentController;
    
    private SessionController _sessionController;
    
    public ReservationController(IRepository<Reservation> reservationRepository, SessionController sessionController,
                                PaymentController paymentController, NotificationController notificationController)
    {
        _reservationRepository = reservationRepository;
        _paymentController = paymentController;
        _notificationController = notificationController; 
        _sessionController = sessionController;
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
        
        Reservation reservation = GetBy(r => r.Date.InitialDate.Date == initialDate && r.Date.FinalDate.Date == finalDate);
        
        return reservation;
    }
    
    public Reservation Get(int reservationId)
    {
        Reservation reservation = GetById(reservationId);
        
        return reservation;
    }
    
    private Reservation GetById(int reservationId)
    {
        try
        {
            Reservation reservation = _reservationRepository.GetById(reservationId);
            return reservation;
        }
        catch (NullReferenceException e)
        {
            throw new ReservationNotFoundException(ReservationNotFoundExceptionMessage); 
        }
    }
    
    private Reservation GetBy(Func<Reservation, bool> predicate)
    {
        Reservation reservation = _reservationRepository.GetBy(predicate).FirstOrDefault();
        
        
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
        return _sessionController.ActiveUser.IsAdministrator; 
    }
    
    private bool UserIsLogged()
    {
        return _sessionController.UserLoggedIn(); 
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