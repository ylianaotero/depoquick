using BusinessLogic.Exceptions.ControllerExceptions;
using DepoQuick.Domain;

namespace BusinessLogic;

public class DepositController
{
    private DepoQuickContext _context;
    private Session _session;
    
    public DepositController(DepoQuickContext context,Session session )
    {
        _context = context;
        _session = session; 
    }
    
    public void AddDeposit(Deposit deposit, List<Promotion> promotions)
    {
        if (UserIsLogged() && UserLoggedIsAnAdministrator())
        {
           // AddPromotionsToTheDataBase(promotions);

            AddDepositToTheDataBase(deposit);

            ConectDepositToPromotions(deposit, promotions);
        }
        else
        {
            throw new ActionRestrictedToAdministratorException("No se puede agregar un deposito si sos cliente"); 
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
        _context.Deposits.Add(deposit);
        _context.SaveChanges(); 
    }
    
    private void AddPromotionsToTheDataBase(List<Promotion> promotions)
    {
        foreach (Promotion promotion in promotions)
        {
            _context.Promotions.Add(promotion);
        }
        _context.SaveChanges(); 
    }
    
    private void ConectDepositToPromotions(Deposit deposit,List<Promotion> promotions)
    {
        foreach (Promotion promotion in promotions)
        {
            deposit.AddPromotion(promotion);
            promotion.AddDeposit(deposit);
        }
        _context.SaveChanges(); 
    }

    
    public Deposit GetDeposit(int depositId)
    {
        Deposit deposit = SearchDeposit(depositId);
        
        if (deposit == null)
        {
            throw new DepositNotFoundException("Deposito no encontrado"); 
        }
        else
        {
            return deposit; 
        }
    }
    
    private Deposit SearchDeposit(int id)
    {
        return _context.Deposits.Find(id); 
    }
    
    public List<Deposit> GetDeposits()
    {
        List<Deposit> deposits = _context.Deposits.ToList();
        return deposits; 
    }

    public void DeleteDeposit(int id)
    {
        if (UserIsLogged() && UserLoggedIsAnAdministrator())
        {
            Deposit depositToDelete = SearchDeposit(id);
        
            List<Promotion> relatedPromotions = depositToDelete.Promotions;

            RemoveDepositToRelatedPromotions(depositToDelete, relatedPromotions); 
            
            RemoveDepositToTheDataBase(depositToDelete);
        }else
        {
            throw new ActionRestrictedToAdministratorException("No se puede agregar un deposito si sos cliente"); 
        }

    }

    private void RemoveDepositToRelatedPromotions(Deposit depositToDelete, List<Promotion> relatedPromotions)
    {
        foreach (Promotion promotion in relatedPromotions)
        {
            promotion.RemoveDeposit(depositToDelete);
        }
    }

    private void RemoveDepositToTheDataBase(Deposit depositToDelete)
    {
        _context.Deposits.Remove(depositToDelete);
        _context.SaveChanges();
    }
    


}