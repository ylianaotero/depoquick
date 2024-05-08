namespace DepoQuick.Exceptions.UserExceptions;

public class InvalidUserNameException : Exception
{
    public InvalidUserNameException(string message) : base(message) {}
}