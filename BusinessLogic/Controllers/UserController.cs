using BusinessLogic.Exceptions.UserControllerExceptions;
using DepoQuick.Domain;

namespace BusinessLogic.Controllers;

public class UserController
{
    private const string UserDoesNotExistExceptionMessage = "No se encontro usuario con los datos proporcionados";
    private const string UserAlreadyExistsExceptionMessage = "Ya existe un usuario registrado con ese email";
    private const string AdministratorAlreadyExistsExceptionMessage = "Ya existe un administrador registrado";
    private const string CannotCreateClientBeforeAdminExceptionMessage = "No se puede registrar un cliente sin haber " +
                                                                         "registrado un administrador previamente";
    private const string EmptyAdministratorExceptionMessage = "No hay ningún administrador registrado.";
    
    private IRepository<User> _userRepository;
    
    public UserController(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public User Get(int userId)
    {
        User user = GetById(userId);
        
        return user;
    }
    
    public User GetUserByEmail(string email)
    {
        User user = GetBy(u => u.Email == email);
        
        return user;
    }

    public void RegisterAdministrator(string adminName, string adminEmail, string adminPassword, string passwordValidation)
    {
        User.ValidatePasswordConfirmation(adminPassword,passwordValidation);
        
        if (AdministratorExists())
        {
            throw new AdministratorAlreadyExistsException(AdministratorAlreadyExistsExceptionMessage);
        }
        
        Administrator administrator = new Administrator(adminName, adminEmail, adminPassword);
        Add(administrator);
    }

    public void RegisterClient(string clientName, string clientEmail, string clientPassword, string clientPasswordValidation)
    {
        if (!AdministratorExists())
        {
            throw new CannotCreateClientBeforeAdminException(CannotCreateClientBeforeAdminExceptionMessage);
        }
        
        User.ValidatePasswordConfirmation(clientPassword,clientPasswordValidation);
        
        if (UserExists(clientEmail))
        {
            throw new UserAlreadyExistsException(UserAlreadyExistsExceptionMessage);
        }
        
        Client client = new Client(clientName, clientEmail, clientPassword);
        Add(client);
    }
    
    public Administrator GetAdministrator()
    {
        try
        {
            Administrator admin = GetBy(u => u.IsAdministrator) as Administrator;
            return admin;
        }
        catch (UserDoesNotExistException e)
        {
            throw new EmptyAdministratorException(EmptyAdministratorExceptionMessage);
        }
    }
    
    public bool UserExists(string email)
    {
        try
        {
            User user = GetBy(u => u.Email == email);
            return true;
        }
        catch (UserDoesNotExistException e)
        {
            return false;
        }
    }
    
    private User GetById(int userId)
    {
        
        try
        {
            User reservation = _userRepository.GetById(userId);
            return reservation;
        }
        catch (NullReferenceException e)
        {
            throw new UserDoesNotExistException(UserDoesNotExistExceptionMessage); 
        }
        
    }
    
    private User GetBy(Func<User, bool> predicate)
    {
        
        User user = _userRepository.GetBy(predicate).FirstOrDefault();
        
        if (user == null)
        {
            throw new UserDoesNotExistException(UserDoesNotExistExceptionMessage); 
        }
        
        return user;
    }
    
    private void Add(User newUser)
    {
        _userRepository.Add(newUser);
    }
    
    private bool AdministratorExists()
    {
        return _userRepository.GetBy(u => u.IsAdministrator).Any();
    }
}