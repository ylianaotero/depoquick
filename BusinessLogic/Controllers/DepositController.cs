using BusinessLogic.Exceptions.DepositControllerExceptions;
using BusinessLogic.Exceptions.UserControllerExceptions;
using DepoQuick.Domain;

namespace BusinessLogic.Controllers;

public class DepositController
{
    private const string DepositDateIsAlreadyreservedMessage = "El deposito no esta disponible en esa fecha";
    private const string DepositNotFoundExceptionMessage = "Deposito no encontrado";
    private const string ActionRestrictedToAdministratorExceptionMessage = "Solo el administrador puede realizar esta acción";
    private const string DepositDateIsOverlappingMessage = "El deposito ya tiene una reserva en esa fecha";
    private const string DepositAlreadyExistsMessage = "Ya existe un deposito con ese nombre";
    
    private IRepository<Deposit> _depositRepository;
    private SessionController _sessionController;
    
    public DepositController(IRepository<Deposit> depositRepository, SessionController sessionController)
    {
        _depositRepository = depositRepository;
        _sessionController = sessionController; 
    }
    
    public void AddDeposit(Deposit deposit, List<Promotion> promotions)
    {
        RestrictActionToAdministrator();
        
        if (DepositExists(deposit.Name)){
            throw new DepositNameAlreadyExistsException(DepositAlreadyExistsMessage);
        }
        
        Add(deposit);

        ConnectPromotionsToDeposit(deposit, promotions);
    }
    
    public Deposit Get(int id)
    {
        return GetById(id); 
    }
    
    public Deposit GetDepositByName(String name)
    {
        return GetBy(d => d.Name == name);
    }
    
    public List<Deposit> GetDepositsByPromotion(Promotion promotion)
    {
        List<Deposit> deposits = GetDeposits();
        List<Deposit> depositsWithPromotion = new List<Deposit>();
        
        foreach (Deposit deposit in deposits)
        {
            if (deposit.Promotions.Contains(promotion))
            {
                depositsWithPromotion.Add(deposit);
            }
        }
        
        return depositsWithPromotion; 
    }

    public bool DepositExists(string name)
    {
        return _depositRepository.GetAll().Any(d => d.Name == name);
    }

    public List<Deposit> GetDeposits()
    {
        List<Deposit> deposits = _depositRepository.GetAll();
        
        return deposits; 
    }

    public void DeleteDeposit(Deposit depositToDelete)
    {
        RestrictActionToAdministrator();
            
        _depositRepository.Reload(depositToDelete);
            
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
    
    public void ConnectPromotionsToDeposit(Deposit deposit, List<Promotion> promotions)
    {
        foreach (Promotion promotion in promotions)
        {
            if (!deposit.Promotions.Contains(promotion))
            {
                deposit.AddPromotion(promotion);
                _depositRepository.Update(deposit);
            }
        }
    }
    
    private Deposit GetById(int depositId)
    {
        Deposit deposit = new Deposit();
        
        try
        {
            deposit = _depositRepository.GetById(depositId);
        }
        catch (NullReferenceException e)
        {
            throw new DepositNotFoundException(DepositNotFoundExceptionMessage); 
        }
        
        if (deposit == null)
        {
            throw new DepositNotFoundException(DepositNotFoundExceptionMessage); 
        }
        
        return deposit;
    }
    
    private Deposit GetBy(Func<Deposit, bool> predicate)
    {
        Deposit deposit = new Deposit();
        
        try
        {
            deposit = _depositRepository.GetBy(predicate).FirstOrDefault();
        }
        catch (NullReferenceException e)
        {
            throw new DepositNotFoundException(DepositNotFoundExceptionMessage); 
        }
        
        if (deposit == null)
        {
            throw new DepositNotFoundException(DepositNotFoundExceptionMessage); 
        }
        
        return deposit;
    }
    
    public void DisconnectPromotionsFromDeposit(Deposit deposit, List<Promotion> promotions)
    {
        foreach (Promotion promotion in promotions)
        {
            if (deposit.Promotions.Contains(promotion))
            {
                deposit.RemovePromotion(promotion);
                _depositRepository.Update(deposit);
            }
        }
    }
    
    private bool DateIsAvailable(Deposit deposit, DateRange date)
    {
        bool isAvailable = false;
        List<DateRange> availableDates = deposit.AvailableDates;
        foreach (DateRange depositDate in availableDates)
        {
            if (depositDate.Contains(date))
            {
                 isAvailable = true;
            }
        }
        
        return isAvailable;
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
    
    private void Delete(Deposit depositToDelete)
    {
        _depositRepository.Reload(depositToDelete);
        _depositRepository.Delete(depositToDelete.Id);
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
}