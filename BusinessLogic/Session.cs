using BusinessLogic.Exceptions.ControllerExceptions;
using DepoQuick.Domain;

namespace BusinessLogic;

public class Session
{

    private UserController _userController;
    public User ActiveUser { get; set; }
    
    
    public Session(UserController userController)
    {
        _userController = userController;
    }
    
    public bool UserLoggedIn()
    {
        return ActiveUser != null;
    }
    
    public void LogoutUser()
    {
        User u = ActiveUser;
        u.LogAction("Cerró sesión",DateTime.Now);
        ActiveUser = null;
    }
    
    public void LoginUser(string email, string password)
    {
        if (_userController.UserExists(email))
        {
            User u = _userController.Get(email);
            if (u.Password.Equals(password))
            {
                u.LogAction("Ingresó al sistema",DateTime.Now);
                ActiveUser = u;
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
}