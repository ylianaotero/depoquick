namespace BusinessLogic.Exceptions.UserControllerExceptions;

public class UserPasswordIsInvalidException : Exception
{
    public UserPasswordIsInvalidException(string message) : base(message) {}
}