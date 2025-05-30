﻿using BusinessLogic.Exceptions.UserControllerExceptions;
using DepoQuick.Domain;

namespace BusinessLogic.Controllers;

public class SessionController
{
    private const string LogInMessage = "Ingresó al sistema";
    private const string LogOutMessage = "Cerró sesión";
    
    private const string UserDoesNotExistExceptionMessage = "No existe un usuario con los datos proporcionados";
    private const string UserPasswordIsInvalidExceptionMessage = "La contraseña ingresada no es correcta";

    private UserController _userController;
    private LogController _logController;
    public User ActiveUser { get; set; }
    
    public SessionController(UserController userController, LogController logController)
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
        if (!_userController.UserExists(email))
        {
            throw new UserDoesNotExistException(UserPasswordIsInvalidExceptionMessage);
        }
        
        User user = _userController.GetUserByEmail(email);
        if (!user.Password.Equals(password))
        {
            throw new UserPasswordIsInvalidException(UserDoesNotExistExceptionMessage);
        }
        
        _logController.LogAction(user, LogInMessage, DateTime.Now);
                
        ActiveUser = user;
    }
}