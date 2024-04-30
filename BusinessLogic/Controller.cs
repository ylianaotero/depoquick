using DepoQuick.Domain;
using DepoQuick.Domain.Exceptions.ControllerExceptions;
using DepoQuick.Domain.Exceptions.MemoryDataBaseExceptions;

namespace BusinessLogic;

public class Controller
{
    private readonly MemoryDataBase _memoryDataBase;
    
    public Controller(MemoryDataBase memoryDatabase)
    {
        _memoryDataBase = memoryDatabase;
    }
    
    public void AddDeposit(Deposit deposit)
    {
        _memoryDataBase.GetListOfDeposits().Add(deposit);
    }
    
    public Deposit GetDeposit(int id)
    {
        Deposit deposit = SearchDeposit(id); 
        if (deposit == null)
        {
            throw new DepositNotFoundException("Deposito no encontrado"); 
        }
        else
        {
            return deposit; 
        }
    }
    

    public Deposit SearchDeposit(int id)
    {
        return _memoryDataBase.GetListOfDeposits().FirstOrDefault(deposit => deposit.GetId() == id);
    }
    
    public Reservation GetReservation(int id)
    {
        Reservation reservation = SearchReservation(id); 
        if (reservation == null)
        {
            throw new ReservationNotFoundException("Reservacion no encontrada"); 
        }
        else
        {
            return reservation; 
        }
    }
    
    private Reservation SearchReservation(int id)
    {
        return _memoryDataBase.GetReservations().FirstOrDefault(reservation => reservation.GetId() == id);
    }

    public void AddReservation(Reservation reservation)
    {
        _memoryDataBase.GetReservations().Add(reservation);
    }

    public void AddPromotion(Promotion promotion, List<Deposit> deposits)
    {
        _memoryDataBase.GetPromotions().Add(promotion);
        foreach (Deposit deposit in deposits)
        {
            promotion.AddDeposit(deposit);
            deposit.AddPromotion(promotion);
        }
    }

    public Promotion GetPromotion(int id)
    {
        Promotion promotion = SearchPromotion(id);
        if (promotion == null)
        {
            throw new PromotionNotFoundException("Promoción no encontrada.");
        }
        return promotion;
    }
    
    public List<Promotion> GetPromotions()
    {
        return _memoryDataBase.GetPromotions();
    }

    private Promotion SearchPromotion(int id)
    {
        return _memoryDataBase.GetPromotions().Find(p => p.GetId() == id);
    }

    public void DeletePromotion(int id)
    {
        List<Promotion> promotions = _memoryDataBase.GetPromotions();
        Promotion promotionToDelete = SearchPromotion(id);
        
        List<Deposit> relatedDeposits = promotionToDelete.GetDeposits();
        
        foreach (Deposit deposit in relatedDeposits)
        {
            deposit.GetPromotions().Remove(promotionToDelete);
        }
       
        promotions.Remove(promotionToDelete);
    }

    public void RegisterAdministrator(string name, string email, string password, String validation)
    {
        User.ValidatePasswordConfirmation(password,validation);
        Administrator newAdministrator = new Administrator(name, email, password);
        if (_memoryDataBase.GetListOfUsers().Count == 0)
        {
            _memoryDataBase.GetListOfUsers().Add(newAdministrator);
        }
        else
        {
            throw new AdministratorAlreadyExistsException("El administrador ya fue registrado");
        }
    }

    public void LoginUser(string email, string password)
    {
        bool userExists = false;
        foreach (User us in _memoryDataBase.GetListOfUsers())
        {
            if (us.GetEmail() == email && us.GetPassword() == password)
            {
                _memoryDataBase.setActiveUser(us);
                userExists = true;
            }
        }
        if(!userExists){
            throw new UserDoesNotExistException("No existe un usuario con los datos proporcionados");
        }
    }

    public Administrator GetAdministrator()
    {
        if (_memoryDataBase.GetListOfUsers().Count == 0)
        {
            throw new EmptyUserListException("No hay usuarios registrados");
        }
        else
        {
            Administrator administrator = null;
            foreach (Administrator user in _memoryDataBase.GetListOfUsers())
            {
                if (user.IsAdministrator())
                {
                    administrator = user;
                }
            }

            return administrator;
        }
    }

    public void RegisterClient(string name, string email, string password, string validation)
    {
        if (_memoryDataBase.GetListOfUsers().Count == 0)
        {
            throw new CannotCreateClientBeforeAdminException("Debe registrarse como administrador");
        }
        else
        {
            User.ValidatePasswordConfirmation(password, validation);
            foreach (User user in _memoryDataBase.GetListOfUsers())
            {
                if (user.GetEmail() == email)
                {
                    throw new UserAlreadyExistsException("Un usuario ya fue registrado con ese mail");
                }
            }

            Client client = new Client(name, email, password);
            _memoryDataBase.GetListOfUsers().Add(client);
        }
    }

    public User GetActiveUser()
    {
        return _memoryDataBase.GetActiveUser();
    }

    public List<Deposit> GetDeposits()
    {
        return _memoryDataBase.GetListOfDeposits();
    }
    
    public void DeleteDeposit(int id)
    {
        List<Deposit> deposits = _memoryDataBase.GetListOfDeposits();
        Deposit depositToDelete = SearchDeposit(id);
        
        List<Promotion> relatedPromotions = depositToDelete.GetPromotions();
        
        foreach (Promotion promotion in relatedPromotions)
        {
            promotion.GetDeposits().Remove(depositToDelete);
        }
       
        deposits.Remove(depositToDelete);
    }
    
    public void DeleteAllExpiredPromotions()
    {
        List<Promotion> promotions = _memoryDataBase.GetPromotions();
        List<Promotion> promotionsToDelete = new List<Promotion>();
        
        foreach (Promotion promotion in promotions)
        {
            if (PromotionIsExpired(promotion))
            {
                promotionsToDelete.Add(promotion);
            }
        }
        
        foreach (Promotion promotion in promotionsToDelete)
        {
            DeletePromotion(promotion.GetId());
        }
    }
    
    private bool PromotionIsExpired(Promotion promotion)
    {
        return promotion.GetValidityDate().GetFinalDate() < DateTime.Now;
    }
}
