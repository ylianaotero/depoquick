using BusinessLogic.Exceptions.ControllerExceptions;
using BusinessLogic.Exceptions.ReservationControllerExceptions;
using DepoQuick.Domain;

namespace BusinessLogic;

public class ReservationController
{
    private DepoQuickContext _context;
    private Session _session;
    
    public ReservationController(DepoQuickContext context, Session session)
    {
        _context = context;
        _session = session;
    }
    
    
    public void Add(Reservation reservation)
    { 
        _context.Reservations.Add(reservation);
      //Client client = reservation.Client;
       //client.AddReservation(reservation);
       
      //  reservation.Deposit.AddReservation(reservation);
        _context.SaveChanges();
    }

    public void PayReservation(Reservation reservation)
    {
        Payment payment = new Payment(); 
        payment.Reservation = reservation;
        _context.Payments.Add(payment); 
        _context.SaveChanges();
    }
    
    public Payment GetPaymentOfReservation(Reservation reservation)
    {
        Payment payment = SearchForAPayment(reservation);
        if (payment == null)
        {
            throw new PaymentNotFoundException("No se encontro pago asociado a la reserva"); 
        }
        else
        {
            return payment; 
        }
    }

    private Payment SearchForAPayment(Reservation reservation)
    {
        return _context.Payments.FirstOrDefault(r =>r.Reservation == reservation);
        ;
    }
    
    
    public Reservation Get(DateRange validDateRange)
    {
        DateTime initialDate = validDateRange.InitialDate.Date;
        DateTime finalDate = validDateRange.FinalDate.Date;

        Reservation reservation = _context.Reservations.FirstOrDefault(r =>r.Date.InitialDate.Date == validDateRange.InitialDate.Date && r.Date.InitialDate.Hour == validDateRange.InitialDate.Hour && r.Date.InitialDate.Minute == validDateRange.InitialDate.Minute && r.Date.FinalDate.Date == validDateRange.FinalDate.Date && r.Date.FinalDate.Hour == validDateRange.FinalDate.Hour && r.Date.FinalDate.Minute == validDateRange.FinalDate.Minute );

        if (reservation == null)
        {
            throw new ReservationNotFoundException("No se encontró la reserva");
        }
        return reservation;
    }
    
    public Reservation Get(int reservationId)
    {
        Reservation reservation = _context.Reservations.Find(reservationId);
        if (reservation == null)
        {
            throw new ReservationNotFoundException("No se encontró la reserva");
        }
        else
        {
            return reservation;
        }
    }
    
    
    public List<Reservation> GetReservations()
    {
        List<Reservation> reservations = _context.Reservations.ToList();
        return reservations; 
    }
    
    
    public List<Reservation> GetReservationsById(int id)
    {
        var reservationsById = new List<Reservation>();
    
        foreach (var reservation in _context.Reservations)
        {
            int clientId = _context.Entry(reservation).Property<int>("ClientId").CurrentValue;
            if (clientId == id)
            {
                reservationsById.Add(reservation);
            }
        }
        return reservationsById;
    }


    public void ApproveReservation(Reservation reservation)
    {
        Administrator admin = (Administrator)_context.Users.FirstOrDefault(u => u.IsAdministrator);
        if (admin == null)
        {
            throw new EmptyAdministratorException("No se encontró ningún administrador");
        }

        Payment payment = GetPaymentOfReservation(reservation); 
        payment.Capture();

        admin.ApproveReservation(reservation);
        _context.SaveChanges();
    }

    public void RejectReservation(Reservation reservation, string reason)
    {
        Administrator admin = _context.Users.OfType<Administrator>().FirstOrDefault();
        if (admin == null)
        {
            throw new UserDoesNotExistException("No se encontró ningún administrador");
        }

        if (reservation == null)
        {
            throw new ArgumentNullException(nameof(reservation), "La reserva no puede ser nula");
        }

        if (string.IsNullOrEmpty(reason))
        {
            throw new ArgumentException("La razón de rechazo no puede ser nula o vacía", nameof(reason));
        }

        admin.RejectReservation(reservation, reason);
        reservation.Status = -1;
        reservation.Message = reason;
        _context.SaveChanges();
    }
    
    public void CancelRejectionOfReservation(Reservation reservation)
    {
        if (_session.ActiveUser.IsAdministrator)
        {
            reservation.Status = 0;
            reservation.Deposit.AddReservation(reservation);
            //Add(reservation);
        }
        else
        {
            throw new ActionRestrictedToAdministratorException("Solo el administrador puede cancelar una reserva");
        }   
    }

    public void RateReservation(Reservation reservation, Rating rating)
    {
        if (!_session.ActiveUser.IsAdministrator)
        {
            Deposit deposit = reservation.Deposit;
            deposit.AddRating(rating); 
        //    reservation.Rating = rating;
            _context.Ratings.Add(rating);
            UserController _userController = new UserController(_context);
            _userController.LogAction(reservation.Client, "Agregó valoración de la reserva " + reservation.Id, DateTime.Now);
            reservation.Client.LogAction("Agregó valoración de la reserva" + reservation.Id, DateTime.Now);
        }
        else
        {
            throw new ActionRestrictedToClientException("Solo el cliente puede calificar una reserva");
        }
    }
}