using System.Collections;
using BusinessLogic.Exceptions.ControllerExceptions;
using DepoQuick.Domain;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic;

public class PromotionController
{
    private DepoQuickContext _context;
    private Session _session;

    public PromotionController(DepoQuickContext context,Session session)
    {
        _context = context;
        _session = session;
    }
    
    public void Add(Promotion promotion, List<Deposit> deposits)
    {
        if (UserIsLogged() && UserLoggedIsAnAdministrator())
        {
            //AddDepositsToTheDataBase(deposits);
            AddPromotionToTheDataBase(promotion);

            ConectPromotionToDeposits(promotion, deposits);
        }
        else
        {
            throw new ActionRestrictedToAdministratorException("No se puede agregar un deposito si sos cliente"); 
        }
    }
    
    private void AddPromotionToTheDataBase(Promotion promotion)
    {
        _context.Promotions.Add(promotion);
        /*
         int id = GetPromotions().Find(p => p == promotion).Id;
        promotion.Id = id;
         */
        _context.SaveChanges(); 
    }
    
    private bool UserIsLogged()
    {
        return _session.UserLoggedIn(); 
    }
    
    private bool UserLoggedIsAnAdministrator()
    {
        return _session.ActiveUser.IsAdministrator; 
    }

    private void AddDepositsToTheDataBase(List<Deposit> deposits)
    {
        foreach (Deposit deposit in deposits)
        {
            _context.Deposits.Add(deposit);
        }
        _context.SaveChanges(); 
    }
    
    private void ConectPromotionToDeposits(Promotion promotion,List<Deposit> deposits)
    {
        foreach (Deposit deposit in deposits)
        {
            promotion.AddDeposit(deposit);
            deposit.AddPromotion(promotion);
        }
        _context.SaveChanges(); 
    }
    
    public List<Promotion> GetPromotions()
    {
        List<Promotion> promotions = _context.Promotions.ToList();
        return promotions; 
    }

    public Promotion Get(int promotionId)
    {
        Promotion promotion = SearchPromotion(promotionId);
        
        if (promotion == null)
        {
            throw new PromotionNotFoundException("Promocion no encontrada"); 
        }
        else
        {
            return promotion; 
        }
    }

    private Promotion SearchPromotion(int promotionId)
    {
        return _context.Promotions.Find(promotionId);
    }

    public void UpdatePromotionData(Promotion promotion, string label, double discountRate, DateRange validityDate)
    {
        if (_session.ActiveUser.IsAdministrator)
        {
            promotion.Label = label;
            promotion.DiscountRate = discountRate;
            promotion.ValidityDate = validityDate;
        }
        else
        {
            throw new ActionRestrictedToAdministratorException(
                "Solo el administrador puede editar la información de las promociones");
        }
    }

    public void UpdatePromotionDeposits(Promotion promotion, List<Deposit> deposits)
    {
        if (_session.ActiveUser.IsAdministrator)
        {
            List<Deposit> oldDeposits = promotion.Deposits;
            List<Deposit> depositsToRemove = new List<Deposit>();
        
            foreach (var oldDeposit in oldDeposits)
            {
                if (!deposits.Contains(oldDeposit))
                {
                    oldDeposit.RemovePromotion(promotion);
                    depositsToRemove.Add(oldDeposit);
                }
            }
        
            foreach (var deposit in depositsToRemove)
            {
                promotion.RemoveDeposit(deposit);
            }
        
            foreach (var deposit in deposits)
            {
                if (!oldDeposits.Contains(deposit))
                {
                    deposit.AddPromotion(promotion);
                    promotion.AddDeposit(deposit);
                }
            }
        }
        else
        {
            throw new ActionRestrictedToAdministratorException("Solo el administrador puede editar una promocion");
        }
    }
    
    public void DeletePromotion(Promotion promotion)
    {
        if (_session.ActiveUser.IsAdministrator)
        {
            _context.Promotions.Remove(promotion);
            _context.SaveChanges();
        }
        else
        {
            throw new ActionRestrictedToAdministratorException("Solo el administrador puede eliminar promociones");
        }
    }

    public void DeleteAllExpiredPromotions()
    {
        List<Promotion> promotions = GetPromotions();
        
        foreach (Promotion promotion in promotions)
        {
            if (PromotionIsExpired(promotion))
            {
                _context.Promotions.Remove(promotion);
                _context.SaveChanges();
            }
        }
    }
    
    private bool PromotionIsExpired(Promotion promotion)
    {
        return promotion.ValidityDate.GetFinalDate() < DateTime.Now;
    }
}