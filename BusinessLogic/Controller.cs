using BusinessLogic.Exceptions.ControllerExceptions;
using DepoQuick.Domain;

namespace BusinessLogic;

public class Controller
{
    private readonly MemoryDataBase _memoryDataBase;
    
    public Controller(MemoryDataBase memoryDatabase)
    {
        _memoryDataBase = memoryDatabase;
    }
    
    public void AddDeposit(Deposit deposit, List<Promotion> promotions)
    {
        if (GetActiveUser().IsAdministrator())
        {
            GetDeposits().Add(deposit);
            foreach (Promotion promotion in promotions)
            {
                deposit.AddPromotion(promotion);
                promotion.AddDeposit(deposit);
            }
        }
        else
        {
            throw new ActionRestrictedToAdministratorException("Sólo el administrador puede agregar depositos");
        }
        
    }

    public void AddReservation(Reservation reservation)
    {
        GetReservations().Add(reservation);
        Client client = reservation.GetClient();
        client.AddReservation(reservation);

    }

    public void AddPromotion(Promotion promotion, List<Deposit> deposits)
    {
        if (GetActiveUser().IsAdministrator())
        {
            GetPromotions().Add(promotion);
            foreach (Deposit deposit in deposits)
            {
                promotion.AddDeposit(deposit);
                deposit.AddPromotion(promotion);
            }   
        }
        else
        {
            throw new ActionRestrictedToAdministratorException("Solo el administrador puede registrar promociones");
        }
    }

    public void UpdatePromotionData(Promotion promotion, string label, double discountRate, DateRange validityDate)
    {
        if (GetActiveUser().IsAdministrator())
        {
            promotion.SetLabel(label);
            promotion.SetDiscountRate(discountRate);
            promotion.SetValidityDate(validityDate);
        }
        else
        {
            throw new ActionRestrictedToAdministratorException(
                "Solo el administrador puede editar la información de las promociones");
        }
        
    }
    
    public void UpdatePromotionDeposits(Promotion promotion, List<Deposit> deposits)
    {
        if (GetActiveUser().IsAdministrator())
        {
            List<Deposit> oldDeposits = promotion.GetDeposits();
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

    public void DeletePromotion(int id)
    {
        if (GetActiveUser().IsAdministrator())
        {
            List<Promotion> promotions = GetPromotions();
            Promotion promotionToDelete = SearchPromotion(id);
        
            List<Deposit> relatedDeposits = promotionToDelete.GetDeposits();
        
            foreach (Deposit deposit in relatedDeposits)
            {
                deposit.RemovePromotion(promotionToDelete);
            }
       
            promotions.Remove(promotionToDelete);
        }
        else
        {
            throw new ActionRestrictedToAdministratorException("Solo el administrador puede eliminar promociones");
        }
    }
    
    public void RegisterAdministrator(string name, string email, string password, String validation)
    {
        User.ValidatePasswordConfirmation(password,validation);
        if (GetUsers().Count == 0)
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
    
    public void RegisterClient(string name, string email, string password, string validation)
    {
        if (GetUsers().Count == 0)
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

    public void LoginUser(string email, string password)
    {
        if (UserExists(email))
        {
            User u = GetUserFromUsersList(email);
            if (u.GetPassword().Equals(password))
            {
                u.LogAction("Ingresó al sistema",DateTime.Now);
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
    
    public void LogoutUser()
    {
        User u = GetActiveUser();
        u.LogAction("Cerró sesión",DateTime.Now);
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
    
    private void AddUser(User newUser)
    {
        _memoryDataBase.GetUsers().Add(newUser); 
    }

    public bool UserLoggedIn()
    {
        return _memoryDataBase.GetActiveUser() != null;
    }
    
    public void DeleteDeposit(int id)
    {
        if (GetActiveUser().IsAdministrator())
        {
            List<Deposit> deposits = GetDeposits();
            Deposit depositToDelete = SearchDeposit(id);
        
            List<Promotion> relatedPromotions = depositToDelete.GetPromotions();
        
            foreach (Promotion promotion in relatedPromotions)
            {
                promotion.RemoveDeposit(depositToDelete);
            }
       
            deposits.Remove(depositToDelete);   
        }
        else
        {
            throw new ActionRestrictedToAdministratorException("Sólo los administradores pueden eliminar un deposito");
        }
    }
    
    public void DeleteAllExpiredPromotions()
    {
        List<Promotion> promotions = GetPromotions();
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
    
    public void ApproveReservation(Reservation reservation)
    {
        Administrator admin = (Administrator)_memoryDataBase.GetActiveUser();
        
        admin.ApproveReservation(reservation);
    }
    
    public void RejectReservation(Reservation reservation, string reason)
    {
        Administrator admin = (Administrator)GetActiveUser();
        
        admin.RejectReservation(reservation, reason);
    }

    public void CancelRejectionOfReservation(Reservation reservation)
    {
        if (GetActiveUser().IsAdministrator())
        {
            reservation.SetState(0);
            reservation.GetDeposit().AddReservation(reservation);
        }
        else
        {
            throw new ActionRestrictedToAdministratorException("Solo el administrador puede cancelar una reserva");
        }
    }
    
    public void RateReservation(Reservation reservation, Rating rating)
    {
        if (!GetActiveUser().IsAdministrator())
        {
            Deposit deposit = reservation.GetDeposit();
            deposit.AddRating(rating); 
            reservation.SetRating(rating);
            GetRatings().Add(rating);
            reservation.GetClient().LogAction("Agregó valoración de la reserva " + reservation.GetId(),DateTime.Now);
        }
        else
        {
            throw new ActionRestrictedToClientException("Solo el cliente puede calificar una reserva");
        }
        
    }
    
    private User GetUserFromUsersList(String email)
    {
        User user = SearchUser(email);
        return user;
    }
    
    public List<(String,DateTime)> GetLogs(User user)
    {
        if (GetActiveUser().IsAdministrator())
        {
            return user.GetLogs();
        }
        else
        {
            throw new ActionRestrictedToAdministratorException("Solo el administrador puede ver los logs");
        }
        
    }
    
    public User GetActiveUser()
    {
        return _memoryDataBase.GetActiveUser();
    }
    
    public Administrator GetAdministrator()
    {
        Administrator administrator = _memoryDataBase.GetAdministrator(); 
        if (administrator  == null)  
        {
            throw new EmptyAdministratorException("No hay administrador registrado");
        }
        else
        {
            return administrator;
        }
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
    
    public Promotion GetPromotion(int id)
    {
        Promotion promotion = SearchPromotion(id);
        if (promotion == null)
        {
            throw new PromotionNotFoundException("Promoción no encontrada.");
        }
        return promotion;
    }
    
    public List<User> GetUsers()
    {
        return _memoryDataBase.GetUsers();
    }
    
    public List<Rating> GetRatings()
    {
        return _memoryDataBase.GetRatings();
    }
    
    public List<Deposit> GetDeposits()
    {
        return _memoryDataBase.GetDeposits();
    }
    
    public List<Reservation> GetReservations()
    {
        return _memoryDataBase.GetReservations();
    } 
    
    public List<Promotion> GetPromotions()
    {
        return _memoryDataBase.GetPromotions();
    }
    
    private Reservation SearchReservation(int id)
    {
        return _memoryDataBase.GetReservations().FirstOrDefault(reservation => reservation.GetId() == id);
    }
    
    private Promotion SearchPromotion(int id)
    {
        return _memoryDataBase.GetPromotions().Find(p => p.GetId() == id);
    }
    
    private User SearchUser(String email)
    {
        return _memoryDataBase.GetUsers().Find(u => u.GetEmail() == email);
    }
    
    public Deposit SearchDeposit(int id)
    {
        return _memoryDataBase.GetDeposits().FirstOrDefault(deposit => deposit.GetId() == id);
    }
    
}
