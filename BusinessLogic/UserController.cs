using BusinessLogic.Exceptions.ControllerExceptions;
using DepoQuick.Domain;
using DepoQuick.Exceptions.UserExceptions;
using SQLitePCL;

namespace BusinessLogic;

public class UserController
{
    private DepoQuickContext _context;
    public UserController(DepoQuickContext context)
    {
        _context = context;
    }

    private void Add(User newUser)
    {
        _context.Users.Add(newUser);
        _context.SaveChanges();
    }

    public User Get(int userId)
    {
        User user = _context.Users.Find(userId);
        if (user == null)
        {
            throw new UserDoesNotExistException("El usuario buscado no existe");
        }
        else
        {
            return user;
        }
    }
    
    public void LogAction(User user, string message, DateTime timestamp)
    {
        if (string.IsNullOrEmpty(message))
        {
            throw new EmptyActionLogException("El mensaje no puede estar vacío.");
        }
        else
        {
            LogEntry log = new LogEntry()
            {
                Message = message,
                Timestamp = timestamp,
                UserId = user.Id
            };

            _context.LogEntries.Add(log); 
            _context.SaveChanges();
        }
    }
    
    public User Get(string email)
    {
        User user = _context.Users.FirstOrDefault(u =>u.Email==email);
        if (user == null)
        {
            throw new UserDoesNotExistException("El usuario buscado no existe");
        }
        else
        {
            return user;
        }
    }
    
    public bool UserExists(string email)
    {
        User user = _context.Users.FirstOrDefault(u =>u.Email==email);
        if (user == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void Remove(int id){
        User user = _context.Users.Find(id);
        if (user == null)
        {
            throw new UserDoesNotExistException("El usuario que se intenta eliminar no existe");
        }
        _context.Users.Remove(user);
        _context.SaveChanges();
    }

    public void RegisterAdministrator(string adminName, string adminEmail, string adminPassword, string passwordValidation)
    {
        User.ValidatePasswordConfirmation(adminPassword,passwordValidation);
        if (!_context.Administrators.Any())
        {
            Administrator administrator = new Administrator(adminName, adminEmail, adminPassword);
            Add(administrator);
        }
        else
        {
            throw new AdministratorAlreadyExistsException("Ya existe un administador");
        }
    }

    public void RegisterClient(string clientName, string clientEmail, string clientPassword, string clientPasswordValidation)
    {
        if (!_context.Users.Any())
        {
            throw new CannotCreateClientBeforeAdminException(
                "No se puede registrar un cliente sin haber registrado un administrador previamente");
        }
        else
        {
            User.ValidatePasswordConfirmation(clientPassword,clientPasswordValidation);
            if (UserExists(clientEmail))
            {
                throw new UserAlreadyExistsException("Ya existe un usuario registrado con ese email");
            }
            Client client = new Client(clientName, clientEmail, clientPassword);
            Add(client);
        }
    }

    public Administrator GetAdministrator()
    {
        Administrator admin = _context.Users.OfType<Administrator>().FirstOrDefault();
        if (admin == null)
        {
            throw new EmptyAdministratorException("No hay ningún administrador registrado.");
        }
        return admin;
    }
    
    public List<LogEntry> GetLogs(User userToGetLogs, User activeUser)
    {
        if (activeUser.IsAdministrator)
        {
            return userToGetLogs.Logs;
        }
        else
        {
            throw new ActionRestrictedToAdministratorException("Solo el administrador puede ver los logs");
        }
        
    }
}