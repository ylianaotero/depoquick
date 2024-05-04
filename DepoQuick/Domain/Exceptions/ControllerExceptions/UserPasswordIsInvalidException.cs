namespace DepoQuick.Domain.Exceptions.ControllerExceptions;

public class UserPasswordIsInvalidException : Exception
{
    public UserPasswordIsInvalidException(string message) : base(message) {}
}