using System.Collections;
using BusinessLogic.Exceptions.ControllerExceptions;
using DepoQuick.Domain;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic;

public class PromotionController
{
    private const string PromotionNotFoundExceptionMessage = "Promocion no encontrada";
    private const string ActionRestrictedToAdministratorExceptionMessage = "Solo el administrador puede realizar esta acción";
    
    private IRepository<Deposit> _depositRepository;
    private IRepository<Promotion> _promotionRepository;
    private Session _session;

    public PromotionController(IRepository<Deposit> depositRepository, IRepository<Promotion> promotionRepository, Session session)
    {
        _depositRepository = depositRepository;
        _promotionRepository = promotionRepository;
        _session = session;
    }
    
    public void Add(Promotion promotion, List<Deposit> deposits)
    {
        if (!(UserIsLogged() && UserLoggedIsAnAdministrator()))
        {
            throw new ActionRestrictedToAdministratorException(ActionRestrictedToAdministratorExceptionMessage); 

        }
        
        Add(promotion);

        ConectPromotionToDeposits(promotion, deposits);
    }
    
    public List<Promotion> GetPromotions()
    {
        List<Promotion> promotions = _promotionRepository.GetAll();
        return promotions; 
    }
    
    public void UpdatePromotionData(Promotion promotion, string label, double discountRate, DateRange validityDate)
    {
        if (!UserLoggedIsAnAdministrator())
        {
            throw new ActionRestrictedToAdministratorException(ActionRestrictedToAdministratorExceptionMessage);
        }
        
        promotion.Label = label;
        promotion.DiscountRate = discountRate;
        promotion.ValidityDate = validityDate;
            
        _promotionRepository.Update(promotion);
    }
    
    public void UpdatePromotionDeposits(Promotion promotion, List<Deposit> deposits) //Muy largo
    {
        if (!UserLoggedIsAnAdministrator())
        {
            throw new ActionRestrictedToAdministratorException(ActionRestrictedToAdministratorExceptionMessage);
        }
        
        List<Deposit> oldDeposits = new List<Deposit>(promotion.Deposits);
        List<Deposit> depositsToRemove = new List<Deposit>();
        List<Deposit> depositsToAdd = new List<Deposit>();

        foreach (var oldDeposit in oldDeposits)
        {
            if (!deposits.Contains(oldDeposit))
            {
                oldDeposit.RemovePromotion(promotion);
                _depositRepository.Update(oldDeposit);

                depositsToRemove.Add(oldDeposit);
            }
        }

        foreach (var deposit in deposits)
        {
            if (!oldDeposits.Contains(deposit))
            {
                depositsToAdd.Add(deposit);
            }
        }

        foreach (var deposit in depositsToRemove)
        {
            promotion.RemoveDeposit(deposit);
               
            _promotionRepository.Update(promotion);
        }

        foreach (var deposit in depositsToAdd)
        {
            if (!promotion.Deposits.Contains(deposit))
            {
                deposit.AddPromotion(promotion);
                promotion.AddDeposit(deposit);
            }
        }
    }
    
    public void UpdatePromotionDepositsbla(Promotion promotion, List<Deposit> deposits) //Muy largo?
    {
        if (!UserLoggedIsAnAdministrator())
        {
            throw new ActionRestrictedToAdministratorException(ActionRestrictedToAdministratorExceptionMessage);

        }
        
        List<Deposit> oldDeposits = promotion.Deposits;
        List<Deposit> depositsToRemove = new List<Deposit>();
        
        foreach (var oldDeposit in oldDeposits)
        {
            if (!deposits.Contains(oldDeposit))
            {
                oldDeposit.RemovePromotion(promotion);
                _depositRepository.Update(oldDeposit);
                    
                depositsToRemove.Add(oldDeposit);
            }
        }
        
        foreach (var deposit in depositsToRemove)
        {
            promotion.RemoveDeposit(deposit);
            _promotionRepository.Update(promotion);
        }
        
        foreach (var deposit in deposits)
        {
            if (!oldDeposits.Contains(deposit))
            {
                deposit.AddPromotion(promotion);
                promotion.AddDeposit(deposit);
                    
                _depositRepository.Update(deposit);
                _promotionRepository.Update(promotion);
            }
        }
    }
    
    public void DeleteAllExpiredPromotions()
    {
        List<Promotion> promotions = GetPromotions();
        
        foreach (Promotion promotion in promotions)
        {
            if (PromotionIsExpired(promotion))
            {
                Delete(promotion.Id);
            }
        }
    }
    
    public void Delete(int id)
    {
        if (!(UserIsLogged() && UserLoggedIsAnAdministrator()))
        {
            throw new ActionRestrictedToAdministratorException(ActionRestrictedToAdministratorExceptionMessage); 
        }
        
        Promotion promotionToDelete = Get(id);
            
        _promotionRepository.Reload(promotionToDelete);
        
        List<Deposit> relatedDeposits = promotionToDelete.Deposits;

        RemovePromotionFromRelatedDeposits(promotionToDelete, relatedDeposits); 
            
        Delete(promotionToDelete);
    }
    
    public Promotion Get(int promotionId)
    {
        Promotion promotion = _promotionRepository.GetById(promotionId);
        
        if (promotion == null)
        {
            throw new PromotionNotFoundException(PromotionNotFoundExceptionMessage); 
        }
        
        return promotion; 
    }
    
    private void Delete(Promotion promotionToDelete)
    {
        _promotionRepository.Reload(promotionToDelete);
        _promotionRepository.Delete(promotionToDelete.Id);
    }
    
    private void RemovePromotionFromRelatedDeposits(Promotion promotionToDelete, List<Deposit> relatedDeposits)
    {
        foreach (Deposit deposit in relatedDeposits)
        {
            deposit.RemovePromotion(promotionToDelete);
           // _depositRepository.Update(deposit);
        }
    }
    
    private bool PromotionIsExpired(Promotion promotion)
    {
        return promotion.ValidityDate.GetFinalDate() < DateTime.Now;
    }
    
    private void ConectPromotionToDeposits(Promotion promotion,List<Deposit> deposits)
    {
        foreach (Deposit deposit in deposits)
        {
            deposit.AddPromotion(promotion);
            //    _depositRepository.Update(deposit);
            
            promotion.AddDeposit(deposit);
            //   _promotionRepository.Update(promotion);
        }
    }
    
    private void Add(Promotion promotion)
    {
        _promotionRepository.Add(promotion);
    }

    private bool UserIsLogged()
    {
        return _session.UserLoggedIn(); 
    }
    
    private bool UserLoggedIsAnAdministrator()
    {
        return _session.ActiveUser.IsAdministrator; 
    }
}