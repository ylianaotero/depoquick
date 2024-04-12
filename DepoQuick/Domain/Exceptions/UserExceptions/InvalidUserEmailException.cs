namespace DepoQuick.Domain.Exceptions.UserExceptions;

public class InvalidUserEmailException : Exception
{
    public InvalidUserEmailException(string message) : base(message) {}
}