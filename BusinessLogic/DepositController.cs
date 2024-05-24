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
        Deposit deposit = _context.Deposits.Find(depositId);
        if (deposit == null)
        {
            throw new DepositNotFoundException("Deposito no encontrado"); 
        }
        else
        {
            return deposit; 
        }
    }
    
    public List<Deposit> GetDeposits()
    {
        List<Deposit> deposits = _context.Deposits.ToList();
        return deposits; 
    }
    


}