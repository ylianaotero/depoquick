namespace DepoQuick.Exceptions.UserExceptions;

public class EmptyActionLogException : Exception
{
    public EmptyActionLogException(string message) : base(message) { }
}