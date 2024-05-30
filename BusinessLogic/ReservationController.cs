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
        else
        {
            return reservation;
        }
    }
    
    public List<Reservation> GetReservations()
    {
        List<Reservation> reservations = _reservationRepository.GetAll();
        return reservations; 
    }
    
    
    public List<Reservation> GetReservationsById(int id)
    {
        List<Reservation> reservationsById = _reservationRepository.GetBy(r => r.Client.Id == id);
        
        return reservationsById;
    }


    public void ApproveReservation(Reservation reservation)
    {
        User activeUser = _session.ActiveUser;
   
        if (activeUser.IsAdministrator)
        {
            _paymentController.CapturePayment(reservation);
            
            reservation.Status = 1;
            
            UpdateReservation(reservation);
            
        } else {
            throw new ActionRestrictedToAdministratorException(ActionRestrictedToAdministratorExceptionMessage);
        }
    }

    public void RejectReservation(Reservation reservation, string reason)
    {
        User activeUser = _session.ActiveUser;
   
        if (activeUser.IsAdministrator)
        {
            _paymentController.DeleteByReservation(reservation);
            
            reservation.Status = -1;
            reservation.Message = reason;
            
            UpdateReservation(reservation);
            
        } else {
            throw new ActionRestrictedToAdministratorException(ActionRestrictedToAdministratorExceptionMessage);
        }
    }
    
    public void CancelRejectionOfReservation(Reservation reservation)
    {
        User activeUser = _session.ActiveUser;
        
        if (activeUser.IsAdministrator)
        {
            if (reservation != null)
            {
                reservation.Status = 0;
                UpdateReservation(reservation);
            }
        }
        else
        {
            throw new ActionRestrictedToAdministratorException(ActionRestrictedToAdministratorExceptionMessage);
        }   
    }
    
    private void UpdateReservation(Reservation reservation)
    {
        _reservationRepository.Update(reservation);
    }
    public void RateReservation(Reservation reservation, Rating rating)
    {
    /*    User activeUser = _session.ActiveUser;
        
        if (!activeUser.IsAdministrator)
        {
            Deposit deposit = reservation.Deposit;
            deposit.AddRating(rating); 
        //    reservation.Rating = rating;
            _context.Ratings.Add(rating);
            IRepository<User> userRepository = new SqlRepository<User>(_context);
            UserController _userController = new UserController(userRepository);
            _userController.LogAction(reservation.Client, "Agregó valoración de la reserva " + reservation.Id, DateTime.Now);
            reservation.Client.LogAction("Agregó valoración de la reserva" + reservation.Id, DateTime.Now);
        }
        else
        {
            throw new ActionRestrictedToClientException("Solo el cliente puede calificar una reserva");
        }*/
    }
}