namespace BusinessLogic.Exceptions.UserControllerExceptions;

public class UserDoesNotExistException : Exception
{
    public UserDoesNotExistException(string message) : base(message) {} 
}