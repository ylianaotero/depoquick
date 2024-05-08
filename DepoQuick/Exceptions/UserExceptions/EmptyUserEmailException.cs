namespace DepoQuick.Exceptions.UserExceptions;

public class EmptyUserEmailException : Exception
{
    public EmptyUserEmailException(string message) : base(message) { }
}