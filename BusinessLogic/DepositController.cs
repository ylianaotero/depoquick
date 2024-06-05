using BusinessLogic.Exceptions.ControllerExceptions;
using DepoQuick.Domain;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic;

public class DepositController
{
    private const string DepositDateIsAlreadyreservedMessage = "El deposito no esta disponible en esa fecha";
    private const string DepositNotFoundExceptionMessage = "Deposito no encontrado";
    private const string ActionRestrictedToAdministratorExceptionMessage = "Solo el administrador puede realizar esta acción";
    private const string DepositDateIsOverlappingMessage = "El deposito ya tiene una reserva en esa fecha";
    
    private IRepository<Deposit> _depositRepository;
    private IRepository<Promotion> _promotionRepository;
    private Session _session;
    
    public DepositController(IRepository<Deposit> depositRepository, IRepository<Promotion> promotionRepository, Session session)
    {
        _depositRepository = depositRepository;
        _promotionRepository = promotionRepository;
        _session = session; 
    }
    
    public void AddDeposit(Deposit deposit, List<Promotion> promotions)
    {
        if (!(UserIsLogged() && UserLoggedIsAnAdministrator()))
        {
            throw new ActionRestrictedToAdministratorException(ActionRestrictedToAdministratorExceptionMessage); 

        }
        
        Add(deposit);

        ConectDepositToPromotions(deposit, promotions);
    }
    
    public Deposit Get(int id)
    {
        Deposit deposit = _depositRepository.GetById(id);
        
        if (deposit == null)
        {
            throw new DepositNotFoundException(DepositNotFoundExceptionMessage); 
        }
        
        return deposit; 
    }
    
    public List<Deposit> GetDeposits()
    {
        List<Deposit> deposits = _depositRepository.GetAll();
        
        return deposits; 
    }

    public void DeleteDeposit(int id)
    {
        if (!(UserIsLogged() && UserLoggedIsAnAdministrator()))
        {
            throw new ActionRestrictedToAdministratorException(ActionRestrictedToAdministratorExceptionMessage); 
        }
        
        Deposit depositToDelete = Get(id);
            
        _depositRepository.Reload(depositToDelete);
        
        List<Promotion> relatedPromotions = depositToDelete.Promotions;

        RemoveDepositFromRelatedPromotions(depositToDelete, relatedPromotions); 
            
        Delete(depositToDelete);
    }

    public void AddAvailabilityDate(Deposit deposit, DateRange date)
    {
        ValidateDepositIsNotReserved(deposit,date);

        ValidateDepositDateIsNotOverlapping(deposit,date);
        
        deposit.AvailableDates.Add(date);
        _depositRepository.Update(deposit);
    }
    
    public List<Deposit> AvailableDeposits(DateRange date)
    {
        List<Deposit> deposits = _depositRepository.GetAll();
        List<Deposit> availableDeposits = new List<Deposit>();
        
        foreach (Deposit deposit in deposits)
        {
            if (!deposit.IsReserved(date) && DateIsAvailable(deposit, date))
            {
                availableDeposits.Add(deposit);
            }
        }
        
        return availableDeposits; 
    }

    private bool DateIsAvailable(Deposit deposit, DateRange date)
    {
        List<DateRange> availableDates = deposit.AvailableDates;
        foreach (DateRange depositDate in availableDates)
        {
            if (depositDate.Contains(date))
            {
                return true;
            }
        }

        return false;
    }

    private void ValidateDepositIsNotReserved(Deposit deposit, DateRange date)
    {
        if (deposit.IsReserved(date))
        {
            throw new DepositDateIsAlreadyReservedException(DepositDateIsAlreadyreservedMessage);
        }
    }
    
    private void ValidateDepositDateIsNotOverlapping(Deposit deposit, DateRange date)
    {
        List<DateRange> availableDates = deposit.AvailableDates;
        foreach (DateRange depositDate in availableDates)
        {
            if (depositDate.DateRangeIsOverlapping(date))
            {
                throw new DepositDateIsOverlappingException(DepositDateIsOverlappingMessage);
            }
        }
    }
    
    private void Add(Deposit deposit)
    {
        _depositRepository.Add(deposit);
    }
    
    private void ConectDepositToPromotions(Deposit deposit,List<Promotion> promotions)
    {
        foreach (Promotion promotion in promotions)
        {
            deposit.AddPromotion(promotion);
            _depositRepository.Update(deposit);
            
            promotion.AddDeposit(deposit);
            _promotionRepository.Update(promotion);
        }
    }
    
    private void RemoveDepositFromRelatedPromotions(Deposit depositToDelete, List<Promotion> relatedPromotions)
    {
        foreach (Promotion promotion in relatedPromotions)
        {
            promotion.RemoveDeposit(depositToDelete);
            _promotionRepository.Update(promotion);
            _depositRepository.Reload(depositToDelete);
            _depositRepository.Update(depositToDelete);
        }
    }

    private void Delete(Deposit depositToDelete)
    {
        _depositRepository.Reload(depositToDelete);
        _depositRepository.Delete(depositToDelete.Id);
    }
    
    private bool UserLoggedIsAnAdministrator()
    {
        return _session.ActiveUser.IsAdministrator; 
    }
    
    private bool UserIsLogged()
    {
        return _session.UserLoggedIn(); 
    }
    
}