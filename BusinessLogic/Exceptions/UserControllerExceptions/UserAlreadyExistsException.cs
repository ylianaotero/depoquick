namespace BusinessLogic.Exceptions.UserControllerExceptions;

public class UserAlreadyExistsException : Exception
{
    public UserAlreadyExistsException(string message) : base(message) {}   
}