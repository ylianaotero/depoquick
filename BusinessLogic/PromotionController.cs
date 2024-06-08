using BusinessLogic.Exceptions.ControllerExceptions;
using DepoQuick.Domain;

namespace BusinessLogic;

public class PromotionController
{
    private const string PromotionNotFoundExceptionMessage = "Promocion no encontrada";
    private const string ActionRestrictedToAdministratorExceptionMessage = "Solo el administrador puede realizar esta acción";
    
    private DepositController _depositController;
    private IRepository<Promotion> _promotionRepository;
    private Session _session;

    public PromotionController(IRepository<Promotion> promotionRepository, Session session, DepositController depositController)
    {
        _depositController = depositController;
        _promotionRepository = promotionRepository;
        _session = session;
    }
    
    public void Add(Promotion promotion, List<Deposit> deposits)
    {
        RestrictActionToAdministrator();
        
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
        Promotion promotion = _promotionRepository.GetById(promotionId);
        
        if (promotion == null)
        {
            throw new PromotionNotFoundException(PromotionNotFoundExceptionMessage); 
        }
        
        return promotion; 
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

    private void RestrictActionToAdministrator()
    {
        if (!(UserIsLogged() && UserLoggedIsAnAdministrator()))
        {
            throw new ActionRestrictedToAdministratorException(ActionRestrictedToAdministratorExceptionMessage); 

        }
    }
    
    private void Delete(Promotion promotionToDelete)
    {
        _promotionRepository.Delete(promotionToDelete.Id);
        _promotionRepository.Reload(promotionToDelete);
    }
    
    private void ConectPromotionToDeposits(Promotion promotion,List<Deposit> deposits)
    {
        foreach (Deposit deposit in deposits)
        {
            deposit.AddPromotion(promotion);
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