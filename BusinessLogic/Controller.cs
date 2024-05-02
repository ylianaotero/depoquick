using DepoQuick.Domain;
using DepoQuick.Domain.Exceptions.ControllerExceptions;
using DepoQuick.Domain.Exceptions.MemoryDataBaseExceptions;
using DepoQuick.Domain.Exceptions.UserExceptions;

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
        _memoryDataBase.GetDeposits().Add(deposit);
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
        return _memoryDataBase.GetDeposits().FirstOrDefault(deposit => deposit.GetId() == id);
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
        if (_memoryDataBase.GetUsers().Count == 0)
        {
            Administrator newAdministrator = new Administrator(name, email, password);
            AddUser(newAdministrator);
            _memoryDataBase.SetAdministrator(newAdministrator);
        }
        else
        {
            throw new AdministratorAlreadyExistsException("El administrador ya fue registrado");
        }
    }

    public void LoginUser(string email, string password)
    {
        if (UserExists(email))
        {
            User u = GetUserFromUsersList(email);
            if (u.GetPassword().Equals(password))
            {
                _memoryDataBase.SetActiveUser(u);
            }
            else
            {
                throw new UserPasswordIsInvalidException("La contraseña ingresada no es correcta");
            }
            
        }
        else
        {
            throw new UserDoesNotExistException("No existe un usuario con los datos proporcionados");
        }
    }

    public Administrator GetAdministrator()
    {
        if (_memoryDataBase.GetUsers().Count == 0)
        {
            throw new EmptyUserListException("No hay usuarios registrados");
        }
        else
        {
            return _memoryDataBase.GetAdministrator();
        }
    }

    public void RegisterClient(string name, string email, string password, string validation)
    {
        if (_memoryDataBase.GetUsers().Count == 0)
        {
            throw new CannotCreateClientBeforeAdminException("Debe registrarse como administrador");
        }
        else
        {
            User.ValidatePasswordConfirmation(password, validation);
            if (UserExists(email))
            {
                throw new UserAlreadyExistsException("Un usuario ya fue registrado con ese mail");
            }
            Client client = new Client(name, email, password);
            AddUser(client);
        }
    }

    public User GetActiveUser()
    {
        return _memoryDataBase.GetActiveUser();
    }

    public List<User> GetUsers()
    {
        return _memoryDataBase.GetUsers();
    }


    public List<Deposit> GetDeposits()
    {
        return _memoryDataBase.GetDeposits();
    }
    
    public void DeleteDeposit(int id)
    {
        List<Deposit> deposits = _memoryDataBase.GetDeposits();
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

    public void LogoutUser()
    {
        _memoryDataBase.SetActiveUser(null);
    }

    public bool UserExists(string email)
    {
        bool exists = false;
        foreach (User user in _memoryDataBase.GetUsers())
        {
            if (user.GetEmail() == email)
            {
                exists = true;
            }
        }

        return exists;
    }

    private User GetUserFromUsersList(String email)
    {
        User user = SearchUser(email);
        return user;
    }
    
    private User SearchUser(String email)
    {
        return _memoryDataBase.GetUsers().Find(u => u.GetEmail() == email);
    }

    private void AddUser(User newUser)
    {
        _memoryDataBase.GetUsers().Add(newUser);
    }

    public bool UserLoggedIn()
    {
        return _memoryDataBase.GetActiveUser() != null;
    }
}
