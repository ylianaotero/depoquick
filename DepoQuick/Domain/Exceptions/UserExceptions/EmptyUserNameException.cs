namespace DepoQuick.Domain.Exceptions.UserExceptions;

public class EmptyUserNameException : Exception
{
    public EmptyUserNameException(string message) : base(message) { }
}