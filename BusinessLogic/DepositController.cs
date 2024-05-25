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
        if (_session.UserLoggedIn() && _session.ActiveUser.IsAdministrator)
        {
            foreach (Promotion promotion in promotions)
            {
                _context.Promotions.Add(promotion);
            }
            _context.SaveChanges(); 
            _context.Deposits.Add(deposit);
            _context.SaveChanges(); 
            foreach (Promotion promotion in promotions)
            {
                deposit.AddPromotion(promotion);
                promotion.AddDeposit(deposit);
            }
            _context.SaveChanges(); 
        }
        else
        {
            throw new ActionRestrictedToAdministratorException("No se puede agregar un deposito si sos cliente"); 
        }
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
        Deposit depositToDelete = SearchDeposit(id);
        
        List<Promotion> relatedPromotions = depositToDelete.Promotions;
        
        foreach (Promotion promotion in relatedPromotions)
        {
            promotion.RemoveDeposit(depositToDelete);
        }
        
        _context.Deposits.Remove(depositToDelete);
        _context.SaveChanges();
    }
    


}