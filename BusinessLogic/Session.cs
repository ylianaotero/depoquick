using BusinessLogic.Exceptions.ControllerExceptions;
using DepoQuick.Domain;

namespace BusinessLogic;

public class Session
{
    private const string LogInMessage = "Ingresó al sistema";
    private const string LogOutMessage = "Cerró sesión";
    
    private const string UserDoesNotExistExceptionMessage = "No existe un usuario con los datos proporcionados";
    private const string UserPasswordIsInvalidExceptionMessage = "La contraseña ingresada no es correcta";

    private UserController _userController;
    private LogController _logController;
    public User? ActiveUser { get; set; }
    
    public Session(UserController userController, LogController logController)
    {
        _userController = userController;
        _logController = logController;
    }
    
    public bool UserLoggedIn()
    {
        return ActiveUser != null;
    }
    
    public void LogoutUser()
    {
        User user = ActiveUser;
        _logController.LogAction(user, LogOutMessage, DateTime.Now);
        ActiveUser = null;
    }
    
    public void LoginUser(string email, string password)
    {
        if (_userController.UserExists(email))
        {
            User user = _userController.Get(email);
            if (user.Password.Equals(password))
            {
                _logController.LogAction(user, LogInMessage, DateTime.Now);
                
                ActiveUser = user;
            }
            else
            {
                throw new UserPasswordIsInvalidException(UserDoesNotExistExceptionMessage);
            }
            
        }
        else
        {
            throw new UserDoesNotExistException(UserPasswordIsInvalidExceptionMessage);
        }
    }
}