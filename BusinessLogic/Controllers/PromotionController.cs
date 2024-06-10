using BusinessLogic.Exceptions.PromotionControllerExceptions;
using BusinessLogic.Exceptions.UserControllerExceptions;
using DepoQuick.Domain;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Controllers;

public class PromotionController
{
    private const string PromotionNotFoundExceptionMessage = "Promocion no encontrada";
    private const string ActionRestrictedToAdministratorExceptionMessage = "Solo el administrador puede realizar esta acción";
    
    private DepositController _depositController;
    private IRepository<Promotion> _promotionRepository;
    private SessionController _sessionController;

    public PromotionController(IRepository<Promotion> promotionRepository, SessionController sessionController, DepositController depositController)
    {
        _depositController = depositController;
        _promotionRepository = promotionRepository;
        _sessionController = sessionController;
    }
    
    public void Add(Promotion promotion, List<Deposit> deposits)
    {
        RestrictActionToAdministrator();
        
        foreach (var deposit in deposits)
        {
            promotion.Deposits.Add(deposit);
            deposit.Promotions.Add(promotion);
        }
        
        Add(promotion);
        
        
        
    }
    
    public List<Promotion> GetPromotions()
    {
        List<Promotion> promotions = _promotionRepository.GetAll();
        return promotions; 
    }
    
    public void UpdatePromotionData(Promotion promotion, string label, double discountRate, DateRange validityDate)
    {
        RestrictActionToAdministrator();
        
        promotion.Label = label;
        promotion.DiscountRate = discountRate;
        promotion.ValidityDate = validityDate;
            
        _promotionRepository.Update(promotion);
    }
    
    public void UpdatePromotionDeposits(Promotion promotion, List<Deposit> deposits)
    {
        RestrictActionToAdministrator();
        
        RemovePromotionFromDepositsNotInList(promotion, deposits);
        
        AddPromotionToDepositsInList(promotion, deposits);
    }
    
    public void Delete(int id)
    {
        RestrictActionToAdministrator();
        
        Promotion promotionToDelete = Get(id);
            
        Delete(promotionToDelete);
        
        _promotionRepository.Reload(promotionToDelete);
    }
    
    public Promotion Get(int promotionId)
    {
        return GetById(promotionId);
    }
    
    public bool PromotionIsTiedToReservedDeposit(Promotion promotion)
    {
        List<Deposit> deposits = _depositController.GetDepositsByPromotion(promotion);
        
        foreach (Deposit deposit in deposits)
        {
            if (deposit.IsReserved())
            {
                return true;
            }
        }
        
        return false;
    }
    
    private Promotion GetById(int promotionId)
    {
        try
        {
            Promotion promotion = _promotionRepository.GetById(promotionId);
            return promotion;
        }
        catch (NullReferenceException e)
        {
            throw new PromotionNotFoundException(PromotionNotFoundExceptionMessage); 
        }
        
        
    }
    
    private void Delete(Promotion promotionToDelete)
    {
        _promotionRepository.Delete(promotionToDelete.Id);
        _promotionRepository.Reload(promotionToDelete);
    }
    
    
    private void Add(Promotion promotion)
    {
        _promotionRepository.Add(promotion);
    }
    
    private void RestrictActionToAdministrator()
    {
        if (!(UserIsLogged() && UserLoggedIsAnAdministrator()))
        {
            throw new ActionRestrictedToAdministratorException(ActionRestrictedToAdministratorExceptionMessage); 

        }
    }
    
    private bool UserIsLogged()
    {
        return _sessionController.UserLoggedIn(); 
    }
    
    private bool UserLoggedIsAnAdministrator()
    {
        return _sessionController.ActiveUser.IsAdministrator; 
    }
    
    private void RemovePromotionFromDepositsNotInList(Promotion promotion, List<Deposit> deposits)
    {
        List<Deposit> oldDeposits = _depositController.GetDepositsByPromotion(promotion);
        
        foreach (var oldDeposit in oldDeposits)
        {
            if (!deposits.Contains(oldDeposit))
            {
                _depositController.DisconnectPromotionsFromDeposit(oldDeposit, new List<Promotion>() {promotion});
                
            }
        }
    }
    
    private void AddPromotionToDepositsInList(Promotion promotion, List<Deposit> deposits)
    {
        List<Deposit> oldDeposits = _depositController.GetDepositsByPromotion(promotion);
        
        foreach (var deposit in deposits)
        {
            if (!oldDeposits.Contains(deposit))
            {
                _depositController.ConnectPromotionsToDeposit(deposit, new List<Promotion>() {promotion});
                
            }
        }
    }
}