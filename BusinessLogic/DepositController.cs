using BusinessLogic.Exceptions.ControllerExceptions;
using DepoQuick.Domain;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic;

public class DepositController
{
    private const string DepositNotFoundExceptionMessage = "Deposito no encontrado";
    private const string ActionRestrictedToAdministratorExceptionMessage = "Solo el administrador puede realizar esta acción";
    
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
        if (UserIsLogged() && UserLoggedIsAnAdministrator())
        {
            AddDepositToTheDataBase(deposit);

            ConectDepositToPromotions(deposit, promotions);
        }
        else
        {
            throw new ActionRestrictedToAdministratorException(ActionRestrictedToAdministratorExceptionMessage); 
        }
    }
    
    private bool UserIsLogged()
    {
        return _session.UserLoggedIn(); 
    }
    
    private bool UserLoggedIsAnAdministrator()
    {
        return _session.ActiveUser.IsAdministrator; 
    }
    
    private void AddDepositToTheDataBase(Deposit deposit)
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
    
    public Deposit GetDeposit(int depositId)
    {
        Deposit deposit = SearchDeposit(depositId);
        
        if (deposit == null)
        {
            throw new DepositNotFoundException(DepositNotFoundExceptionMessage); 
        }
        else
        {
            return deposit; 
        }
    }
    
    private Deposit SearchDeposit(int id)
    {
        return _depositRepository.GetById(id);
    }
    
    public List<Deposit> GetDeposits()
    {
        List<Deposit> deposits = _depositRepository.GetAll();
        return deposits; 
    }

    public void DeleteDeposit(int id)
    {
        if (UserIsLogged() && UserLoggedIsAnAdministrator())
        {
            Deposit depositToDelete = SearchDeposit(id);
            
            _depositRepository.Reload(depositToDelete);
        
            List<Promotion> relatedPromotions = depositToDelete.Promotions;

            RemoveDepositFromRelatedPromotions(depositToDelete, relatedPromotions); 
            
            RemoveDeposit(depositToDelete);
        }else
        {
            throw new ActionRestrictedToAdministratorException("No se puede agregar un deposito si sos cliente"); 
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

    private void RemoveDeposit(Deposit depositToDelete)
    {
        _depositRepository.Reload(depositToDelete);
        _depositRepository.Delete(depositToDelete.Id);
    }
    
}