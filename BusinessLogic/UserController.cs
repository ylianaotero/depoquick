using System.Reflection.Metadata.Ecma335;
using BusinessLogic.Exceptions.ControllerExceptions;
using DepoQuick.Domain;

namespace BusinessLogic;

public class UserController
{
    private const string UserNotFoundExceptionMessage = "No se encontro usuario con los datos proporcionados";
    private const string UserAlreadyExistsExceptionMessage = "Ya existe un usuario registrado con ese email";
    private const string AdministratorAlreadyExistsExceptionMessage = "Ya existe un administrador registrado";
    private const string CannotCreateClientBeforeAdminExceptionMessage = "No se puede registrar un cliente sin haber registrado un administrador previamente";
    private const string EmptyAdministratorExceptionMessage = "No hay ningún administrador registrado.";
    private const string UserDoesNotExistMessage = "El usuario no existe";

    
    private IRepository<User> _userRepository;
    
    public UserController(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public User Get(int userId)
    {
        User user = _userRepository.GetById(userId);
        if (user == null)
        {
            throw new UserDoesNotExistException(UserNotFoundExceptionMessage);
        }
        
        return user;
    }
    
    public User GetUserByEmail(string email)
    {
        User user = _userRepository.GetBy(u => u.Email == email).FirstOrDefault();
        if (user == null)
        {
            throw new UserDoesNotExistException(UserNotFoundExceptionMessage);
        }
        
        return user;
    }
    
    public bool UserExists(string email)
    {
        User user = _userRepository.GetBy(u => u.Email == email).FirstOrDefault();
        if (user == null)
        {
            return false;
        }
        
        return true;
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
        Administrator admin  = _userRepository.GetBy(u => u.IsAdministrator).FirstOrDefault() as Administrator;
        if (admin == null)
        {
            throw new EmptyAdministratorException(EmptyAdministratorExceptionMessage);
        }
        return admin;
    }

    public void Delete(User user) //Hay excepciones? Primero lo busco y despues lo elimino?
    {
        if (user == null)
        {
            throw new UserDoesNotExistException(UserDoesNotExistMessage);
        }

        _userRepository.Reload(user);
        User userToDelete = _userRepository.GetById(user.Id);
        if (userToDelete == null)
        {
            throw new UserDoesNotExistException(UserDoesNotExistMessage);
        }

        _userRepository.Delete(userToDelete.Id);
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