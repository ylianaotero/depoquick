namespace DepoQuick.Exceptions.UserExceptions;

public class InvalidUserEmailException : Exception
{
    public InvalidUserEmailException(string message) : base(message) {}
}