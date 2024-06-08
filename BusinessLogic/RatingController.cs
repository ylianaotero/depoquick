using BusinessLogic.Exceptions.RatingControllerExceptions;
using BusinessLogic.Exceptions.UserControllerExceptions;

namespace BusinessLogic;
using DepoQuick.Domain;

public class RatingController
{
    private const string BaseLogMessage = "Agregó valoración de la reserva ";
    private const string RatingNotFoundExceptionMessage = "No se encontro calificacion asociada a la reserva";
    private const string ActionRestrictedToClientExceptionMessage = "Solo el cliente puede realizar esta acción";
    private const string ActionRestrictedToAdministratorExceptionMessage = "Solo el administrador puede realizar esta acción";

    private IRepository<Rating> _ratingRepository;
    private Session _session;
    private LogController _logController;
    
    public RatingController(IRepository<Rating> ratingRepository,Session session, LogController logController)
    {
        _ratingRepository = ratingRepository;
        _session = session;
        _logController = logController;
    }
    
    public void DeleteByReservation(Reservation reservation)
    {
        Rating rating = GetRatingByReservation(reservation);
        
        if (rating == null)
        {
            throw new RatingNotFoundException(RatingNotFoundExceptionMessage); 
        }
        
        Delete(rating.Id);
    }
    
    public void RateReservation(Reservation reservation, Rating rating)
    { 
        User activeUser = _session.ActiveUser;

        if (activeUser.IsAdministrator)
        {
            throw new ActionRestrictedToClientException(ActionRestrictedToClientExceptionMessage);
        }
        
        Deposit deposit = reservation.Deposit;
        deposit.AddRating(rating);
        //    reservation.Rating = rating;
        Add(rating);
        String logMessage = BaseLogMessage + reservation.Id;
        _logController.LogAction(activeUser, logMessage, DateTime.Now);
    }

    public void UpdateRating(Rating rating, String message, int stars)
    {
        if (!UserLoggedIsAnAdministrator())
        {
            throw new ActionRestrictedToAdministratorException(ActionRestrictedToAdministratorExceptionMessage);
        }
        
        rating.Comment = message;
        rating.Stars = stars;
        
        _ratingRepository.Update(rating);
    }
    
    public Rating Get(int id)
    {
        return _ratingRepository.GetById(id);
    }

    public List<Rating> GetRatings()
    {
        return _ratingRepository.GetAll();
    }

    public List<Rating> GetRatingsByDeposit(Deposit deposit)
    {
        return _ratingRepository.GetBy(r => r.Reservation.Deposit == deposit); 
    }
    
    private Rating GetRatingByReservation(Reservation reservation)
    {
        return _ratingRepository.GetBy(p => p.Reservation == reservation).FirstOrDefault();
    }
    
    private void Add(Rating rating)
    {
        _ratingRepository.Add(rating);
    }
    
    private void Delete(int ratingId)
    {
        _ratingRepository.Delete(ratingId);
    }
    
    private bool UserLoggedIsAnAdministrator()
    {
        return _session.ActiveUser.IsAdministrator; 
    }
}