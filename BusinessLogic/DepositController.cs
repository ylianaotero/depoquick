using BusinessLogic.Exceptions.ControllerExceptions;
using DepoQuick.Domain;

namespace BusinessLogic;

public class DepositController
{
    private DepoQuickContext _context;
    
    public DepositController(DepoQuickContext context)
    {
        _context = context;
    }
    
    public void AddDeposit(Deposit deposit, List<Promotion> promotions)
    {
        foreach (Promotion promotion in promotions)
        {
            deposit.AddPromotion(promotion);
            promotion.AddDeposit(deposit);
        }
        _context.Deposits.Add(deposit);
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