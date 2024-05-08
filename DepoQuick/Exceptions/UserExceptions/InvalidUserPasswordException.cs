namespace DepoQuick.Exceptions.UserExceptions;

public class InvalidUserPasswordException : Exception
{
    public InvalidUserPasswordException(string message) : base(message) { }
}