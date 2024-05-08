namespace DepoQuick.Exceptions.UserExceptions;

public class EmptyUserNameException : Exception
{
    public EmptyUserNameException(string message) : base(message) { }
}