namespace BusinessLogic.Exceptions.ControllerExceptions;

public class UserPasswordIsInvalidException : Exception
{
    public UserPasswordIsInvalidException(string message) : base(message) {}
}