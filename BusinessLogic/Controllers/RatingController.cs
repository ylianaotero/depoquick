using BusinessLogic.Exceptions.RatingControllerExceptions;
using BusinessLogic.Exceptions.UserControllerExceptions;
using DepoQuick.Domain;

namespace BusinessLogic.Controllers;

public class RatingController
{
    private const string BaseLogMessage = "Agregó valoración de la reserva ";
    private const string ActionRestrictedToClientExceptionMessage = "Solo el cliente puede realizar esta acción";
    private const string RatingNotFoundExceptionMessage = "El rating no fue encontrado";

    private IRepository<Rating> _ratingRepository;
    private SessionController _sessionController;
    private LogController _logController;
    
    public RatingController(IRepository<Rating> ratingRepository,SessionController sessionController, LogController logController)
    {
        _ratingRepository = ratingRepository;
        _sessionController = sessionController;
        _logController = logController;
    }
    
    public void RateReservation(Reservation reservation, Rating rating)
    { 
        User activeUser = _sessionController.ActiveUser;

        RestrictActionToClient();
        
        rating.Reservation = reservation;
        
        Deposit deposit = reservation.Deposit;
        deposit.AddRating(rating);
     
        Add(rating);
        String logMessage = BaseLogMessage + reservation.Id;
        _logController.LogAction(activeUser, logMessage, DateTime.Now);
    }

    public void UpdateRating(Rating rating, String message, int stars)
    {
        RestrictActionToClient();
        
        rating.Comment = message;
        rating.Stars = stars;
        
        _ratingRepository.Update(rating);
    }
    
    public List<Rating> GetRatings()
    {
        return _ratingRepository.GetAll();
    }

    public List<Rating> GetRatingsByDeposit(Deposit deposit)
    {
        return _ratingRepository.GetBy(r => r.Reservation.Deposit == deposit); 
        
    }
    
    public Rating GetRatingByReservation(Reservation reservation)
    {
        Rating rating = _ratingRepository.GetBy(p => p.Reservation == reservation).FirstOrDefault();
        if (rating == null)
        {
            throw new RatingNotFoundException(RatingNotFoundExceptionMessage); 
        }

        return rating; 
    }
    
    private void Add(Rating rating)
    {
        _ratingRepository.Add(rating);
    }
    
    private void RestrictActionToClient()
    {
        if ((UserIsLogged() && !UserLoggedIsAClient()))
        {
            throw new ActionRestrictedToClientException(ActionRestrictedToClientExceptionMessage); 
        }
    }

    private bool UserIsLogged()
    {
        return _sessionController.UserLoggedIn(); 
    }
    
    private bool UserLoggedIsAClient()
    {
        return !_sessionController.ActiveUser.IsAdministrator; 
    }
}