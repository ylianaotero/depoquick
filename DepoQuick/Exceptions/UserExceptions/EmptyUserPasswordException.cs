namespace DepoQuick.Exceptions.UserExceptions;

public class EmptyUserPasswordException : Exception
{
    public EmptyUserPasswordException(string message) : base(message) { }
}